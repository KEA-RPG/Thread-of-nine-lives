import { Image, Button, Card, CardBody, CardFooter, Heading, Stack, Text, FormControl, FormLabel, Input, Link } from '@chakra-ui/react'
const LoginBox = () => {
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
        <Heading size='md'>Log in</Heading>
      </CardBody>
      <CardBody>
        <FormControl>
          <FormLabel>Username</FormLabel>
          <Input type="email"></Input>
        </FormControl>
        <FormControl>
          <FormLabel>Password</FormLabel>
          <Input type="password"></Input>
        </FormControl>
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