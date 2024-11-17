import { createBrowserRouter } from "react-router-dom";
import App from './App.tsx';
import CenterNavigation from './components/CenterNavigation.tsx';
import DeckManager from './pages/DeckManager.tsx';
import ListLayout from './components/ListLayout.tsx';
import MainLayout from './layouts/MainLayout.tsx';
import CombatPage from "./pages/Combat.tsx";
import SelectionPage from "./pages/Selection.tsx"
import EnemyUpsert from './pages/EnemyUpsert.tsx';
import Login from './pages/Login.tsx';
import Logout from "./pages/Logout.tsx";
import SignUp from "./pages/SignUp.tsx";
import GuestLayout from "./layouts/GuestLayout.tsx";
import PlayerLayout from "./layouts/PlayerLayout.tsx";
import AdminLayout from "./layouts/Adminlayout.tsx";
import CardUpsert from "./pages/CardUpsert.tsx";
import CardUpdate from "./pages/CardUpdate.tsx";
import CardCreate from "./pages/CardCreate.tsx";

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
        path: "/decks/:deckId",
        element: (
          <PlayerLayout header="Deck Manager">
            <DeckManager />
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
        path: "/selection",
        element: (
          <MainLayout header="Select Enemy and Player">
            <SelectionPage />
          </MainLayout>
        ),
      },
      {
        path: "/combat",
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
