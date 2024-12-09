import { createBrowserRouter } from "react-router-dom";
import App from './App.tsx';
import CenterNavigation from './components/CenterNavigation.tsx';
import MainLayout from './layouts/MainLayout.tsx';
import CombatPage from "./pages/CombatPage.tsx";
import SelectionPage from "./pages/Selection.tsx"
import Login from './pages/Login.tsx';
import Logout from "./pages/Logout.tsx";
import SignUp from "./pages/SignUp.tsx";
import GuestLayout from "./layouts/GuestLayout.tsx";
import PlayerLayout from "./layouts/PlayerLayout.tsx";
import AdminLayout from "./layouts/Adminlayout.tsx";
import CardUpdate from "./pages/CardUpdate.tsx";
import CardCreate from "./pages/CardCreate.tsx";
import DeckCreate from "./pages/DeckCreate.tsx";
import DeckUpdate from "./pages/DeckUpdate.tsx";
import PublicDecksPage from "./pages/PublicDecksPage.tsx";
import AdminEnemyList from "./pages/AdminEnemyList.tsx";
import EnemyUpdate from "./pages/EnemyUpdate.tsx";
import EnemyCreate from "./pages/EnemyCreate.tsx";
import AdminCardList from "./pages/AdminCardList.tsx";
import DeckList from "./pages/DeckList.tsx";

const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      {
        path: "/",
        element: (
          <PlayerLayout header="Log in">
            <CenterNavigation />
          </PlayerLayout>
        ),
      },
      {
        path: "/login",
        element: (
          <GuestLayout header="Log in">
            <Login />
          </GuestLayout>
        ),
      },
      {
        path: "/logout",
        element: (
          <GuestLayout header="Log in">
            <Logout />
          </GuestLayout>
        ),
      },
      {
        path: "/signup",
        element: (
          <GuestLayout header="Sign up">
            <SignUp />
          </GuestLayout>
        ),
      },
      {
        path: "/menu",
        element: (
          <PlayerLayout header="Main Menu">
            <CenterNavigation />
          </PlayerLayout>
        ),
      },
      {
        path: "/decks",
        element: (
          <PlayerLayout header="Decks">
            <DeckList />
          </PlayerLayout>
        ),
      },
      {
        path: "/decks/create",
        element: (
          <PlayerLayout header="Deck Manager">
            <DeckCreate />
          </PlayerLayout>
        ),
      },
      {
        path: "/decks/:deckId",
        element: (
          <PlayerLayout header="Deck Manager">
            <DeckUpdate />
          </PlayerLayout>
        ),
      },
      {
        path: "/admin/cards",
        element: (
          <AdminLayout header="Admin Cards">
            <AdminCardList />
          </AdminLayout>
        ),
      },
      {
        path: "/admin/cards/:cardId",
        element: (
          <AdminLayout header="Card Manager">
            <CardUpdate />
          </AdminLayout>
        ),
      },
      {
        path: "/admin/cards/create",
        element: (
          <AdminLayout header="Card Manager">
            <CardCreate />
          </AdminLayout>
        ),
      },
      {
        path: "/admin/enemies",
        element: (
          <AdminLayout header="Admin Enemies">
            <AdminEnemyList />
          </AdminLayout>
        ),
      },
      {
        path: "/admin/enemies/create",
        element: (
          <AdminLayout header="Enemy create">
            <EnemyCreate />
          </AdminLayout>
        ),
      },
      {
        path: "/admin/enemies/:enemyId",
        element: (
          <AdminLayout header="Enemy update">
            <EnemyUpdate />
          </AdminLayout>
        ),
      },
      {
        path: "/decks/public",
        element: (
          <GuestLayout header="Public Decks">
            <PublicDecksPage />
          </GuestLayout>
        ),
      },

      {
        path: "/selection",
        element: (
          <MainLayout header="Select Enemy">
            <SelectionPage />
          </MainLayout>
        ),
      },
      {
        path: "/combat/:fightId",
        element: (
          <MainLayout header="Combat">
            <CombatPage />
          </MainLayout>
        ),
      },
    ],
  },
]);

export default router;
