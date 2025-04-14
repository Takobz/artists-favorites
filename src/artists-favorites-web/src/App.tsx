import './App.css'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import Callback from './components/callback/Callback'
import Home from './components/home/Home'
import { AuthContextProvider } from './contexts/AuthContextProvider';
import MakePlaylist from './components/playlist/ReviewMakePlaylist';

function App() {
  const routes = createBrowserRouter([
    {
      path: '/',
      element: <Home />
    },
    {
      path: 'callback',
      element: <Callback />
    },
    {
      path: 'playlist/create',
      element: <MakePlaylist />
    }
  ]);

  return (
      <AuthContextProvider>
        <RouterProvider router={routes} />
      </AuthContextProvider>
  );
}

export default App
