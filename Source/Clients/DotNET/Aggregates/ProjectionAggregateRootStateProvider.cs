// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Projections;

namespace Cratis.Chronicle.Aggregates;

/// <summary>
/// Represents an implementation of <see cref="IAggregateRootStateProvider{TState}"/> using a projection.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProjectionAggregateRootStateProvider{TState}"/> class.
/// </remarks>
/// <typeparam name="TState">Type of state for the aggregate root.</typeparam>
/// <param name="aggregateRootContext">The <see cref="AggregateRoot"/> the state is for.</param>
/// <param name="projections"><see cref="IProjections"/> to use for getting state.</param>
public class ProjectionAggregateRootStateProvider<TState>(
    IAggregateRootContext aggregateRootContext,
    IProjections projections) : IAggregateRootStateProvider<TState>
    where TState : class
{
    /// <inheritdoc/>
    public async Task<TState?> Provide()
    {
        var result = await projections.GetInstanceByIdForSession(
            aggregateRootContext.CorrelationId,
            typeof(TState),
            aggregateRootContext.EventSourceId);
        return result.Model as TState;
    }

    /// <inheritdoc/>
    public async Task<TState?> Update(TState? initialState, IEnumerable<object> events)
    {
        var result = await projections.GetInstanceByIdForSessionWithEventsApplied(
            aggregateRootContext.CorrelationId,
            typeof(TState),
            aggregateRootContext.EventSourceId,
            events);
        return result.Model as TState;
    }

    /// <inheritdoc/>
    public Task Dehydrate() =>
        projections.DehydrateSession(
            aggregateRootContext.CorrelationId,
            typeof(TState),
            aggregateRootContext.EventSourceId);
}
