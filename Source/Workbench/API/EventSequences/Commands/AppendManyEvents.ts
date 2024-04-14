/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from 'Infrastructure';

import { EventToAppend } from './EventToAppend';
import { EventType } from '../../EventTypes/EventType';
import { Causation } from '../../Auditing/Causation';
import { Identity } from '../../Identities/Identity';

export class AppendManyEvents {

    @field(String)
    eventSourceId!: string;

    @field(EventToAppend, true)
    events!: EventToAppend[];

    @field(Causation, true)
    causation!: Causation[];

    @field(Identity)
    causedBy!: Identity;
}
