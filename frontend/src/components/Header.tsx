import { useEffect, useState } from 'react';
import { Button, HStack, Link, Spacer, Text } from '@chakra-ui/react';
import { MoonIcon } from '@chakra-ui/icons';
import { useUserContext } from './UserContext';
interface Props {
  text: string;
}
const Header = ({ text }: Props) => {
  const { role, username } = useUserContext();
  const [userRole, setUserRole] = useState(role);

  useEffect(() => {
    setUserRole(role); // Update local state when role changes
  }, [role]);

  const buttonMenu = () => {
    if (userRole === null) {
      return null;
    } else {
      return (
        <>
          <Text>Welcome {username}</Text>
          <Link href="/logout">
            <Button>Logout</Button>
          </Link>
        </>
      );
    }
  };

  return (
    <HStack justifyContent="space-between" h={12}>
      <a href={userRole ? "/menu" : "/"} >
        <HStack spacing={2} w="200px">
          <MoonIcon />
          <Text>Thread of Nine Lives KALI TEST</Text>
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
