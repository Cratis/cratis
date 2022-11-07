// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Events.Projections.Expressions.EventValues.for_EventSourceIdExpressionResolver;

public class when_trying_to_resolve_valid_event_source_id_expression : given.an_appended_event
{
    EventSourceIdExpressionResolver resolver;
    object result;

    void Establish() => resolver = new();

    void Because() => result = resolver.Resolve("$eventSourceId")(@event);

    [Fact] void should_resolve_to_a_value_provider_that_gets_event_source_id() => result.ShouldEqual(@event.Context.EventSourceId.Value);
}
