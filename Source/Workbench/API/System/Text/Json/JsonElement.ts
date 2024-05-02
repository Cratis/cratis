// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from 'Infrastructure';

import { JsonValueKind } from './JsonValueKind';
import { JsonElement } from './JsonElement';

export class JsonElement {

    @field(JsonValueKind)
    valueKind!: JsonValueKind;

    @field(JsonElement)
    item!: JsonElement;
}
