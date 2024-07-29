// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Connections;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.Grains.Clients;
using Cratis.Chronicle.Observation;
using Microsoft.Extensions.Logging;
using Orleans.Placement;

namespace Cratis.Chronicle.Grains.Observation.Reactions.Clients;

/// <summary>
/// Represents an implementation of <see cref="IReaction"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Reaction"/> class.
/// </remarks>
/// <param name="localSiloDetails"><see cref="ILocalSiloDetails"/> for getting information about the silo this grain is on.</param>
/// <param name="logger"><see cref="ILogger"/> for logging.</param>
[PreferLocalPlacement]
public class Reaction(
    ILocalSiloDetails localSiloDetails,
    ILogger<Reaction> logger) : Grain, IReaction, INotifyClientDisconnected
{
    IConnectedClients? _connectedClients;
    ConnectedObserverKey? _observerKey;

    /// <inheritdoc/>
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _connectedClients = GrainFactory.GetGrain<IConnectedClients>(0);
        await _connectedClients.SubscribeDisconnected(this.AsReference<INotifyClientDisconnected>());

        _observerKey = ConnectedObserverKey.Parse(this.GetPrimaryKeyString());
    }

    /// <inheritdoc/>
    public async Task Start(IEnumerable<EventType> eventTypes)
    {
        logger.Starting(_observerKey!.EventStore, _observerKey.ObserverId!, _observerKey!.EventSequenceId, _observerKey!.Namespace);
        var key = new ObserverKey(_observerKey.ObserverId, _observerKey.EventStore, _observerKey.Namespace, _observerKey.EventSequenceId);
        var observer = GrainFactory.GetGrain<IObserver>(key);
        await _connectedClients!.SubscribeDisconnected(this.AsReference<INotifyClientDisconnected>());
        var connectedClient = await _connectedClients!.GetConnectedClient(_observerKey.ConnectionId!);
        await observer.Subscribe<IReactionObserverSubscriber>(ObserverType.Client, eventTypes, localSiloDetails.SiloAddress, connectedClient);
    }

    /// <inheritdoc/>
    public void OnClientDisconnected(ConnectedClient client)
    {
        if (client.ConnectionId != _observerKey!.ConnectionId) return;

        logger.ClientDisconnected(client.ConnectionId, _observerKey!.EventStore, _observerKey.ObserverId!, _observerKey!.EventSequenceId, _observerKey!.Namespace);
        var key = new ObserverKey(_observerKey.ObserverId, _observerKey.EventStore, _observerKey.Namespace, _observerKey.EventSequenceId);
        var observer = GrainFactory.GetGrain<IObserver>(key);
        observer.Unsubscribe();
        _connectedClients!.UnsubscribeDisconnected(this.AsReference<INotifyClientDisconnected>()).Wait();
        DeactivateOnIdle();
    }
}
