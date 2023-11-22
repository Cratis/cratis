// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Kernel.Grains.Observation.Jobs.for_CatchUpObserver.when_asking_if_can_resume;

public class and_it_is_not_subscribed : given.a_catchup_observer_and_an_unsubscribed_request
{
    Mock<IObserver> observer;
    bool result;

    void Establish()
    {
        observer = new();
        grain_factory.Setup(_ => _.GetGrain<IObserver>(state.Request.ObserverId, state.Request.ObserverKey, null)).Returns(observer.Object);
        observer.Setup(_ => _.IsSubscribed()).ReturnsAsync(false);
    }

    async Task Because() => result = await job.WrappedCanResume();

    [Fact] void should_not_be_able_to_resume() => result.ShouldBeFalse();
}
