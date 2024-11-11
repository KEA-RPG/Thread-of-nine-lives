import { createContext, ReactNode, useContext, useMemo } from "react";
import { useNavigate } from "react-router-dom";

import { jwtDecode } from 'jwt-decode';
import { Response } from '../services/apiClient';
import { Credentials, Token, login, signUp } from "../hooks/useUser";

interface UserContextType {
  token: string | null;
  username: string | null;
  handleLogin: (credentials: Credentials) => Promise<Response<Token>>;
  logout: () => void;
  handleSignUp: (credentials: Credentials) => Promise<Response<string>>;
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

  const handleLogin = async (credentials: Credentials): Promise<Response<Token>> => {
    const result = await login(credentials);
    if (result.data) {
      setToken(result.data.token)
      const decodedToken: JwtToken = jwtDecode(result.data.token);
      if (decodedToken.role) {
        setToken(result.data.token);
        setRole(decodedToken.role.toLowerCase());
        setUsername(decodedToken.sub);
        navigate('/menu');
      }
    }
    else {
      console.error('Login failed: result is undefined');
    }
    return result;
  };

  const logout = () => {
    localStorage.removeItem("token");
    setToken("");
    setRole("");
    setUsername("");
    navigate('/');
  };

  const  handleSignUp = async (credentials: Credentials): Promise<Response<string>> => {
    const result = await signUp(credentials);
    return  result;
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
  const contextValue = useMemo(
    () => ({
      token: getToken(),
      username: getUsername(),
      handleLogin,
      logout,
      handleSignUp,
      requireLogin,
      role: getRole(),
    }),
    [getToken(), getUsername(), getRole()]
  );
  return (

    <UserContext.Provider value={contextValue}>
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