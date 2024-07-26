// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace Cratis.Chronicle.Aggregates.for_StatelessAggregateRootMutator.when_rehydrating;

public class and_aggregate_root_does_not_have_any_handle_methods : given.a_stateless_aggregate_root_mutator
{
    async Task Because() => await _mutator.Rehydrate();

    [Fact] void should_not_ger_any_events() => _eventSequence.ReceivedCalls().ShouldBeEmpty();
    [Fact] void should_not_handle_any_events() => _eventHandlers.DidNotReceive().Handle(Arg.Any<IAggregateRoot>(), Arg.Any<IEnumerable<EventAndContext>>());
}
