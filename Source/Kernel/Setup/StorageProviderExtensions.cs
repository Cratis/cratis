// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Grains;
using Cratis.Chronicle.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Storage;
using Polly;

namespace Orleans.Hosting;

/// <summary>
/// Defines extensions for <see cref="ISiloBuilder"/> for configuring storage providers.
/// </summary>
public static class StorageProviderExtensions
{
    /// <summary>
    /// Add storage providers to the silo.
    /// </summary>
    /// <param name="builder">The <see cref="ISiloBuilder"/> to add to.</param>
    /// <returns><see cref="ISiloBuilder"/> for continuation.</returns>
    public static ISiloBuilder AddStorageProviders(this ISiloBuilder builder)
    {
        // Based on https://devblogs.microsoft.com/dotnet/building-resilient-cloud-services-with-dotnet-8/#custom-resilience-pipeline-and-dynamic-reloads
        builder.Services.Configure<ResilientStorageOptions>(
            ResilientGrainStorage.ResiliencePipelineKey,
            builder.Configuration.GetSection(ConfigurationPath.Combine("Chronicle", "ResilientStorage")));
        builder.Services.AddResiliencePipeline(ResilientGrainStorage.ResiliencePipelineKey, static (builder, context) =>
        {
            context.EnableReloads<ResilientStorageOptions>();
            var options = context.GetOptions<ResilientStorageOptions>();
            builder.AddRetry(options.Retry);
            builder.AddTimeout(options.Timeout);
        });

        builder.ConfigureServices(services =>
        {
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.Namespaces, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Namespaces.NamespacesStateStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.EventSequences, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.EventSequences.EventSequencesStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.Observers, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Observation.ObserverGrainStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.FailedPartitions, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Observation.FailedPartitionGrainStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.Jobs, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Jobs.JobGrainStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.JobSteps, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Jobs.JobStepGrainStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.Recommendations, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Recommendations.RecommendationGrainStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.Projections, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Projections.ProjectionDefinitionStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.ProjectionsManager, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Projections.ProjectionsManagerStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.Reactors, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Observation.Reactors.Clients.ReactorDefinitionStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.Reducers, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Observation.Reducers.Clients.ReducerDefinitionStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.ReducersManager, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Observation.Reducers.Clients.ReducersManagerStorageProvider>());
            services.AddKeyedSingleton<IGrainStorage>(WellKnownGrainStorageProviders.Constraints, (serviceProvider, _) => serviceProvider.GetRequiredService<Cratis.Chronicle.Grains.Events.Constraints.ConstraintsStorageProvider>());
        });

        return builder;
    }
}
