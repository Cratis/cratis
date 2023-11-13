// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Aggregates.for_AggregateRootStateProvider;

public class when_providing_an_aggregate_with_more_than_one_state_provider : given.an_aggregate_root_state_manager
{
    Exception result;

    void Establish()
    {
        reducers_registrar.Setup(_ => _.HasReducerFor(typeof(StateForAggregateRoot))).Returns(true);
        immediate_projections.Setup(_ => _.HasProjectionFor(typeof(StateForAggregateRoot))).Returns(true);
    }

    async Task Because() => result = await Catch.Exception(() => manager.Provide(aggregate_root, event_sequence.Object));

    [Fact] void should_throw_ambiguous_state_provider_exception() => result.ShouldBeOfExactType<AmbiguousAggregateRootStateProvider>();
}