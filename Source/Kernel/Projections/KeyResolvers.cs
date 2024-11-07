// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using Cratis.Chronicle.Concepts.Events;
using Cratis.Chronicle.Concepts.Keys;
using Cratis.Chronicle.Dynamic;
using Cratis.Chronicle.Objects;
using Cratis.Chronicle.Properties;
using Cratis.Chronicle.Storage.EventSequences;

namespace Cratis.Chronicle.Projections;

/// <summary>
/// Represents utilities for creating <see cref="KeyResolvers"/> instances for providing values from <see cref="AppendedEvent">events</see>.
/// </summary>
public static class KeyResolvers
{
    /// <summary>
    /// Create a <see cref="KeyResolver"/> that provides the event source id from an event.
    /// </summary>
    /// <returns>A new <see cref="KeyResolver"/>.</returns>
    public static readonly KeyResolver FromEventSourceId = (IEventSequenceStorage eventSequenceStorage, AppendedEvent @event) => Task.FromResult(new Key(EventValueProviders.EventSourceId(@event), ArrayIndexers.NoIndexers))!;

    /// <summary>
    /// Create a <see cref="KeyResolver"/> that provides a value from the event content.
    /// </summary>
    /// <param name="eventValueProvider">The actual <see cref="ValueProvider{T}"/> for resolving key.</param>
    /// <returns>A new <see cref="KeyResolver"/>.</returns>
    public static KeyResolver FromEventValueProvider(ValueProvider<AppendedEvent> eventValueProvider)
    {
        return (IEventSequenceStorage eventSequenceStorage, AppendedEvent @event) =>
        {
            var key = eventValueProvider(@event);
            return Task.FromResult(new Key(key, ArrayIndexers.NoIndexers))!;
        };
    }

    /// <summary>
    /// Create a <see cref="KeyResolver"/> that provides a key value which is a composite of a set of <see cref="ValueProvider{T}"/>.
    /// </summary>
    /// <param name="propertiesWithKeyValueProviders">Target property paths in key and resolvers to use for resolving.</param>
    /// <returns>A new <see cref="KeyResolver"/>.</returns>
    public static KeyResolver Composite(IDictionary<PropertyPath, ValueProvider<AppendedEvent>> propertiesWithKeyValueProviders)
    {
        return (IEventSequenceStorage eventSequenceStorage, AppendedEvent @event) =>
        {
            var key = new ExpandoObject();
            foreach (var keyValue in propertiesWithKeyValueProviders)
            {
                var actualTarget = key.EnsurePath(keyValue.Key, ArrayIndexers.NoIndexers) as IDictionary<string, object>;
                actualTarget![keyValue.Key.LastSegment.Value] = keyValue.Value(@event);
            }
            return Task.FromResult(new Key(key, ArrayIndexers.NoIndexers));
        };
    }

    /// <summary>
    /// Create a <see cref="KeyResolver"/> that provides a key value for a join relationship.
    /// </summary>
    /// <param name="projection"><see cref="IProjection"/> the join is for.</param>
    /// <param name="keyResolver"><see cref="KeyResolver"/> for resolving the key from the event.</param>
    /// <param name="identifiedByProperty">The <see cref="PropertyPath"/> for the identified by property in the join relationship.</param>
    /// <returns><see cref="KeyResolver"/> that will be used to resolve.</returns>
    public static KeyResolver ForJoin(IProjection projection, KeyResolver keyResolver, PropertyPath identifiedByProperty)
    {
        return async (IEventSequenceStorage eventSequenceStorage, AppendedEvent @event) =>
        {
            var key = await keyResolver(eventSequenceStorage, @event);
            if (!projection.HasParent)
            {
                return key with { ArrayIndexers = ArrayIndexers.NoIndexers };
            }

            var arrayIndexers = new List<ArrayIndexer>
            {
                new(projection.ChildrenPropertyPath, identifiedByProperty, key.Value)
            };
            return key with { ArrayIndexers = new ArrayIndexers(arrayIndexers) };
        };
    }

    /// <summary>
    /// Create a <see cref="KeyResolver"/> that provides a key value hierarchically upwards in Child->Parent relationships.
    /// </summary>
    /// <param name="projection"><see cref="IProjection"/> to start at.</param>
    /// <param name="keyResolver">see cref=KeyResolver"/> to use for resolving the key for the incoming event.</param>
    /// <param name="parentKeyResolver">The property that represents the parent key.</param>
    /// <param name="identifiedByProperty">The property that identifies the key on the child object.</param>
    /// <returns>A new <see cref="KeyResolver"/>.</returns>
    public static KeyResolver FromParentHierarchy(IProjection projection, KeyResolver keyResolver, KeyResolver parentKeyResolver, PropertyPath identifiedByProperty)
    {
        return async (IEventSequenceStorage eventSequenceStorage, AppendedEvent @event) =>
        {
            var parentKey = await parentKeyResolver(eventSequenceStorage, @event);
            if (!projection.HasParent)
            {
                return parentKey with { ArrayIndexers = ArrayIndexers.NoIndexers };
            }
            var arrayIndexers = new List<ArrayIndexer>();

            var key = await keyResolver(eventSequenceStorage, @event);
            arrayIndexers.Add(new ArrayIndexer(projection.ChildrenPropertyPath, identifiedByProperty, key.Value));
            var parentProjection = projection.Parent!;
            var parentEventTypeIds = parentProjection.OwnEventTypes.Select(_ => _.Id).ToArray();
            if (parentEventTypeIds.Length == 0)
            {
                return parentKey with { ArrayIndexers = new ArrayIndexers(arrayIndexers) };
            }
            AppendedEvent parentEvent;
            if (parentEventTypeIds.Any(id => id == @event.Metadata.Type.Id))
            {
                parentEvent = @event;
            }
            else
            {
                parentEvent = await eventSequenceStorage.GetLastInstanceOfAny(parentKey.Value.ToString()!, parentEventTypeIds);
            }

            var eventType = parentProjection.EventTypes.First(eventType => eventType.Id == parentEvent.Metadata.Type.Id);
            var keyResolverForEventType = parentProjection.GetKeyResolverFor(eventType);
            var resolvedParentKey = await keyResolverForEventType(eventSequenceStorage, parentEvent);
            parentKey = resolvedParentKey;
            arrayIndexers.AddRange(resolvedParentKey.ArrayIndexers.All);

            return parentKey with { ArrayIndexers = new ArrayIndexers(arrayIndexers) };
        };
    }
}
