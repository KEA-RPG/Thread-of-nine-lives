import { Image, Button, Card, CardBody, CardFooter, Heading, Stack, Text, Link } from '@chakra-ui/react'
import { useState } from 'react'
import LoginHandler from './LoginHandler';
import InputFieldElement from '../components/InputFieldElement';

const LoginBox = () => {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null);

  const handleLogin = async () => {
    setError(null);
    try {
      await LoginHandler({ username, password });

    } catch (err) {
      setError('failed to login');
    }
  };

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
        <InputFieldElement type="text" name="Username" placeholder="Username" value={username} onChange={(e) => setUsername(e)} />
        <InputFieldElement type="password" name="Password" placeholder="Password" value={password} onChange={(e) => setPassword(e)} />
        <Button variant='solid' colorScheme='blue' mt={3} onClick={handleLogin}>
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