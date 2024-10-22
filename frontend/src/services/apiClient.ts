import axios from "axios";
import { useContext, useEffect } from "react";
import { UserContext } from "../components/UserContext";

// Create an Axios instance
const apiClient = axios.create({
  baseURL: "https://localhost:7195/",
  timeout: 1000,
});

const useApiClient = () => {
  const userContext = useContext(UserContext);
  if (!userContext) {
    throw new Error("UserContext is undefined");
  }
  const { user } = userContext;

  useEffect(() => {
    const requestInterceptor = apiClient.interceptors.request.use(
      (config) => {
        if (user?.loggedIn && user.token) {
          config.headers['Authorization'] = `Bearer ${user.token}`;
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );

    return () => {
      apiClient.interceptors.request.eject(requestInterceptor);
    };
  }, [user]);

  return apiClient;
};

export default useApiClient;
