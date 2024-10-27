import { createBrowserRouter } from "react-router-dom";
import App from './App.tsx';
import CenterNavigation from './components/CenterNavigation.tsx';
import DeckManager from './pages/DeckManager.tsx';
import ListLayout from './components/ListLayout.tsx';
import EnemyUpsert from './pages/EnemyUpsert.tsx';
import MainLayout from './layouts/MainLayout.tsx'; 
import Login from './pages/Login.tsx';
import Logout from "./pages/Logout.tsx";
import SignUp from "./pages/SignUp.tsx";

const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      {
        path: "/",
        element: (
          <MainLayout header="Log in">
            <CenterNavigation/>
          </MainLayout>
        ),
      },
      {
        path: "/login",
        element: (
          <MainLayout header="Log in">
            <Login/>
          </MainLayout>
        ),
      },      
      {
        path: "/logout",
        element: (
          <MainLayout header="Log in">
            <Logout/>
          </MainLayout>
        ),
      },
      {
        path: "/signup",
        element: (
          <MainLayout header="Sign up">
            <SignUp/>
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
