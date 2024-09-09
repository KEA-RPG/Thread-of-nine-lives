import { Center, Grid, GridItem } from "@chakra-ui/react"
import NavBar from "./components/NavBar"
import LoginBox from "./login/LoginBox"
import { Outlet } from "react-router-dom"

function App() {
  return <Grid
    templateAreas={`"header"
                    "main"`}
    gridTemplateRows={'50px 1fr 30px'}
    h='200px'
    gap='1'
    color='blackAlpha.700'
    fontWeight='bold'
  >
    <GridItem pl='2' bg='orange.300' area={'header'}>
      <NavBar />
    </GridItem>
    <GridItem pl='2' area={'main'} display="flex" justifyContent="center" alignItems="center">
      <main>
        <Outlet />
      </main>
      </GridItem>
  </Grid>
}

export default App
