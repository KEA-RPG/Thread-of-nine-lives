import { FormControl, FormLabel, Input } from '@chakra-ui/react'

const PasswordInput = () =>
    <FormControl>
        <FormLabel>Password</FormLabel>
        <Input type="password" placeholder="Password" />
    </FormControl>

export default PasswordInput;