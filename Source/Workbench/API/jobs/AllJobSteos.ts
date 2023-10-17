/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { JobStepState } from './JobStepState';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/jobs/{{microserviceId}}/{{jobId}}/steps');

export interface AllJobSteosArguments {
    jobId: string;
    microserviceId: string;
}
export class AllJobSteos extends ObservableQueryFor<JobStepState[], AllJobSteosArguments> {
    readonly route: string = '/api/jobs/{{microserviceId}}/{{jobId}}/steps';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: JobStepState[] = [];

    constructor() {
        super(JobStepState, true);
    }

    get requestArguments(): string[] {
        return [
            'jobId',
            'microserviceId',
        ];
    }

    static use(args?: AllJobSteosArguments): [QueryResultWithState<JobStepState[]>] {
        return useObservableQuery<JobStepState[], AllJobSteos, AllJobSteosArguments>(AllJobSteos, args);
    }
}
