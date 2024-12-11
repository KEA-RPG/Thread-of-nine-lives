import { Outlet } from "react-router-dom"
import { UserProvider } from "./components/UserContext"

function App() {
  return (
    <UserProvider>
          <Outlet/>
    </UserProvider>
  )
}

export default App
