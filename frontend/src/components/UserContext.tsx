import { createContext, ReactNode, useContext } from "react";
import { useNavigate } from "react-router-dom";

import { jwtDecode } from 'jwt-decode';
import { Response } from '../services/apiClient';
import { Credentials, Token, useLogin, useSignUp } from "../hooks/useUser";

interface UserContextType {
  token: string | null;
  username: string | null;
  login: (credentials: Credentials) => Promise<Response<Token>>;
  logout: () => void;
  signUp: (credentials: Credentials) => Promise<Response<string>>;
  requireLogin: (role: string) => void;
  role: string | null;
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
  const navigate = useNavigate();

  // Helper functions to get values from local storage
  const getToken = (): string | null => localStorage.getItem("token");
  const getRole = (): string | null => localStorage.getItem("role");
  const getUsername = (): string | null => localStorage.getItem("username");

  // Function to set values directly in local storage
  const setToken = (token: string) => localStorage.setItem("token", token);
  const setRole = (role: string) => localStorage.setItem("role", role);
  const setUsername = (username: string) => localStorage.setItem("username", username);

  const login = async (credentials: Credentials): Promise<Response<Token>> => {
    const loggedInUser = await useLogin(credentials);
    if (loggedInUser && loggedInUser.data) {
      setToken(loggedInUser.data.token)
      const decodedToken = jwtDecode(loggedInUser.data.token) as JwtToken;
      if (decodedToken.role) {
        setToken(loggedInUser.data.token);
        setRole(decodedToken.role.toLowerCase());
        setUsername(decodedToken.sub);
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
  const requireLogin = (requiredRole: string) => {
    const role = getRole();
    const token = getToken(); 
    if (!token) {
      console.error('No token found, redirecting to login');
      navigate('/login');
    }
    else if (requiredRole === "admin" && role !== "admin") {
      console.error('Admin role required, but current role is not admin, redirecting to login');
      navigate('/login');
    }
    else if (requiredRole !== "player" && requiredRole !== "admin") {
      console.error('Invalid role specified, role not found');
    }
  }
  return (
    
    <UserContext.Provider value={{ token: getToken(), username: getUsername(), login, logout, signUp, requireLogin, role: getRole() }}>
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