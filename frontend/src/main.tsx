import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { ChakraProvider } from '@chakra-ui/react'
import { createBrowserRouter, RouterProvider, } from "react-router-dom";
import App from './App.tsx'
import LoginBox from './login/LoginBox.tsx';
import SignUpBox from './login/SignUpBox.tsx';
import CenterNavigation from './components/CenterNavigation.tsx';
const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      {
        path: "/",
        element: <LoginBox />,
      },
      {
        path: "/main-navigation",
        element: <CenterNavigation />,
      },
      {
        path: "/signup",
        element: <SignUpBox />,
      }
    ],
  },
])
createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ChakraProvider>
      <RouterProvider router={router} />
    </ChakraProvider>
  </StrictMode>,
)
