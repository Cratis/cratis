// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React from 'react';
import { IIdentityContext } from './IIdentityContext';
import { useState, useEffect } from 'react';

const defaultIdentityContext: IIdentityContext = {
    details: {}
};

export const IdentityProviderContext = React.createContext<IIdentityContext>(defaultIdentityContext);

const cookieName = '.aksio-identity';

export interface IdentityProviderProps {
    children?: JSX.Element | JSX.Element[]
}

function getCookie(name: string) {
    const decoded = decodeURIComponent(document.cookie);
    const cookies = decoded.split(';');
    const cookie = cookies.find(_ => _.trim().indexOf(`${name}=`) == 0);
    if (cookie) {
        const keyValue = cookie.split('=');
        return [keyValue[0].trim(), keyValue[1].trim()];
    }
    return [];
}

export const IdentityProvider = (props: IdentityProviderProps) => {
    const [context, setContext] = useState<IIdentityContext>(defaultIdentityContext);
    const identityCookie = getCookie(cookieName);
    if (identityCookie.length == 2) {
        const json = atob(identityCookie[1]);
        setContext({
            details: JSON.parse(json.toString())
        });
    } else {
        useEffect(() => {
            fetch('/.aksio/me').then(async response => {
                const json = await response.json();
                setContext({
                    details: json
                });
            });
        }, []);
    }

    return (
        <IdentityProviderContext.Provider value={context}>
            {props.children}
        </IdentityProviderContext.Provider>
    );
};
