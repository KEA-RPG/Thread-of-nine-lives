import { Image, Button, Card, CardBody, CardFooter, Heading, Stack, Text, Link } from '@chakra-ui/react';
import { useState } from 'react';
import InputFieldElement from '../components/InputFieldElement';
import { LoginCredentials } from '../hooks/useUser';
import { useUserContext } from '../components/UserContext'; // Assuming you're handling auth here

const LoginBox = () => {
  const [credentials, setCredentials] = useState<LoginCredentials>({ username: '', passwordHash: '' });
  const { login } = useUserContext();

  const handleLogin = async () => {
    login(credentials);
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
          <Button variant="solid" colorScheme="blue" mt={3} onClick={handleLogin} >
            Sign in
          </Button>
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
