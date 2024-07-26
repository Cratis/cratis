// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Chronicle.EventSequences;

namespace Cratis.Chronicle.Aggregates.for_AggregateRootMutatorFactory;

public class when_creating_for_stateful_aggregate_root : given.an_aggregate_root_mutator_factory
{
    AggregateRootContext _context;
    IAggregateRootMutator _result;

    void Establish() => _context = new AggregateRootContext(
        CorrelationId.New(),
        EventSourceId.New(),
        Substitute.For<IEventSequence>(),
        new StatefulAggregateRoot(),
        true);

    async Task Because() => _result = await _factory.Create<StatefulAggregateRoot>(_context);

    [Fact] void should_return_a_stateful_mutator() => _result.ShouldBeOfExactType<StatefulAggregateRootMutator<StateForAggregateRoot>>();
}
