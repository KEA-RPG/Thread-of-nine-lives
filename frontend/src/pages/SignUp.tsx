import { useState } from 'react';
import { Image, Button, Card, CardBody, Stack, CardFooter, Text, Link, Box } from '@chakra-ui/react'
import { useToast } from '@chakra-ui/react'
import InputFieldElement from '../components/InputFieldElement';
import { Credentials } from '../hooks/useUser';
import { useUserContext } from '../components/UserContext';

const SignUp = () => {
  const [credentials, setCredentials] = useState<Credentials>({ username: '', password: '' });
  const [repeatPassword, setRepeatPassword] = useState<string>('');
  const [error, setError] = useState<string>('');
  const { signUp } = useUserContext();
  const toast = useToast()


  const handleSignUp = async () => {

    setError('');

    if (credentials.password !== repeatPassword) {
      setError('Passwords do not match');
      return;
    }
    var result = await signUp(credentials);
    if (result.error) {
      setError(result.error);
    }
    else {
      toast({
        description: "User created successfully", 
        status: "success",
      })
      console.log("User created successfully");
    }
    console.log(result);

  };

  return <Card direction={{ base: 'column', sm: 'row' }} overflow="hidden" variant="elevated">
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
        <InputFieldElement
          type="password"
          name="Repeat Password"
          placeholder="Repeat Password"
          onChange={(password) => setRepeatPassword(password)}
        />
        <Button variant="solid" colorScheme="blue" mt={3} onClick={handleSignUp} >
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