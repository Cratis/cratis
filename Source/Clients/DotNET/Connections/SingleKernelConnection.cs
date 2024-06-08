// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Tasks;
using Aksio.Timers;
using Cratis.Chronicle.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cratis.Chronicle.Connections;

/// <summary>
/// Represents an implementation of <see cref="IConnection"/> for a single instance.
/// </summary>
public class SingleKernelConnection : RestKernelConnection
{
    readonly IHttpClientFactory _httpClientFactory;
    readonly IExecutionContextManager _executionContextManager;
    readonly IOptions<ClientOptions> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleKernelConnection"/> class.
    /// </summary>
    /// <param name="options">The <see cref="ClientOptions"/>.</param>
    /// <param name="server">The ASP.NET Core server.</param>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> for getting service instances.</param>
    /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/> to use.</param>
    /// <param name="taskFactory">A <see cref="ITaskFactory"/> for creating tasks.</param>
    /// <param name="timerFactory">A <see cref="ITimerFactory"/> for creating timers.</param>
    /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with the execution context.</param>
    /// <param name="connectionLifecycle"><see cref="IConnectionLifecycle"/> for communicating lifecycle events outside.</param>
    /// <param name="jsonSerializerOptions"><see cref="JsonSerializerOptions"/> for serialization.</param>
    /// <param name="logger"><see cref="ILogger"/> for logging.</param>
    public SingleKernelConnection(
        IOptions<ClientOptions> options,
        IServer server,
        IServiceProvider serviceProvider,
        IHttpClientFactory httpClientFactory,
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
        _httpClientFactory = httpClientFactory;
        _executionContextManager = executionContextManager;
        _options = options;
    }

    /// <inheritdoc/>
    protected override HttpClient CreateHttpClient()
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = _options.Value.Kernel.SingleKernel?.Endpoint;
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