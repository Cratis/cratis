// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Concepts.Events.Constraints;

namespace Cratis.Chronicle.Grains.Events.Constraints.when_registering;

public class and_existing_constraints_are_the_same : given.a_constraints_system
{
    IConstraintDefinition _existingFirstConstraint;
    IConstraintDefinition _existingSecondConstraint;
    IConstraintDefinition _firstConstraint;
    IConstraintDefinition _secondConstraint;

    void Establish()
    {
        _existingFirstConstraint = Substitute.For<IConstraintDefinition>();
        _existingFirstConstraint.Name.Returns<ConstraintName>("First");
        _existingSecondConstraint = Substitute.For<IConstraintDefinition>();
        _existingSecondConstraint.Name.Returns<ConstraintName>("Second");

        _firstConstraint = Substitute.For<IConstraintDefinition>();
        _firstConstraint.Name.Returns<ConstraintName>("First");
        _secondConstraint = Substitute.For<IConstraintDefinition>();
        _secondConstraint.Name.Returns<ConstraintName>("Second");

        _existingFirstConstraint.Equals(_firstConstraint).Returns(true);
        _existingSecondConstraint.Equals(_secondConstraint).Returns(true);
        _firstConstraint.Equals(_existingFirstConstraint).Returns(true);
        _secondConstraint.Equals(_existingSecondConstraint).Returns(true);

        _stateStorage.State.Constraints.Add(_existingFirstConstraint);
        _stateStorage.State.Constraints.Add(_existingSecondConstraint);
    }

    async Task Because() => await _constraints.Register([_firstConstraint, _secondConstraint]);

    [Fact] void should_only_have_two_constraints() => _stateStorage.State.Constraints.Count.ShouldEqual(2);
    [Fact] void should_not_write_state() => _storageStats.Writes.ShouldEqual(0);
    [Fact] void should_not_broadcast_constraints_changed() => _broadcastChannelWriter.DidNotReceive().Publish(Arg.Any<ConstraintsChanged>());
}
