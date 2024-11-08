import React, { useEffect, useState } from 'react';
import { Button, HStack, Link, Spacer, Text } from '@chakra-ui/react';
import { MoonIcon } from '@chakra-ui/icons';
import { useUserContext } from './UserContext';

const Header: React.FC = () => {
  const { role, logout } = useUserContext();
  const [userRole, setUserRole] = useState(role);

  useEffect(() => {
    setUserRole(role); // Update local state when role changes
  }, [role]);

  const buttonMenu = () => {
    if (userRole === "admin") {
      return (
        <>
          <Button onClick={logout}>Logout</Button>
          <Button>Admin Dashboard</Button>
        </>
      );
    } else if (userRole === "player") {
      return (
        <Link href="/logout">
          <Button>Logout</Button>
        </Link>
      );
    }
    return null; // Show nothing if role is undefined
  };

  return (
    <HStack justifyContent="space-between" h={12}>
      <a href={userRole ? "/menu" : "/"}>
        <HStack spacing={2}>
          <MoonIcon />
          <Text>Thread of Nine Lives</Text>
        </HStack>
      </a>

      <Spacer />
      <HStack spacing={4} mr={4}>
        {buttonMenu()}
      </HStack>
    </HStack>
  );
};

export default Header;
