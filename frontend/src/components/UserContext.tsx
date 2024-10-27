import { createContext, ReactNode, useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { jwtDecode } from 'jwt-decode';
import { Response } from '../services/apiClient';
import { Credentials, Token, useLogin, useSignUp } from "../hooks/useUser";

interface UserContextType {
  token?: string;
  username?: string;
  login: (credentials: Credentials) => Promise<Response<Token>>;
  logout: () => void;
  signUp: (credentials: Credentials) => Promise<Response<string>>;
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
  const [token, setToken] = useState<string>(localStorage.getItem("token") || "");
  const [username, setUsername] = useState<string>();
  const navigate = useNavigate();

  useEffect(() => {
    if (token) {
      const decodedToken = jwtDecode<JwtToken>(token);
      setRole(decodedToken.role?.toLowerCase());
      setUsername(decodedToken.name);
    }
  }, [token]);

  const login = async (credentials: Credentials): Promise<Response<Token>> => {
    const loggedInUser = await useLogin(credentials);

    if (loggedInUser && loggedInUser.data) {
      setToken(loggedInUser.data.token)
      const decodedToken = jwtDecode(loggedInUser.data.token) as JwtToken;
      if (decodedToken.role) {
        localStorage.setItem("token", loggedInUser.data.token);
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
    localStorage.removeItem("token");
    setToken("");
    setRole("");
    setUsername("");
    navigate('/');
  };

  const signUp = async (credentials: Credentials): Promise<Response<string>> => {
    const signedUpUser = await useSignUp(credentials);
    return signedUpUser;
  }

  return (
    <UserContext.Provider value={{ token, username, login, logout, signUp, role }}>
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