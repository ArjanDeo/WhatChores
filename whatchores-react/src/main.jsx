import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import Navbar from './components/ui/navbar.jsx'
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import ErrorPage from './error-page.jsx'
import About from './routes/about.jsx';
import CharacterLookup from './routes/characterlookup.jsx'
import Character from './routes/character.jsx'

const router = createBrowserRouter([
  {
    path: "/",
    element: <Navbar/>,
    errorElement: <ErrorPage />, 
    children: [
      {
        path: "About",
        element: <About />,
       
      },
      {
        path: "/",
        element: <App />
      },
      {
        path: "CharacterLookup",
        element: <CharacterLookup />
      },
      {
        path: "Character/:realm/:name",
        element: <Character />
      },
    ],
  },
]);

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router} />    
  </React.StrictMode>,
)
