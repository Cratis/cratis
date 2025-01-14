// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Concepts.Events;

namespace Cratis.Chronicle.Concepts.Observation;

/// <summary>
/// Represents the state of an attempt of a failed partition. This includes representing the initial attempt that caused it.
/// </summary>
public class FailedPartitionAttempt
{
    /// <summary>
    /// Gets the empty <see cref="FailedPartitionAttempt"/>.
    /// </summary>
    public static FailedPartitionAttempt NoAttempt => new();

    /// <summary>
    /// Gets or sets when the attempt occurred.
    /// </summary>
    public DateTimeOffset Occurred { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets or sets the <see cref="EventSequenceNumber"/> for the event that caused the failure.
    /// </summary>
    public EventSequenceNumber SequenceNumber { get; set; } = EventSequenceNumber.Unavailable;

    /// <summary>
    /// Gets or sets the Error Messages for the last error on this failed partition.
    /// </summary>
    public IEnumerable<string> Messages { get; set; } = [];

    /// <summary>
    /// Gets or sets the StackTrace for the last error on this failed partition.
    /// </summary>
    public string StackTrace { get; set; } = string.Empty;
}
