import { useState } from 'react';
import { Image, Button, Card, CardBody, Stack, CardFooter, Text, Link, Box, useToast } from '@chakra-ui/react'
import InputFieldElement from '../components/InputFieldElement';
import { Credentials } from '../hooks/useUser';
import { useUserContext } from '../components/UserContext';

const SignUp = () => {
  const [credentials, setCredentials] = useState<Credentials>({ username: '', password: '' });
  const [repeatPassword, setRepeatPassword] = useState<string>('');
  const [error, setError] = useState<string>('');
  const { handleSignUp } = useUserContext();
  const toast = useToast()


  const submitSignUp = async () => {

    setError('');

    if (credentials.password !== repeatPassword) {
      setError('Passwords do not match');
      return;
    }
    const result = await handleSignUp(credentials);
    if (result.error) {
      setError(result.error.message);
    }
    else {
      toast({
        description: "User created successfully", 
        status: "success",
      })
    }
  };

  return <Card direction={{ base: 'column', sm: 'row' }} overflow="hidden" variant="elevated">
    <Image objectFit="cover" maxW={{ base: '100%', sm: '200px' }} src="https://loremflickr.com/1280/720" />

    <Stack>
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
          value={credentials.password}
          onChange={(password) => setCredentials({ ...credentials, password: password })}
        />
        <InputFieldElement
          type="password"
          name="Repeat Password"
          placeholder="Repeat Password"
          value={repeatPassword}
          onChange={(password) => setRepeatPassword(password)}
        />
        <Button variant="solid" colorScheme="blue" mt={3} onClick={submitSignUp} >
          Sign up
        </Button>
        <Box h={2}>
          {error && <Text color="red.500">
            {error}
          </Text>}
        </Box>
      </CardBody>

      <CardFooter>
        <Text>
          Or sign in <Link href="/login">here</Link>
        </Text>
      </CardFooter>
    </Stack>
  </Card>

}

export default SignUp