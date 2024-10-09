import { createBrowserRouter } from "react-router-dom";
import App from './App.tsx';
import LoginBox from './login/LoginBox.tsx';
import SignUpBox from './login/SignUpBox.tsx';
import CenterNavigation from './components/CenterNavigation.tsx';
import DeckManager from './components/DeckManager.tsx';
import ListLayout from './components/ListLayout.tsx';
import EnemyUpsert from './components/EnemyUpsert.tsx';
import MainLayout from './layouts/MainLayout.tsx'; // Import your MainLayout

const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      {
        path: "/",
        element: (
          <MainLayout header="Log in">
            <LoginBox />
          </MainLayout>
        ),
      },
      {
        path: "/login",
        element: (
          <MainLayout header="Log in">
            <LoginBox />
          </MainLayout>
        ),
      },
      {
        path: "/signup",
        element: (
          <MainLayout header="Sign up">
            <SignUpBox />
          </MainLayout>
        ),
      },
      {
        path: "/menu",
        element: (
          <MainLayout header="Main Menu">
            <CenterNavigation />
          </MainLayout>
        ),
      },
      {
        path: "/decks",
        element: (
          <MainLayout header="Decks">
            <ListLayout />
          </MainLayout>
        ),
      },
      {
        path: "/decks/:deckId",
        element: (
          <MainLayout header="Deck Manager">
            <DeckManager />
          </MainLayout>
        ),
      },
      {
        path: "/admin/cards",
        element: (
          <MainLayout header="Admin Cards">
            <ListLayout />
          </MainLayout>
        ),
      },
      {
        path: "/admin/cards/:cardId",
        element: (
          <MainLayout header="Card Manager">
            <CenterNavigation />
          </MainLayout>
        ),
      },
      {
        path: "/admin/enemies",
        element: (
          <MainLayout header="Admin Enemies">
            <ListLayout />
          </MainLayout>
        ),
      },
      {
        path: "/admin/enemies/upsert",
        element: (
          <MainLayout header="Enemy Upsert">
            <EnemyUpsert />
          </MainLayout>
        ),
      },
      {
        path: "/admin/enemies/upsert/:enemyid",
        element: (
          <MainLayout header="Enemy Upsert">
            <EnemyUpsert />
          </MainLayout>
        ),
      },

    ],
  },
]);

export default router;
