import { createBrowserRouter } from "react-router-dom";
import App from './App.tsx';
import CenterNavigation from './components/CenterNavigation.tsx';
import ListLayout from './components/ListLayout.tsx';
import MainLayout from './layouts/MainLayout.tsx';
import CombatPage from "./pages/CombatPage.tsx";
import SelectionPage from "./pages/Selection.tsx"
import EnemyUpsert from './pages/EnemyUpsert.tsx';
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
            <ListLayout />
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
            <ListLayout />
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
            <ListLayout />
          </AdminLayout>
        ),
      },
      {
        path: "/admin/enemies/upsert",
        element: (
          <AdminLayout header="Enemy Upsert">
            <EnemyUpsert />
          </AdminLayout>
        ),
      },
      {
        path: "/admin/enemies/upsert/:enemyid",
        element: (
          <AdminLayout header="Enemy Upsert">
            <EnemyUpsert />
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
