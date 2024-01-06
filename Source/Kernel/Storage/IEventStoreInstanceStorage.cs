// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.EventSequences;
using Aksio.Cratis.Kernel.Storage.Changes;
using Aksio.Cratis.Kernel.Storage.EventSequences;
using Aksio.Cratis.Kernel.Storage.Jobs;
using Aksio.Cratis.Kernel.Storage.Observation;
using Aksio.Cratis.Kernel.Storage.Recommendations;

namespace Aksio.Cratis.Kernel.Storage;

/// <summary>
/// Defines the storage for a specific instance of an event store.
/// </summary>
public interface IEventStoreInstanceStorage
{
    /// <summary>
    /// Gets the <see cref="IEventSequenceStorage"/> for the event store.
    /// </summary>
    IChangesetStorage Changesets { get; }

    /// <summary>
    /// Gets the <see cref="IJobStorage"/> for the event store.
    /// </summary>
    IJobStorage Jobs { get; }

    /// <summary>
    /// Gets the <see cref="IJobStepStorage"/> for the event store.
    /// </summary>
    IJobStepStorage JobSteps { get; }

    /// <summary>
    /// Gets the <see cref="IObserverStorage"/> for the event store.
    /// </summary>
    IObserverStorage Observers { get; }

    /// <summary>
    /// Gets the <see cref="IEventSequenceStorage"/> for the event store.
    /// </summary>
    IFailedPartitionsStorage FailedPartitions { get; }

    /// <summary>
    /// Gets the <see cref="IRecommendationStorage"/> for the event store.
    /// </summary>
    IRecommendationStorage Recommendations { get; }

    /// <summary>
    /// Get a <see cref="IJobStorage{TJobState}"/> for a specific <see cref="JobState"/> type.
    /// </summary>
    /// <typeparam name="TJobState">Type of <see cref="JobState"/> to get for.</typeparam>
    /// <returns>An instance of the <see cref="IJobStorage{TJobState}"/> for the given type.</returns>
    IJobStorage<TJobState> GetJobStorage<TJobState>()
        where TJobState : JobState;

    /// <summary>
    /// Get a <see cref="IJobStepStorage{TJobStepState}"/> for a specific <see cref="JobStepState"/> type.
    /// </summary>
    /// <typeparam name="TJobStepState">Type of <see cref="JobStepState"/> to get for.</typeparam>
    /// <returns>An instance of the <see cref="IJobStorage{TJobStepState}"/> for the given type.</returns>
    IJobStepStorage<TJobStepState> GetJobStepStorage<TJobStepState>()
       where TJobStepState : JobStepState;

    /// <summary>
    /// Get the <see cref="IEventSequenceStorage"/> for a specific <see cref="EventSequenceId"/>.
    /// </summary>
    /// <param name="eventSequenceId"><see cref="EventSequenceId"/> to get for.</param>
    /// <returns>The <see cref="IEventStoreInstanceStorage"/> instance.</returns>
    IEventSequenceStorage GetEventSequence(EventSequenceId eventSequenceId);
}
