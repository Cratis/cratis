// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Chronicle.Contracts.EventSequences;

/// <summary>
/// Represents the response for getting the tail sequence number.
/// </summary>
public class GetTailSequenceNumberResponse
{
    /// <summary>
    /// Gets or sets the sequence number.
    /// </summary>
    public ulong SequenceNumber { get; set; }
}
