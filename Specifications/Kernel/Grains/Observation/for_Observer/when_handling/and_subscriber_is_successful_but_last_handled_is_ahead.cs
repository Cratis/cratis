// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Kernel.Grains.Observation.for_Observer.when_handling;

public class and_subscriber_is_successful_but_last_handled_is_ahead : given.an_observer_with_subscription_for_specific_event_type
{
    void Establish()
    {
        subscriber.Setup(_ => _.OnNext(IsAny<IEnumerable<AppendedEvent>>(), IsAny<ObserverSubscriberContext>())).Returns(Task.FromResult(ObserverSubscriberResult.Ok(42UL)));
        state = state with { LastHandledEventSequenceNumber = 44UL };
    }

    async Task Because() => await observer.Handle("Something", new[] { AppendedEvent.EmptyWithEventTypeAndEventSequenceNumber(event_type, 42UL) });

    [Fact] void should_write_state_once() => written_states.Count.ShouldEqual(1);
    [Fact] void should_set_next_sequence_number() => written_states[0].NextEventSequenceNumber.ShouldEqual((EventSequenceNumber)43UL);
    [Fact] void should_not_modify_last_handled_event_sequence_number() => written_states[0].LastHandledEventSequenceNumber.ShouldEqual((EventSequenceNumber)44UL);
}