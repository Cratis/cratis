// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Aksio.Cratis.Events.Store.Grains.Observation;

/// <summary>
/// Holds log messages for <see cref="Observer"/>.
/// </summary>
public static partial class ObserverLogMesssages
{
    [LoggerMessage(0, LogLevel.Information, "Subscribing observer {ObserverId} for microservice '{MicroserviceId}' on sequence '{EventSequenceId}' for tenant '{TenantId}'")]
    internal static partial void Subscribing(this ILogger logger, Guid observerId, Guid microserviceId, Guid eventSequenceId, Guid tenantId);

    [LoggerMessage(1, LogLevel.Information, "Unsubscribing observer {ObserverId} for microservice '{MicroserviceId}' on sequence '{EventSequenceId}' for tenant '{TenantId}'")]
    internal static partial void Unsubscribing(this ILogger logger, Guid observerId, Guid microserviceId, Guid eventSequenceId, Guid tenantId);

    [LoggerMessage(2, LogLevel.Information, "Rewinding observer {ObserverId} for microservice '{MicroserviceId}' on sequence '{EventSequenceId}' for tenant '{TenantId}'")]
    internal static partial void Rewinding(this ILogger logger, Guid observerId, Guid microserviceId, Guid eventSequenceId, Guid tenantId);

    [LoggerMessage(3, LogLevel.Information, "Catching up observer {ObserverId} up for microservice '{MicroserviceId}' on sequence '{EventSequenceId}' for tenant '{TenantId}'")]
    internal static partial void CatchingUp(this ILogger logger, Guid observerId, Guid microserviceId, Guid eventSequenceId, Guid tenantId);

    [LoggerMessage(4, LogLevel.Information, "Observer {ObserverId} is now active up for microservice '{MicroserviceId}' on sequence '{EventSequenceId}' for tenant '{TenantId}'")]
    internal static partial void Active(this ILogger logger, Guid observerId, Guid microserviceId, Guid eventSequenceId, Guid tenantId);

    [LoggerMessage(5, LogLevel.Information, "Observer {ObserverId} is connected for microservice '{MicroserviceId}' on sequence '{EventSequenceId}' for tenant '{TenantId}'")]
    internal static partial void Connected(this ILogger logger, Guid observerId, Guid microserviceId, Guid eventSequenceId, Guid tenantId);

    [LoggerMessage(6, LogLevel.Information, "Observer {ObserverId} is disconnected for microservice '{MicroserviceId}' on sequence '{EventSequenceId}' for tenant '{TenantId}'")]
    internal static partial void Disconnected(this ILogger logger, Guid observerId, Guid microserviceId, Guid eventSequenceId, Guid tenantId);
}
