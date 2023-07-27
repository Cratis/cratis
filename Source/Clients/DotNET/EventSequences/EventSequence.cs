// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Connections;
using Aksio.Cratis.Events;
using Aksio.Cratis.Observation;

namespace Aksio.Cratis.EventSequences;

/// <summary>
/// Represents an implementation of <see cref="IEventSequence"/>.
/// </summary>
public class EventSequence : IEventSequence
{
    readonly TenantId _tenantId;
    readonly EventSequenceId _eventSequenceId;
    readonly IEventTypes _eventTypes;
    readonly IEventSerializer _eventSerializer;
    readonly IConnection _connection;
    readonly IObserversRegistrar _observerRegistrar;
    readonly IExecutionContextManager _executionContextManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSequence"/> class.
    /// </summary>
    /// <param name="tenantId"><see cref="TenantId"/> the sequence is for.</param>
    /// <param name="eventSequenceId">The event sequence it represents.</param>
    /// <param name="eventTypes">Known <see cref="IEventTypes"/>.</param>
    /// <param name="eventSerializer">The <see cref="IEventSerializer"/> for serializing events.</param>
    /// <param name="connection"><see cref="IConnection"/> for getting connections.</param>
    /// <param name="observerRegistrar"><see cref="IObserversRegistrar"/> for working with client observers.</param>
    /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with the execution context.</param>
    public EventSequence(
        TenantId tenantId,
        EventSequenceId eventSequenceId,
        IEventTypes eventTypes,
        IEventSerializer eventSerializer,
        IConnection connection,
        IObserversRegistrar observerRegistrar,
        IExecutionContextManager executionContextManager)
    {
        _tenantId = tenantId;
        _eventSequenceId = eventSequenceId;
        _eventTypes = eventTypes;
        _eventSerializer = eventSerializer;
        _connection = connection;
        _observerRegistrar = observerRegistrar;
        _executionContextManager = executionContextManager;
    }

    /// <inheritdoc/>
    public async Task<EventSequenceNumber> GetNextSequenceNumber()
    {
        var route = $"{GetBaseRoute()}/next-sequence-number";
        var result = await _connection.PerformQuery<EventSequenceNumber>(route, metadata: new { EventSequenceId = _eventSequenceId });
        return result.Data;
    }

    /// <inheritdoc/>
    public async Task<EventSequenceNumber> GetTailSequenceNumber()
    {
        var route = $"{GetBaseRoute()}/tail-sequence-number";
        var result = await _connection.PerformQuery<EventSequenceNumber>(route, metadata: new { EventSequenceId = _eventSequenceId });
        return result.Data;
    }

    /// <inheritdoc/>
    public async Task<EventSequenceNumber> GetTailSequenceNumberForObserver(Type type)
    {
        var observer = _observerRegistrar.GetByType(type);
        var route = $"{GetBaseRoute()}/tail-sequence-number/observer/{observer.ObserverId}";
        var result = await _connection.PerformQuery<EventSequenceNumber>(route, metadata: new { EventSequenceId = _eventSequenceId });
        return result.Data;
    }

    /// <inheritdoc/>
    public async Task Append(EventSourceId eventSourceId, object @event, DateTimeOffset? validFrom = null)
    {
        var route = GetBaseRoute();
        var payload = await CreateAppendEvent(eventSourceId, @event, validFrom);
        await _connection.PerformCommand(
            route,
            payload,
            new
            {
                EventSequenceId = _eventSequenceId,
                EventSourceId = eventSourceId,
                payload.EventType
            });
    }

    /// <inheritdoc/>
    public Task AppendMany(EventSourceId eventSourceId, IEnumerable<object> events) =>
        AppendMany(eventSourceId, events.Select(_ => new EventAndValidFrom(_, null)).ToArray());

    /// <inheritdoc/>
    public async Task AppendMany(EventSourceId eventSourceId, IEnumerable<EventAndValidFrom> events)
    {
        var tasks = events.Select(_ => CreateAppendEvent(eventSourceId, _.Event, _.ValidFrom));
        var payload = Task.WhenAll(tasks.ToArray());
        var route = $"{GetBaseRoute()}/many";
        await _connection.PerformCommand(
            route,
            payload,
            new
            {
                EventSequenceId = _eventSequenceId,
                EventSourceId = eventSourceId,
            });
    }

    /// <inheritdoc/>
    public async Task Redact(EventSequenceNumber sequenceNumber, RedactionReason? reason = default)
    {
        reason ??= RedactionReason.Unknown;
        var payload = new RedactEvent(sequenceNumber, reason);
        var route = $"{GetBaseRoute()}/redact-event";
        await _connection.PerformCommand(route, payload, new { EventSequenceId = _eventSequenceId });
    }

    /// <inheritdoc/>
    public async Task Redact(EventSourceId eventSourceId, RedactionReason? reason = default, params Type[] eventTypes)
    {
        reason ??= RedactionReason.Unknown;
        var eventTypeIds = eventTypes.Select(_ => _eventTypes.GetEventTypeFor(_).Id).ToArray();
        var payload = new RedactEvents(eventSourceId, reason, eventTypeIds);
        var route = $"{GetBaseRoute()}/redact-events";
        await _connection.PerformCommand(route, payload, new { EventSequenceId = _eventSequenceId });
    }

    string GetBaseRoute() => $"/api/events/store/{_executionContextManager.Current.MicroserviceId}/{_tenantId}/sequence/{_eventSequenceId}";

    async Task<AppendEvent> CreateAppendEvent(EventSourceId eventSourceId, object @event, DateTimeOffset? validFrom = default)
    {
        var eventTypeClr = @event.GetType();
        ThrowIfUnknownEventType(eventTypeClr);
        var eventType = _eventTypes.GetEventTypeFor(@event.GetType());
        var serializedEvent = await _eventSerializer.Serialize(@event);
        return new AppendEvent(eventSourceId, eventType, serializedEvent, validFrom);
    }

    void ThrowIfUnknownEventType(Type eventTypeClr)
    {
        if (!_eventTypes.HasFor(eventTypeClr))
        {
            throw new UnknownEventType(eventTypeClr);
        }
    }
}
