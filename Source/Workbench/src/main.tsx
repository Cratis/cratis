import 'primereact/resources/themes/lara-light-indigo/theme.css';
import { PrimeReactProvider } from 'primereact/api';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import React from 'react';
import './index.css';

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <PrimeReactProvider>
            <App />
        </PrimeReactProvider>
    </React.StrictMode>
);
