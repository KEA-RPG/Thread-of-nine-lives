import { useState } from 'react';
import { Image, Button, Card, CardBody, Heading, Stack, CardFooter, Text, Link } from '@chakra-ui/react'
import SignUpHandler from './SignUpHandler';
import InputFieldElement from '../components/InputFieldElement';


const SignUpBox = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [repeatPassword, setRepeatPassword] = useState('');
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setError(null);

    try {
      await SignUpHandler({ username, password });
    } catch (error) {
      setError('Signup failed. Please try again.');
    }
  };

  return <Card
    direction={{ base: 'column', sm: 'row' }}
    overflow='hidden'
    variant='outline'>

    <Image
      objectFit='cover'
      maxW={{ base: '100%', sm: '200px' }}
      src='https://loremflickr.com/1280/720'
    />

    <Stack>
      <CardBody>
        <Heading size='md'>Sign up</Heading>
      </CardBody>
      <CardBody>
        <InputFieldElement type="text" name="Username" placeholder="Username" value={username} onChange={(e) => setUsername(e)} />
        <InputFieldElement type="password" name="Password" placeholder="Password" value={password} onChange={(e) => setPassword(e)} />
        <InputFieldElement type="password" name="Repeat Password" placeholder="Repeat Password" value={repeatPassword} onChange={(e) => setRepeatPassword(e)} />
        <Button variant='solid' colorScheme='blue' mt={3} onClick={handleSubmit}>
          Sign up
        </Button>
      </CardBody>
      <CardFooter>
        <Text>Or sign in <Link href='/'>here</Link></Text>
      </CardFooter>
    </Stack>
  </Card>
}

export default SignUpBox