// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Contracts.Events.Constraints;
using UniqueConstraintDefinitionContract = Cratis.Chronicle.Contracts.Events.Constraints.UniqueConstraintDefinition;

namespace Cratis.Chronicle.Events.Constraints.for_UniqueConstraintDefinition;

public class when_converting_to_contract : Specification
{
    static ConstraintName _constraintName = "My Constraint";
    UniqueConstraintDefinition _definition;
    EventType _firstEventType;
    Constraint _contract;
    EventType _secondEventType;
    UniqueConstraintDefinitionContract _definitionContract;

    void Establish()
    {
        _firstEventType = new EventType("First Event Type", EventGeneration.First);
        _secondEventType = new EventType("Second Event Type", EventGeneration.First);
        _definition = new UniqueConstraintDefinition(
            _constraintName,
            _ => "",
            [
                new(_firstEventType, null!, "First Property"),
                new(_secondEventType, null!, "Second Property")
            ]);
    }

    void Because()
    {
        _contract = _definition.ToContract();
        _definitionContract = _contract.Definition.Value as UniqueConstraintDefinitionContract;
    }

    [Fact] void should_have_correct_name() => _contract.Name.ShouldEqual(_constraintName.Value);
    [Fact] void should_have_correct_type() => _contract.Type.ShouldEqual(ConstraintType.Unique);
    [Fact] void should_have_first_event_type() => _definitionContract.EventDefinitions[0].EventType.Id.ShouldEqual(_firstEventType.Id.Value);
    [Fact] void should_have_first_event_property() => _definitionContract.EventDefinitions[0].Property.ShouldEqual(_definition.EventsWithProperties.First().Property);
    [Fact] void should_have_second_event_type() => _definitionContract.EventDefinitions[1].EventType.Id.ShouldEqual(_secondEventType.Id.Value);
    [Fact] void should_have_second_event_property() => _definitionContract.EventDefinitions[1].Property.ShouldEqual(_definition.EventsWithProperties.Last().Property);
}
