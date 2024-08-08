// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Aggregates;
using Cratis.Chronicle.Compliance;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.Events.Constraints;
using Cratis.Chronicle.Integration;
using Cratis.Chronicle.Projections;
using Cratis.Chronicle.Reactions;
using Cratis.Chronicle.Reducers;
using Cratis.Chronicle.Rules;
using Cratis.Reflection;
using Cratis.Types;

namespace Cratis.Chronicle;

/// <summary>
/// Represents a default implementation of <see cref="IClientArtifactsProvider"/>.
/// </summary>
/// <remarks>
/// This will use type discovery through the provided <see cref="ICanProvideAssembliesForDiscovery"/>.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="DefaultClientArtifactsProvider"/> class.
/// </remarks>
/// <param name="assembliesProvider"><see cref="ICanProvideAssembliesForDiscovery"/> for discovering types.</param>
public class DefaultClientArtifactsProvider(ICanProvideAssembliesForDiscovery assembliesProvider) : IClientArtifactsProvider
{
    bool _initialized;

    /// <inheritdoc/>
    public virtual IEnumerable<Type> EventTypes { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> Projections { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> Adapters { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> Reactions { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> Reducers { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> ReactionMiddlewares { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> ComplianceForTypesProviders { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> ComplianceForPropertiesProviders { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> Rules { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> AdditionalEventInformationProviders { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> AggregateRoots { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> AggregateRootStateTypes { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> ConstraintTypes { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> UniqueConstraints { get; private set; } = [];

    /// <inheritdoc/>
    public virtual IEnumerable<Type> UniqueEventTypeConstraints { get; private set; } = [];

    /// <inheritdoc/>
    public void Initialize()
    {
        if (_initialized) return;

        assembliesProvider.Initialize();
        EventTypes = assembliesProvider.DefinedTypes.Where(_ => _.HasAttribute<EventTypeAttribute>()).ToArray();
        ComplianceForTypesProviders = assembliesProvider.DefinedTypes.Where(_ => _ != typeof(ICanProvideComplianceMetadataForType) && _.IsAssignableTo(typeof(ICanProvideComplianceMetadataForType))).ToArray();
        ComplianceForPropertiesProviders = assembliesProvider.DefinedTypes.Where(_ => _ != typeof(ICanProvideComplianceMetadataForProperty) && _.IsAssignableTo(typeof(ICanProvideComplianceMetadataForProperty))).ToArray();
        Rules = assembliesProvider.DefinedTypes.Where(_ => _.BaseType?.IsGenericType == true && _.BaseType?.GetGenericTypeDefinition() == typeof(RulesFor<,>)).ToArray();
        Adapters = assembliesProvider.DefinedTypes.Where(_ => _.HasInterface(typeof(IAdapterFor<,>)) && !_.IsGenericType).ToArray();
        Projections = assembliesProvider.DefinedTypes.Where(_ => _.HasInterface(typeof(IProjectionFor<>))).ToArray();
        Reactions = assembliesProvider.DefinedTypes.Where(_ => _.HasInterface(typeof(IReaction)) && !_.IsGenericType).ToArray();
        ReactionMiddlewares = assembliesProvider.DefinedTypes.Where(_ => _.HasInterface(typeof(IReactionMiddleware))).ToArray();
        Reducers = assembliesProvider.DefinedTypes.Where(_ => _.HasInterface(typeof(IReducerFor<>)) && !_.IsGenericType).ToArray();
        AdditionalEventInformationProviders = assembliesProvider.DefinedTypes.Where(_ => _.HasInterface(typeof(ICanProvideAdditionalEventInformation))).ToArray();
        var aggregateRoots = AggregateRoots = assembliesProvider.DefinedTypes.Where(_ => _.HasInterface(typeof(IAggregateRoot))).ToArray();
        AggregateRootStateTypes = aggregateRoots
                                            .SelectMany(_ => _.AllBaseAndImplementingTypes())
                                            .Where(_ => _.IsDerivedFromOpenGeneric(typeof(AggregateRoot<>)))
                                            .Select(_ => _.GetGenericArguments()[0])
                                            .ToArray();

        ConstraintTypes = assembliesProvider.DefinedTypes.Where(_ => _ != typeof(IConstraint) && _.IsAssignableTo(typeof(IConstraint))).ToArray();
        UniqueConstraints = EventTypes.Where(_ => _.GetProperties().Any(p => p.HasAttribute<UniqueAttribute>())).ToArray();
        UniqueEventTypeConstraints = EventTypes.Where(_ => _.HasAttribute<UniqueAttribute>()).ToArray();

        _initialized = true;
    }
}
