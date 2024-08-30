// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace Cratis.Chronicle.Integration.Orleans.InProcess.Projections.Scenarios.Models;

public record User(
    EventSourceId Id,
    string Name,
    IEnumerable<GroupOnUser> Groups);