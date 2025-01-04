// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;

namespace Cratis.Chronicle.Events.Constraints.for_Constraints.given;

public class two_constraints : no_constraints
{
    protected static readonly ConstraintName _firstConstraintName = "FirstConstraint";
    protected static readonly ConstraintName _secondConstraintName = "SecondConstraint";
    protected UniqueConstraintDefinition _firstConstraint;
    protected UniqueEventTypeConstraintDefinition _secondConstraint;

    async Task Establish()
    {
        _firstConstraint = new(_firstConstraintName, _ => "FirstConstraintViolationMessage", [], null);
        _secondConstraint = new(_secondConstraintName, _ => "SecondConstraintViolationMessage", "", null);

        _constraintsProvider.Provide().Returns(new IConstraintDefinition[] { _firstConstraint, _secondConstraint }.ToImmutableList());
        await _constraints.Discover();
    }
}
