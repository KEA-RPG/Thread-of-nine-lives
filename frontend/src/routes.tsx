import { createBrowserRouter } from "react-router-dom";
import App from './App.tsx'
import LoginBox from './login/LoginBox.tsx';
import SignUpBox from './login/SignUpBox.tsx';
import CenterNavigation from './components/CenterNavigation.tsx';
import DeckManager from './components/DeckManager.tsx';
import ListLayout from "./components/ListLayout.tsx";

const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      {
        path: "/",
        element: <LoginBox />,
      },
      {
        path: "/login",
        element: <LoginBox />,
      },
      {
        path: "/signup",
        element: <SignUpBox />,
      },
      {
        path: "/menu",
        element: <CenterNavigation />,
      },
      {
        path: "/decks",
        element: <ListLayout />,
      },
      {
        path: "/decks/:deckId",
        element: <DeckManager />,
      },
      {
        path: "/admin/",
        element: <CenterNavigation />,
      },
      {
        path: "/admin/cards",
        element: <ListLayout />,
      },
      {
        path: "/admin/cards/:cardId",
        element: <CenterNavigation />,
      },
      {
        path: "/admin/enemies",
        element: <ListLayout />,
      },
      {
        path: "/admin/enemies:enemyId",
        element: <CenterNavigation />,
      },
    ],
  },
]);
export default router;
