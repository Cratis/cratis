// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Projections;
using Cratis.Chronicle.EventSequences;

namespace Aksio.Cratis.Kernel.Domain.Projections;

/// <summary>
/// Represents the payload for performing an immediate projection.
/// </summary>
/// <param name="ProjectionId">The unique identifier of the projection.</param>
/// <param name="EventSequenceId">The event sequence to project from.</param>
/// <param name="ModelKey">The key of the model to project.</param>
public record ImmediateProjection(ProjectionId ProjectionId, EventSequenceId EventSequenceId, ModelKey ModelKey);
