/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { Projection } from './Projection';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/events/store/projections?microserviceId={{microserviceId}}');

export interface AllProjectionsArguments {
    microserviceId: string;
}
export class AllProjections extends QueryFor<Projection[], AllProjectionsArguments> {
    readonly route: string = '/api/events/store/projections?microserviceId={{microserviceId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Projection[] = [];

    constructor() {
        super(Projection, true);
    }

    get requestArguments(): string[] {
        return [
            'microserviceId',
        ];
    }

    static use(args?: AllProjectionsArguments): [QueryResultWithState<Projection[]>, PerformQuery<AllProjectionsArguments>] {
        return useQuery<Projection[], AllProjections, AllProjectionsArguments>(AllProjections, args);
    }
}
