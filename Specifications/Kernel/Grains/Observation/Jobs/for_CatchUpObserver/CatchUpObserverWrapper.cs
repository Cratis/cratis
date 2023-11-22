// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Aksio.Cratis.Kernel.Grains.Jobs;
using Aksio.Cratis.Kernel.Keys;

namespace Aksio.Cratis.Kernel.Grains.Observation.Jobs.for_CatchUpObserver;

public class CatchUpObserverWrapper : CatchUpObserver
{
    public CatchUpObserverWrapper(IObserverKeyIndexes observerKeyIndexes) : base(observerKeyIndexes) { }

    public Task<IImmutableList<JobStepDetails>> WrappedPrepareSteps() => PrepareSteps();
    public Task<bool> WrappedCanResume() => CanResume();
    public Task WrappedOnStepCompleted(JobStepId jobStepId, JobStepResult result) => OnStepCompleted(jobStepId, result);
}
