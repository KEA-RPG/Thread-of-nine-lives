import { createContext, ReactNode, useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import useLogin, { LoginCredentials, Token } from "../hooks/useUser";
import { jwtDecode } from 'jwt-decode';
import { Response } from '../services/apiClient';

interface UserContextType {
  token?: string;
  username?: string;
  login: (credentials: LoginCredentials) => Promise<Response<Token>>;
  logout: () => void;
  role?: string;
}
interface JwtToken {
  sub: string;
  iat: number;
  exp: number;
  iss?: string;
  aud?: string | string[];
  role?: string;
  name: string;
}

// Create the UserContext
const UserContext = createContext<UserContextType | undefined>(undefined);

// UserContext provider component
export const UserProvider = ({ children }: { children: ReactNode }) => {
  const [role, setRole] = useState<string>();
  const [token, setToken] = useState<string>();
  const [username, setUsername] = useState<string>();
  const navigate = useNavigate();
  const login = async (credentials: LoginCredentials): Promise<Response<Token>> => {
    // Replace with actual login logic (e.g., API call)
    const loggedInUser = await useLogin(credentials);

    if (loggedInUser && loggedInUser.data) {
      setToken(loggedInUser.data.token)
      const decodedToken = jwtDecode(loggedInUser.data.token) as JwtToken;
      if (decodedToken.role) {
        setRole(decodedToken.role.toLowerCase());
        setUsername(decodedToken.name);
        navigate('/menu');
      }
    }
    else {
      console.error('Login failed: result is undefined');
    }
    return loggedInUser;
  };

  const logout = () => {
    useEffect(() => {
      console.log("logged out");
      setRole("");
      navigate('/');
    })
  };

  return (
    <UserContext.Provider value={{ token, username, login, logout, role }}>
      {children}
    </UserContext.Provider>
  );
};
// Custom hook to use the UserContext
export const useUserContext = (): UserContextType => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error('useUserContext must be used within a UserProvider');
  }
  return context;
};