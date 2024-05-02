// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from 'Infrastructure';

import { PropertyPath } from './PropertyPath';
import { IPropertyPathSegment } from './IPropertyPathSegment';

export class ArrayIndexer {

    @field(PropertyPath)
    arrayProperty!: PropertyPath;

    @field(PropertyPath)
    identifierProperty!: PropertyPath;

    @field(Object)
    identifier!: any;
}
