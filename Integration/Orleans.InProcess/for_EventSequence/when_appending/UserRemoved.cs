// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace Cratis.Chronicle.Integration.Orleans.InProcess.for_EventSequence.when_appending;

[EventType]
public record UserRemoved();