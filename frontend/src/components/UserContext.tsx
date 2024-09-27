import React, { createContext, useState, ReactNode } from 'react';

interface User {
  loggedIn: boolean;
  isAdmin: boolean;
}

interface UserContextType {
  user: User | null;
  loginAsUser: () => void;
  loginAsAdmin: () => void;
  logout: () => void;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

const UserProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);

  const loginAsUser = () => setUser({ loggedIn: true, isAdmin: false });
  const loginAsAdmin = () => setUser({ loggedIn: true, isAdmin: true });
  const logout = () => setUser(null);

  return (
    <UserContext.Provider value={{ user, loginAsUser, loginAsAdmin, logout }}>
      {children}
    </UserContext.Provider>
  );
};

export { UserProvider, UserContext };