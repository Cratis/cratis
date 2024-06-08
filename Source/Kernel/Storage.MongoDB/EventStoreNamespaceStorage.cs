// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Text.Json;
using Aksio.Cratis.Kernel.Storage.Changes;
using Aksio.Cratis.Kernel.Storage.EventSequences;
using Aksio.Cratis.Kernel.Storage.EventTypes;
using Aksio.Cratis.Kernel.Storage.Jobs;
using Aksio.Cratis.Kernel.Storage.Keys;
using Aksio.Cratis.Kernel.Storage.MongoDB.EventSequences;
using Aksio.Cratis.Kernel.Storage.MongoDB.Jobs;
using Aksio.Cratis.Kernel.Storage.MongoDB.Keys;
using Aksio.Cratis.Kernel.Storage.MongoDB.Observation;
using Aksio.Cratis.Kernel.Storage.MongoDB.Projections;
using Aksio.Cratis.Kernel.Storage.MongoDB.Recommendations;
using Aksio.Cratis.Kernel.Storage.Observation;
using Aksio.Cratis.Kernel.Storage.Recommendations;
using Cratis.Chronicle.EventSequences;
using Microsoft.Extensions.Logging;

namespace Aksio.Cratis.Kernel.Storage.MongoDB;

/// <summary>
/// Represents an implementation of <see cref="IEventStoreNamespaceStorage"/> for MongoDB.
/// </summary>
public class EventStoreNamespaceStorage : IEventStoreNamespaceStorage
{
    readonly EventStoreName _eventStore;
    readonly EventStoreNamespaceName _namespace;
    readonly IEventStoreNamespaceDatabase _eventStoreNamespaceDatabase;
    readonly IEventConverter _converter;
    readonly IEventTypesStorage _eventTypesStorage;
    readonly Json.ExpandoObjectConverter _expandoObjectConverter;
    readonly JsonSerializerOptions _jsonSerializerOptions;
    readonly IExecutionContextManager _executionContextManager;
    readonly ILoggerFactory _loggerFactory;
    readonly ConcurrentDictionary<EventSequenceId, IEventSequenceStorage> _eventSequences = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStoreNamespaceStorage"/> class.
    /// </summary>
    /// <param name="eventStore"><see cref="EventStoreName"/> the storage is for.</param>
    /// <param name="namespace"><see cref="TenantId"/> the storage is for.</param>
    /// <param name="eventStoreNamespaceDatabase">Provider for <see cref="IEventStoreNamespaceDatabase"/> to use.</param>
    /// <param name="converter"><see cref="IEventConverter"/> to convert event types.</param>
    /// <param name="eventTypesStorage">The <see cref="IEventTypesStorage"/> for working with the schema types.</param>
    /// <param name="expandoObjectConverter"><see cref="IExpandoObjectConverter"/> for converting between expando object and json objects.</param>
    /// <param name="jsonSerializerOptions">The global <see cref="JsonSerializerOptions"/>.</param>
    /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for getting the execution context.</param>
    /// <param name="loggerFactory"><see cref="ILoggerFactory"/> for creating loggers.</param>
    public EventStoreNamespaceStorage(
        EventStoreName eventStore,
        EventStoreNamespaceName @namespace,
        IEventStoreNamespaceDatabase eventStoreNamespaceDatabase,
        IEventConverter converter,
        IEventTypesStorage eventTypesStorage,
        Json.ExpandoObjectConverter expandoObjectConverter,
        JsonSerializerOptions jsonSerializerOptions,
        IExecutionContextManager executionContextManager,
        ILoggerFactory loggerFactory)
    {
        _eventStore = eventStore;
        _namespace = @namespace;
        _eventStoreNamespaceDatabase = eventStoreNamespaceDatabase;
        _converter = converter;
        _eventTypesStorage = eventTypesStorage;
        _expandoObjectConverter = expandoObjectConverter;
        _jsonSerializerOptions = jsonSerializerOptions;
        _executionContextManager = executionContextManager;
        _loggerFactory = loggerFactory;
        Changesets = new ChangesetStorage();
        Jobs = new JobStorage(eventStoreNamespaceDatabase);
        JobSteps = new JobStepStorage(eventStoreNamespaceDatabase);
        Observers = new ObserverStorage(eventStoreNamespaceDatabase);
        FailedPartitions = new FailedPartitionStorage(eventStoreNamespaceDatabase);
        Recommendations = new RecommendationStorage(eventStoreNamespaceDatabase);
        ObserverKeyIndexes = new ObserverKeyIndexes(eventStoreNamespaceDatabase, Observers);
    }

    /// <inheritdoc/>
    public IChangesetStorage Changesets { get; }

    /// <inheritdoc/>
    public IJobStorage Jobs { get; }

    /// <inheritdoc/>
    public IJobStepStorage JobSteps { get; }

    /// <inheritdoc/>
    public IObserverStorage Observers { get; }

    /// <inheritdoc/>
    public IFailedPartitionsStorage FailedPartitions { get; }

    /// <inheritdoc/>
    public IRecommendationStorage Recommendations { get; }

    /// <inheritdoc/>
    public IObserverKeyIndexes ObserverKeyIndexes { get; }

    /// <inheritdoc/>
    public IEventSequenceStorage GetEventSequence(EventSequenceId eventSequenceId)
    {
        if (_eventSequences.TryGetValue(eventSequenceId, out var eventSequenceStorage))
        {
            return eventSequenceStorage;
        }

        eventSequenceStorage = new EventSequenceStorage(
            _eventStore,
            _namespace,
            eventSequenceId,
            _eventStoreNamespaceDatabase,
            _converter,
            _eventTypesStorage,
            _expandoObjectConverter,
            _jsonSerializerOptions,
            _executionContextManager,
            _loggerFactory.CreateLogger<EventSequenceStorage>());
        _eventSequences[eventSequenceId] = eventSequenceStorage;

        return eventSequenceStorage;
    }
}
