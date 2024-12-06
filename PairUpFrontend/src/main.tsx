import {QueryClient, QueryClientProvider } from "react-query";
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import './index.css'

const queryClient = new QueryClient();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
      <QueryClientProvider client={queryClient}>
          <App />
      </QueryClientProvider>
  </StrictMode>
)
