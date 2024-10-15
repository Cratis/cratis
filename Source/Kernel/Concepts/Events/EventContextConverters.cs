// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Concepts.Auditing;
using Cratis.Chronicle.Concepts.Identities;

namespace Cratis.Chronicle.Concepts.Events;

/// <summary>
/// Converter methods for <see cref="EventContext"/>.
/// </summary>
public static class EventContextConverters
{
    /// <summary>
    /// Convert to contract version of <see cref="EventContext"/>.
    /// </summary>
    /// <param name="context"><see cref="EventContext"/> to convert.</param>
    /// <returns>Converted <see cref="Contracts.Events.EventContext"/>.</returns>
    public static Contracts.Events.EventContext ToContract(this EventContext context) => new()
    {
        EventSource = context.EventSource,
        EventSourceId = context.EventSourceId,
        EventStreamType = context.EventStreamType,
        EventStreamId = context.EventStreamId,
        SequenceNumber = context.SequenceNumber,
        Occurred = context.Occurred!,
        EventStore = context.EventStore,
        Namespace = context.Namespace,
        CorrelationId = context.CorrelationId,
        Causation = context.Causation.Select(_ => _.ToContract()).ToList(),
        CausedBy = context.CausedBy.ToContract(),
        ObservationState = context.ObservationState.ToContract()
    };

    /// <summary>
    /// Convert to Chronicle version of <see cref="EventContext"/>.
    /// </summary>
    /// <param name="context"><see cref="Contracts.Events.EventContext"/> to convert.</param>
    /// <returns>Converted <see cref="EventContext"/>.</returns>
    public static EventContext ToChronicle(this Contracts.Events.EventContext context) => new(
        context.EventSource,
        context.EventSourceId,
        context.EventStreamType,
        context.EventStreamId,
        context.SequenceNumber,
        context.Occurred,
        context.EventStore,
        context.Namespace,
        context.CorrelationId,
        context.Causation.Select(_ => _.ToChronicle()),
        context.CausedBy.ToChronicle(),
        context.ObservationState.ToChronicle());
}
