import { Grid, GridItem } from "@chakra-ui/react"
import Header from "./components/Header"
import { Outlet } from "react-router-dom"
import { UserProvider } from "./components/UserContext"

function App() {
  return (
    <UserProvider>
      <Grid
        templateAreas={`"header" "main"`}
        h='200px'
        gap='1'
        color='blackAlpha.700'
        fontWeight='bold'
      >
        <GridItem pl='2' bg='orange.300' area={'header'}>
          <Header />
        </GridItem>
        <GridItem pl='2' area={'main'} display="flex" justifyContent="center" alignItems="center">
          <Outlet />
        </GridItem>
      </Grid>
    </UserProvider>
  )
}

export default App
