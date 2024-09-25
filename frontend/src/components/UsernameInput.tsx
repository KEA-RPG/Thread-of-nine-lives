import { FormControl, FormLabel, Input } from '@chakra-ui/react'

const UsernameInput = () =>
    <FormControl>
        <FormLabel>Username</FormLabel>
        <Input type="text" placeholder="Username" />
    </FormControl>



export default UsernameInput;