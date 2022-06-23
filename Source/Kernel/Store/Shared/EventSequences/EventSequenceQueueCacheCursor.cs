// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Aksio.Cratis.Extensions.Orleans.Execution;
using Orleans.Streams;

namespace Aksio.Cratis.Events.Store.EventSequences;

/// <summary>
/// Represents an implementation of <see cref="IQueueCacheCursor"/> for event log regular cache scenario.
/// </summary>
public class EventSequenceQueueCacheCursor : IQueueCacheCursor
{
    readonly IEventCursor _actualCursor;
    readonly IStreamIdentity _streamIdentity;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSequenceQueueCacheCursor"/> class.
    /// </summary>
    /// <param name="actualCursor">The actual <see cref="IEventCursor"/>.</param>
    /// <param name="streamIdentity"><see cref="IStreamIdentity"/> for the stream.</param>
    public EventSequenceQueueCacheCursor(
        IEventCursor actualCursor,
        IStreamIdentity streamIdentity)
    {
        _actualCursor = actualCursor;
        _streamIdentity = streamIdentity;
    }

    /// <inheritdoc/>
    public IBatchContainer GetCurrent(out Exception exception)
    {
        exception = null!;
        var microserviceAndTenant = (MicroserviceAndTenant)_streamIdentity.Namespace;
        return new EventSequenceBatchContainer(
            _actualCursor.Current,
            _streamIdentity.Guid,
            microserviceAndTenant.MicroserviceId,
            microserviceAndTenant.TenantId,
            new Dictionary<string, object> { { RequestContextKeys.TenantId, _streamIdentity.Namespace } });
    }

    /// <inheritdoc/>
    public bool MoveNext() => _actualCursor.MoveNext().GetAwaiter().GetResult();

    /// <inheritdoc/>
    public void RecordDeliveryFailure()
    {
    }

    /// <inheritdoc/>
    public void Refresh(StreamSequenceToken token)
    {
    }

    /// <inheritdoc/>
    public void Dispose()
    {
    }
}
