// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Cratis.Net;
using Aksio.Tasks;
using Aksio.Timers;
using Cratis.Chronicle.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cratis.Chronicle.Connections;

/// <summary>
/// Represents a <see cref="IConnection"/> for a clustered Kernel.
/// </summary>
public abstract class ClusteredKernelClient : RestKernelConnection
{
    readonly IOptions<ClientOptions> _options;
    readonly ILoadBalancedHttpClientFactory _httpClientFactory;
    readonly IExecutionContextManager _executionContextManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClusteredKernelClient"/> class.
    /// </summary>
    /// <param name="options">The <see cref="ClientOptions"/>.</param>
    /// <param name="server">The ASP.NET Core server.</param>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> for getting service instances.</param>
    /// <param name="httpClientFactory">The <see cref="ILoadBalancedHttpClientFactory"/> to use.</param>
    /// <param name="taskFactory">A <see cref="ITaskFactory"/> for creating tasks.</param>
    /// <param name="timerFactory">A <see cref="ITimerFactory"/> for creating timers.</param>
    /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with the execution context.</param>
    /// <param name="connectionLifecycle"><see cref="IConnectionLifecycle"/> for communicating lifecycle events outside.</param>
    /// <param name="jsonSerializerOptions"><see cref="JsonSerializerOptions"/> for serialization.</param>
    /// <param name="logger"><see cref="ILogger"/> for logging.</param>
    protected ClusteredKernelClient(
        IOptions<ClientOptions> options,
        IServer server,
        IServiceProvider serviceProvider,
        ILoadBalancedHttpClientFactory httpClientFactory,
        ITaskFactory taskFactory,
        ITimerFactory timerFactory,
        IExecutionContextManager executionContextManager,
        IConnectionLifecycle connectionLifecycle,
        JsonSerializerOptions jsonSerializerOptions,
        ILogger<RestKernelConnection> logger) : base(
            options,
            server,
            serviceProvider,
            taskFactory,
            timerFactory,
            executionContextManager,
            connectionLifecycle,
            jsonSerializerOptions,
            logger)
    {
        _options = options;
        _httpClientFactory = httpClientFactory;
        _executionContextManager = executionContextManager;
    }

    /// <summary>
    /// Gets the endpoints to use for connecting to Kernel.
    /// </summary>
    protected abstract IEnumerable<Uri> Endpoints { get; }

    /// <inheritdoc/>
    protected override HttpClient CreateHttpClient()
    {
        var client = _httpClientFactory.Create(Endpoints);
        if (_executionContextManager.IsInContext)
        {
            var tenantId = _options.Value.IsMultiTenanted ? _executionContextManager.Current.TenantId : TenantId.NotSet;
            client.DefaultRequestHeaders.Add(ExecutionContextAppBuilderExtensions.TenantIdHeader, tenantId.ToString());
        }
        else
        {
            client.DefaultRequestHeaders.Add(ExecutionContextAppBuilderExtensions.TenantIdHeader, TenantId.NotSet.ToString());
        }
        return client;
    }
}