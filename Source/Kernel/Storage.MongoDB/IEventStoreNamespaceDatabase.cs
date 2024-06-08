// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Kernel.Storage.Observation;
using Cratis.Chronicle.EventSequences;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Aksio.Cratis.Kernel.Storage.MongoDB;

/// <summary>
/// Defines the database for accessing a specific namespace of the event store.
/// </summary>
public interface IEventStoreNamespaceDatabase
{
    /// <summary>
    /// Get a collection - optionally by its name. If no name is given, it will go by convention from the type name.
    /// </summary>
    /// <param name="name">Optional name of the collection.</param>
    /// <typeparam name="T">Type to get collection for.</typeparam>
    /// <returns>A <see cref="IMongoCollection{T}"/> instance.</returns>
    IMongoCollection<T> GetCollection<T>(string? name = default);

    /// <summary>
    /// Get the <see cref="IMongoCollection{T}"/> for an event sequence based on identifier.
    /// </summary>
    /// <param name="eventSequenceId"><see cref="EventSequenceId"/> identifier.</param>
    /// <returns>The collection instance.</returns>
    IMongoCollection<Event> GetEventSequenceCollectionFor(EventSequenceId eventSequenceId);

    /// <summary>
    /// Get the <see cref="IMongoCollection{T}"/> for an event sequence based on identifier as <see cref="BsonDocument"/>.
    /// </summary>
    /// <param name="eventSequenceId"><see cref="EventSequenceId"/> identifier.</param>
    /// <returns>The collection instance.</returns>
    IMongoCollection<BsonDocument> GetEventSequenceCollectionAsBsonFor(EventSequenceId eventSequenceId);

    /// <summary>
    /// Get the <see cref="IMongoCollection{T}"/> for the observer state.
    /// </summary>
    /// <returns>The collection instance.</returns>
    IMongoCollection<ObserverState> GetObserverStateCollection();
}
