// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from 'Infrastructure';

import { EventTypeRegistration } from '../Cratis/Kernel/Contracts/Events/EventTypeRegistration';
import { EventType } from '../Cratis/Kernel/Contracts/Events/EventType';

export class RegisterEventTypes {

    @field(EventTypeRegistration, true)
    types!: EventTypeRegistration[];
}
