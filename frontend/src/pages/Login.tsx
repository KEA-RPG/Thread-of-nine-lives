import { Image, Button, Card, CardBody, CardFooter, Heading, Stack, Text, Link, Box } from '@chakra-ui/react';
import { useState } from 'react';
import InputFieldElement from '../components/InputFieldElement';
import { Credentials } from '../hooks/useUser';
import { useUserContext } from '../components/UserContext'; // Assuming you're handling auth here

const Login = () => {
  const [credentials, setCredentials] = useState<Credentials>({ username: '', password: '' });
  const [error, setError] = useState<boolean>(false);
  const { login } = useUserContext();

  const handleLogin = async () => {
    setError(false);
    const test = await login(credentials);
    if (test.error) {
      setError(true);
    };
  }

  return (
    <Card direction={{ base: 'column', sm: 'row' }} overflow="hidden" variant="elevated">
      <Image objectFit="cover" maxW={{ base: '100%', sm: '200px' }} src="https://loremflickr.com/1280/720" />

      <Stack>
        <CardBody>
          <InputFieldElement
            type="text"
            name="Username"
            placeholder="Username"
            onChange={(username) => setCredentials({ ...credentials, username })}
          />
          <InputFieldElement
            type="password"
            name="Password"
            placeholder="Password"
            onChange={(password) => setCredentials({ ...credentials, password: password })}
          />
          <Button variant="solid" colorScheme="blue" mt={3} onClick={handleLogin} >
            Sign in
          </Button>
          <Box h={2}>
            {error && <Text color="red.500">
              Invalid username or password
            </Text>}
          </Box>
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

export default Login;
