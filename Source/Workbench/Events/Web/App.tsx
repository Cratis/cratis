// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { BrowserRouter as Router, Route } from 'react-router-dom';
import { Navigation } from './Navigation';
import { EventTypes } from './Types/EventTypes';

import { default as styles } from './App.module.scss';
import { EventLogs } from './Exploring/EventLogs';
import { Projections } from './Programmatic/Projections';

export const App = () => {
    return (
        <Router>
            <div className={styles.appContainer}>
                <div className={styles.navigationBar}>
                    <Navigation />
                </div>
                <div style={{ width: '100%' }}>
                    <Route exact path="/">
                        Home
                    </Route>
                    <Route path="/events/types">
                        <EventTypes />
                    </Route>

                    <Route path="/events/eventlogs">
                        <EventLogs />
                    </Route>

                    <Route path="/programmatic/projections">
                        <Projections/>
                    </Route>
                </div>
            </div>
        </Router>
    );
};

