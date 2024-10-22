import { Image, Button, Card, CardBody, CardFooter, Heading, Stack, Text, Link } from '@chakra-ui/react';
import { useState } from 'react';
import InputFieldElement from '../components/InputFieldElement';
import { jwtDecode } from 'jwt-decode';
import useLogin from '../hooks/useUser';  // Make sure this hook is correctly implemented

export interface LoginCredentials {
  username: string;
  passwordHash: string;//Skal hedde noget andet når api er opdateret
}

const LoginBox = () => {
  const [credentials, setCredentials] = useState<LoginCredentials>({ username: '', passwordHash: '' });

  const { login, error, isLoading } = useLogin();  // Call useLogin at the top of the component

  const handleLogin = async () => {
    const test = triggerLogin(credentials);  // Trigger the login function
  };

  return (
    <Card direction={{ base: 'column', sm: 'row' }} overflow="hidden" variant="elevated">
      <Image objectFit="cover" maxW={{ base: '100%', sm: '200px' }} src="https://loremflickr.com/1280/720" />

      <Stack>
        <CardBody>
          <Heading size="md">Log in</Heading>
        </CardBody>

        <CardBody>
          <InputFieldElement
            type="text"
            name="Username"
            placeholder="Username"
            value={credentials.username}
            onChange={(username) => setCredentials({ ...credentials, username })}
          />
          <InputFieldElement
            type="password"
            name="Password"
            placeholder="Password"
            value={credentials.passwordHash}
            onChange={(password) => setCredentials({ ...credentials, passwordHash: password })}
          />
          <Button variant="solid" colorScheme="blue" mt={3} onClick={handleLogin} isLoading={isLoading}>
            Sign in
          </Button>
          {error && <Text color="red.500">Error: {error}</Text>}
        </CardBody>

        <CardFooter>
          <Text>
            Or sign up <Link href="/signup">here</Link>
          </Text>
        </CardFooter>
      </Stack>
    </Card>
  );
};

export default LoginBox;
