import './App.css'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import Callback from './components/callback/Callback'
import Home from './components/home/Home'

function App() {
  const routes = createBrowserRouter([
    {
      path: '/',
      element: <Home />
    },
    {
      path: 'callback',
      element: <Callback />
    }
  ]);

  return (
    <RouterProvider router={routes} />
  );
}

export default App
