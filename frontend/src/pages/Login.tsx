import { useState } from "react";
import { Image, Button, Card, CardBody, CardFooter, Stack, Text, Link, Box } from "@chakra-ui/react";
import InputFieldElement from "../components/InputFieldElement";
import { Credentials } from "../hooks/useUser";
import { useUserContext } from "../components/UserContext";

const Login = () => {
  const [credentials, setCredentials] = useState<Credentials>({ username: "", password: "" });
  const [error, setError] = useState<boolean>(false);
  const { handleLogin } = useUserContext();

  const submitLogin = async () => {
    setError(false);

    const result = await handleLogin(credentials);
    if (result.error) {
      setError(true);
      return;
    }

  };

  return (
    <Card direction={{ base: "column", sm: "row" }} overflow="hidden" variant="elevated">
      <Image objectFit="cover" maxW={{ base: "100%", sm: "200px" }} src="https://loremflickr.com/1280/720" />

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
          <Button variant="solid" colorScheme="blue" mt={3} onClick={submitLogin}>
            Sign in
          </Button>
          <Box h={2}>
            {error && <Text color="red.500">Invalid username or password</Text>}
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
