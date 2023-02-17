// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Kernel.Grains.Observation.for_ObserverSupervisor.for_FailedPartitionSupervisor.when_failing_a_partition;

public class and_the_partition_is_not_already_failed : given.a_supervisor
{
    FailedPartitionSupervisor supervisor;
    EventSourceId partition_id;
    EventSequenceNumber sequence_number;
    DateTimeOffset occurred;
    Mock<IRecoverFailedPartition> failed_partition_mock;

    Task Establish()
    {
        partition_id = Guid.NewGuid();
        sequence_number = 1;
        occurred = DateTimeOffset.UtcNow;
        supervisor = get_clean_supervisor();
        var partition_key = get_partitioned_observer_key(partition_id);
        failed_partition_mock = a_failed_partition_mock(partition_key.EventSourceId);

        grain_factory_mock.Setup(
            _ => _.GetGrain<IRecoverFailedPartition>(
                observer_id,
                partition_key,
                null)).Returns(failed_partition_mock.Object);

        return Task.CompletedTask;
    }

    Task Because() => supervisor.Fail(partition_id, sequence_number, Enumerable.Empty<string>(), string.Empty, occurred);

    [Fact] void should_have_failed_partitions() => supervisor.HasFailedPartitions.ShouldBeTrue();
    [Fact] void should_have_the_failed_partition() => supervisor.GetState().FailedPartitions.Any(_ => _.Partition == partition_id).ShouldBeTrue();
    [Fact] void should_have_the_failed_partition_with_the_correct_sequence_number() => supervisor.GetState().FailedPartitions.First(_ => _.Partition == partition_id).Tail.ShouldEqual(sequence_number);
    [Fact] void should_initiate_the_failed_partition() => failed_partition_mock.Verify(_ => _.Recover(sequence_number, event_types, IsAny<ObserverKey>()), Once());
}
