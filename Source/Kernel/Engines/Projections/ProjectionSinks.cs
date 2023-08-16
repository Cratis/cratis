// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using Aksio.Cratis.Projections;
using Aksio.Types;

namespace Aksio.Cratis.Kernel.Engines.Projections;

/// <summary>
/// Represents an implementation of <see cref="IProjectionSinkFactory"/>.
/// </summary>
[Singleton]
public class ProjectionSinks : IProjectionSinks
{
    sealed record Key(ProjectionSinkTypeId TypeId, ProjectionId ProjectionId);

    readonly IDictionary<ProjectionSinkTypeId, IProjectionSinkFactory> _factories;
    readonly ConcurrentDictionary<Key, IProjectionSink> _stores = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectionSinks"/> class.
    /// </summary>
    /// <param name="stores"><see cref="IInstancesOf{T}"/> of <see cref="IProjectionSinkFactory"/>.</param>
    public ProjectionSinks(IInstancesOf<IProjectionSinkFactory> stores)
    {
        _factories = stores.ToDictionary(_ => _.TypeId, _ => _);
    }

    /// <inheritdoc/>
    public IProjectionSink GetForTypeAndModel(ProjectionId projectionId, ProjectionSinkTypeId typeId, Model model)
    {
        ThrowIfUnknownProjectionResultStore(typeId);
        var key = new Key(typeId, projectionId);
        if (_stores.TryGetValue(key, out var store)) return store;
        return _stores[key] = _factories[typeId].CreateFor(model);
    }

    /// <inheritdoc/>
    public bool HasType(ProjectionSinkTypeId typeId) => _factories.ContainsKey(typeId);

    void ThrowIfUnknownProjectionResultStore(ProjectionSinkTypeId typeId)
    {
        if (!HasType(typeId)) throw new UnknownProjectionSink(typeId);
    }
}
