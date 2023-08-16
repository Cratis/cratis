// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using Aksio.Cratis.Changes;
using Aksio.Cratis.Events;
using Aksio.Cratis.Projections;

namespace Aksio.Cratis.Kernel.Engines.Projections;

/// <summary>
/// Represents an implementation of <see cref="IProjectionSink"/> that does nothing.
/// </summary>
public class NullProjectionSink : IProjectionSink
{
    /// <inheritdoc/>
    public ProjectionSinkTypeId TypeId => ProjectionSinkTypeId.Null;

    /// <inheritdoc/>
    public ProjectionSinkTypeName Name => "Null sink";

    /// <inheritdoc/>
    public Task ApplyChanges(Key key, IChangeset<AppendedEvent, ExpandoObject> changeset, bool isReplaying) => Task.CompletedTask;

    /// <inheritdoc/>
    public Task BeginReplay() => Task.CompletedTask;

    /// <inheritdoc/>
    public Task EndReplay() => Task.CompletedTask;

    /// <inheritdoc/>
    public Task<ExpandoObject?> FindOrDefault(Key key, bool isReplaying) => Task.FromResult<ExpandoObject?>(null);

    /// <inheritdoc/>
    public Task PrepareInitialRun(bool isReplaying) => Task.CompletedTask;
}
