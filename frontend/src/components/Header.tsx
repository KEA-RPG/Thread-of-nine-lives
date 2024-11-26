import { useEffect, useState } from 'react';
import { Button, HStack, Link, Spacer, Text } from '@chakra-ui/react';
import { MoonIcon } from '@chakra-ui/icons';
import { useUserContext } from './UserContext';
interface Props {
  text: string;
}
const Header = ({ text }: Props) => {
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
      <a href={userRole ? "/menu" : "/"} >
        <HStack spacing={2} w="200px">
          <MoonIcon />
          <Text>Thread of Nine Lives</Text>
        </HStack>
      </a>
      <Spacer />
      <Text>{text}</Text>
      <Spacer />
      <HStack spacing={4} mr={4} w="200px" justifyContent="end">
        {buttonMenu()}
      </HStack>
    </HStack>
  );
};

export default Header;
