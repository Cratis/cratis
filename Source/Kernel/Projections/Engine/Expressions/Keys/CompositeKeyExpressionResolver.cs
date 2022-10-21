// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;
using Aksio.Cratis.Events.Projections.Expressions.EventValues;
using Aksio.Cratis.Properties;

namespace Aksio.Cratis.Events.Projections.Expressions.Keys;

/// <summary>
/// Represents an implementation of <see cref="IKeyExpressionResolver"/> for composite key expressions.
/// </summary>
public class CompositeKeyExpressionResolver : IKeyExpressionResolver
{
    static readonly Regex _regularExpression = new("\\$composite\\((?<expressions>.*?)\\)", RegexOptions.Compiled | RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(1));

    readonly IEventValueProviderExpressionResolvers _resolvers;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeKeyExpressionResolver"/> class.
    /// </summary>
    /// <param name="resolvers"><see cref="IEventValueProviderExpressionResolvers"/> for resolving event values.</param>
    public CompositeKeyExpressionResolver(IEventValueProviderExpressionResolvers resolvers) => _resolvers = resolvers;

    /// <inheritdoc/>
    public bool CanResolve(string expression) => _regularExpression.Match(expression).Success;

    /// <inheritdoc/>
    public KeyResolver Resolve(IProjection projection, string expression, PropertyPath identifiedByProperty)
    {
        var match = _regularExpression.Match(expression);
        var properties = match.Groups["expressions"].Value;
        var expressions = properties.Split(',').Select(_ => _.Trim());
        var propertiesWithKeyValueProviders = expressions.Select(_ =>
        {
            var keyValue = _.Split('=');
            return new
            {
                Property = new PropertyPath(keyValue[0]),
                KeyResolver = _resolvers.Resolve(keyValue[1])
            };
        }).ToDictionary(_ => _.Property, _ => _.KeyResolver);

        return KeyResolvers.Composite(propertiesWithKeyValueProviders);
    }
}
