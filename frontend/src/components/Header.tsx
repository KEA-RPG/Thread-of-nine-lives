import React, {useState} from 'react';
import {Button, HStack, Text} from '@chakra-ui/react';
import { MoonIcon } from '@chakra-ui/icons';
import NavBar from "./NavBar"


const Header: React.FC = () => {
    const [user, setUser] = useState<{ loggedIn: boolean; isAdmin: boolean } | null>(null);
  
    const handleLogin = () => {
      setUser({ loggedIn: true, isAdmin: false });
    };
  
    const handleAdminLogin = () => {
      setUser({ loggedIn: true, isAdmin: true });
    };
  
    const handleLogout = () => {
      setUser(null);
    };

    return <HStack justifyContent="space-between">
        <MoonIcon/>
        <Text>Thread of Nine Lives</Text>
        {!user && (
        <>
          <Button onClick={handleLogin}>Login as User</Button>
          <Button onClick={handleAdminLogin}>Login as Admin</Button>
        </>
      )}
      {user && user.loggedIn && (
        <>
          <Button onClick={handleLogout}>Logout</Button>
          {user.isAdmin ? (
            <Button>Admin Dashboard</Button>
          ) : (
            <Button>User Profile</Button>
          )}
        </>
      )}  
      <NavBar />      
    </HStack>
};

export default Header;