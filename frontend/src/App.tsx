import { Outlet } from "react-router-dom"
import { UserProvider } from "./components/UserContext"
import { AntiForgeryProvider } from "./components/AntiForgeryContext";

function App() {
  return (
    <UserProvider>
      <AntiForgeryProvider>
          <Outlet/>
      </AntiForgeryProvider>
    </UserProvider>
  )
}

export default App
