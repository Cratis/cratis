// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Cratis.Chronicle.Concepts.Events;
using Cratis.Chronicle.Concepts.Jobs;
using Cratis.Chronicle.Grains.Jobs;
namespace Cratis.Chronicle.Grains.Observation.Jobs;

/// <summary>
/// Represents a job for retrying a failed partition.
/// </summary>
public class CatchUpObserverPartition : Job<CatchUpObserverPartitionRequest, JobStateWithLastHandledEvent>, ICatchUpObserverPartition
{
    /// <inheritdoc/>
    public override async Task OnCompleted()
    {
        if (AllStepsCompletedSuccessfully)
        {
            var observer = GrainFactory.GetGrain<IObserver>(Request.ObserverId, Request.ObserverKey);
            await observer.PartitionCaughtUp(Request.Key, State.LastHandledEventSequenceNumber);
        }
    }

    /// <inheritdoc/>
    protected override Task OnStepCompleted(JobStepId jobStepId, JobStepResult result)
    {
        State.HandleResult(result);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task<IImmutableList<JobStepDetails>> PrepareSteps(CatchUpObserverPartitionRequest request)
    {
        var steps = new[]
        {
            CreateStep<IHandleEventsForPartition>(
                new HandleEventsForPartitionArguments(
                    request.ObserverKey,
                    request.ObserverSubscription,
                    request.Key,
                    request.FromSequenceNumber,
                    EventSequenceNumber.Max,
                    EventObservationState.Replay,
                    request.EventTypes))
        }.ToImmutableList();

        return Task.FromResult<IImmutableList<JobStepDetails>>(steps);
    }
}