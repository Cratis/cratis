// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Kernel.Storage.Sinks;
using Aksio.Cratis.Projections;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Aksio.Cratis.Kernel.Storage.MongoDB.Sinks;

/// <summary>
/// Represents an implementation of <see cref="ISink"/> for MongoDB.
/// </summary>
public class SinkCollections : ISinkCollections
{
    readonly Model _model;
    readonly ISinkDatabaseProvider _databaseProvider;
    bool _isReplaying;

    /// <summary>
    /// Initializes a new instance of the <see cref="SinkCollections"/> class.
    /// </summary>
    /// <param name="model">The <see cref="Model"/> the context is for.</param>
    /// <param name="databaseProvider">The <see cref="ISinkDatabaseProvider"/>.</param>
    public SinkCollections(
        Model model,
        ISinkDatabaseProvider databaseProvider)
    {
        _model = model;
        _databaseProvider = databaseProvider;
    }

    IMongoDatabase Database => _databaseProvider.GetDatabase();
    string ReplayCollectionName => $"replay-{_model.Name}";

    /// <inheritdoc/>
    public async Task BeginReplay()
    {
        _isReplaying = true;
        await PrepareInitialRun();
    }

    /// <inheritdoc/>
    public async Task EndReplay()
    {
        var rewindName = ReplayCollectionName;
        var rewoundCollectionsPrefix = $"{_model.Name}-";
        var collectionNames = (await Database.ListCollectionNamesAsync()).ToList();
        var nextCollectionSequenceNumber = 1;
        var rewoundCollectionNames = collectionNames.Where(_ => _.StartsWith(rewoundCollectionsPrefix, StringComparison.InvariantCulture)).ToArray();
        if (rewoundCollectionNames.Length > 0)
        {
            nextCollectionSequenceNumber = rewoundCollectionNames
                .Select(_ =>
                {
                    var postfix = _.Substring(rewoundCollectionsPrefix.Length);
                    if (int.TryParse(postfix, out var value))
                    {
                        return value;
                    }
                    return -1;
                })
                .Where(_ => _ >= 0)
                .OrderByDescending(_ => _)
                .First() + 1;
        }
        var oldCollectionName = $"{rewoundCollectionsPrefix}{nextCollectionSequenceNumber}";

        if (collectionNames.Contains(_model.Name))
        {
            await Database.RenameCollectionAsync(_model.Name, oldCollectionName);
        }

        if (collectionNames.Contains(rewindName))
        {
            await Database.RenameCollectionAsync(rewindName, _model.Name);
        }

        _isReplaying = false;
    }

    /// <inheritdoc/>
    public async Task PrepareInitialRun()
    {
        var collection = GetCollection();
        await collection.DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);
    }

    /// <inheritdoc/>
    public IMongoCollection<BsonDocument> GetCollection() => _isReplaying ? Database.GetCollection<BsonDocument>(ReplayCollectionName) : Database.GetCollection<BsonDocument>(_model.Name);
}