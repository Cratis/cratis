// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Events.Projections.Expressions.EventValues;

namespace Aksio.Cratis.Events.Projections.Expressions.for_ModelPropertyExpressionResolvers.given;

public class model_property_expression_resolvers : Specification
{
    protected ModelPropertyExpressionResolvers resolvers;
    protected Mock<IEventValueProviderExpressionResolvers> event_value_resolvers;

    void Establish()
    {
        event_value_resolvers = new();
        resolvers = new ModelPropertyExpressionResolvers(event_value_resolvers.Object);
    }
}
