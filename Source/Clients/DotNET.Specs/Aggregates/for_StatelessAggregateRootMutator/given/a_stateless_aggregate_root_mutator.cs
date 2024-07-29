// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Chronicle.EventSequences;

namespace Cratis.Chronicle.Aggregates.for_StatelessAggregateRootMutator.given;

public class a_stateless_aggregate_root_mutator : Specification
{
    protected StatelessAggregateRoot _aggregateRoot;

    protected IEventStore _eventStore;
    protected EventSourceId _eventSourceId;
    protected IAggregateRootContext _aggregateRootContext;
    protected IAggregateRootMutator _mutator;
    protected IEventSerializer _eventSerializer;
    protected IAggregateRootEventHandlers _eventHandlers;
    protected IEventSequence _eventSequence;

    void Establish()
    {
        _aggregateRoot = new StatelessAggregateRoot();
        _eventStore = Substitute.For<IEventStore>();
        _eventSourceId = EventSourceId.New();
        _eventSerializer = Substitute.For<IEventSerializer>();
        _eventSequence = Substitute.For<IEventSequence>();
        _aggregateRootContext = Substitute.For<IAggregateRootContext>();
        _aggregateRootContext.EventSourceId.Returns(_eventSourceId);
        _aggregateRootContext.AggregateRoot.Returns(_aggregateRoot);
        _aggregateRootContext.EventSequence.Returns(_eventSequence);
        _eventSerializer = Substitute.For<IEventSerializer>();
        _eventHandlers = Substitute.For<IAggregateRootEventHandlers>();

        _mutator = new StatelessAggregateRootMutator(
            _aggregateRootContext,
            _eventStore,
            _eventSerializer,
            _eventHandlers);
    }
}
