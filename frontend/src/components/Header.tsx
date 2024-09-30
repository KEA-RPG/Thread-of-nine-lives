import React from 'react';
import { Button, HStack, Spacer, Text } from '@chakra-ui/react';
import { MoonIcon } from '@chakra-ui/icons';
import NavBar from "./NavBar"
import useUser from '../hooks/useUser';


const Header: React.FC = () => {
  const { user, loginAsUser, loginAsAdmin, logout } = useUser();

  return <HStack justifyContent="space-between">
      <HStack>
        <MoonIcon />
        <Text>Thread of Nine Lives</Text>
      </HStack>

    <Spacer />
    <HStack spacing={4}>
    {!user && (
      <>
        <Button onClick={loginAsUser}>Login as User</Button>
        <Button onClick={loginAsAdmin}>Login as Admin</Button>
      </>
    )}
    {user && user.loggedIn && (
      <>
        <Button onClick={logout}>Logout</Button>
        {user.isAdmin ? (
          <Button>Admin Dashboard</Button>
        ) : (
          <Button>User Profile</Button>
        )}
      </>
    )}
    <NavBar />
    </HStack>
  </HStack>
};

export default Header;