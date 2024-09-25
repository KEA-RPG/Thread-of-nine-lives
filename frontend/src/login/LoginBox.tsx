import { Image, Button, Card, CardBody, CardFooter, Heading, Stack, Text, Link } from '@chakra-ui/react'
import UsernameInput from '../components/UsernameInput'
import PasswordInput from '../components/PasswordInput'
const LoginBox = () => {
  return <Card
    direction={{ base: 'column', sm: 'row' }}
    overflow='hidden'
    variant='elevated'>

    <Image
      objectFit='cover'
      maxW={{ base: '100%', sm: '200px' }}
      src='https://loremflickr.com/1280/720'
    />

    <Stack>
      <CardBody>
        <Heading size='md'>Log in</Heading>
      </CardBody>
      <CardBody>
        <UsernameInput/>
        <PasswordInput/>
        <Button variant='solid' colorScheme='blue' mt={3}>
          Sign in
        </Button>
      </CardBody>
      <CardFooter>
        <Text>Or sign up <Link href='/signup'>here</Link></Text>
      </CardFooter>
    </Stack>
  </Card>
}

export default LoginBox