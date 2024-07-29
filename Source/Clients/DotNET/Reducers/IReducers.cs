// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Chronicle.Reducers;

/// <summary>
/// Defines a system for working with reducer registrations for the Kernel.
/// </summary>
public interface IReducers
{
    /// <summary>
    /// Discover all reducers from the entry assembly and dependencies.
    /// </summary>
    /// <returns>Awaitable task.</returns>
    Task Discover();

    /// <summary>
    /// Register all reducers with Chronicle.
    /// </summary>
    /// <returns>Awaitable task.</returns>
    Task Register();

    /// <summary>
    /// Check if there is a reducer for a specific model type.
    /// </summary>
    /// <param name="modelType">Model type to check for.</param>
    /// <returns>True if it has, false if not.</returns>
    bool HasReducerFor(Type modelType);

    /// <summary>
    /// Get all registered handlers.
    /// </summary>
    /// <returns>Collection of <see cref="IReducerHandler"/>.</returns>
    IEnumerable<IReducerHandler> GetAll();

    /// <summary>
    /// Get all registered reducers by its identifier.
    /// </summary>
    /// <param name="reducerId">The identifier of the reducer to get.</param>
    /// <returns><see cref="IReducerHandler"/> instance.</returns>
    IReducerHandler GetById(ReducerId reducerId);

    /// <summary>
    /// Get a specific handler for a specific model type.
    /// </summary>
    /// <param name="modelType">Model type to get for.</param>
    /// <returns><see cref="IReducerHandler"/> instance.</returns>
    IReducerHandler GetForModelType(Type modelType);

    /// <summary>
    /// Gets a specific handler by its id.
    /// </summary>
    /// <param name="reducerType">The reducer type to get for.</param>
    /// <returns><see cref="IReducerHandler"/>.</returns>
    IReducerHandler GetByType(Type reducerType);

    /// <summary>
    /// Get the CLR type for a specific reducer.
    /// </summary>
    /// <param name="reducerId"><see cref="ReducerId"/> to get for.</param>
    /// <returns>The type.</returns>
    Type GetClrType(ReducerId reducerId);
}
