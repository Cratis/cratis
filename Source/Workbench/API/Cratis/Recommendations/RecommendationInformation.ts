// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from 'Infrastructure';


export class RecommendationInformation {

    @field(String)
    id!: string;

    @field(String)
    name!: string;

    @field(String)
    description!: string;

    @field(String)
    type!: string;

    @field(Date)
    occurred!: Date;
}
