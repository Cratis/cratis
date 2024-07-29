// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Connections;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.Observation.Reducers;

namespace Cratis.Chronicle.Services.Observation.Reducers;

/// <summary>
/// Extension methods for converting between <see cref="Contracts.Observation.Reducers.EventTypeWithKeyExpression"/> and <see cref="EventTypeWithKeyExpression"/>.
/// </summary>
public static class EventTypeWithKeyExpressionConverters
{
    /// <summary>
    /// Convert from <see cref="Contracts.Observation.Reducers.EventTypeWithKeyExpression"/> to <see cref="EventTypeWithKeyExpression"/>.
    /// </summary>
    /// <param name="eventTypeWithKeyExpression"><see cref="Contracts.Observation.Reducers.EventTypeWithKeyExpression"/> to convert from.</param>
    /// <returns>Converted <see cref="EventTypeWithKeyExpression"/>.</returns>
    public static EventTypeWithKeyExpression ToChronicle(this Contracts.Observation.Reducers.EventTypeWithKeyExpression eventTypeWithKeyExpression) =>
        new(eventTypeWithKeyExpression.EventType.ToChronicle(), eventTypeWithKeyExpression.Key);
}
