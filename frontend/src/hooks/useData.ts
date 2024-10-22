import { useEffect, useState } from "react";
import useApiClient from "../services/apiClient";


const useGet = <T>(endpoint: string) => {
    const apiClient = useApiClient(); 
    const [data, setData] = useState<T>();
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        apiClient.get<T>(endpoint)
            .then((response) => setData(response.data))
            .catch((error) => setError(error.message))
            .finally(() => setIsLoading(false))
    }, [])
    return { data, error, isLoading };
}
const usePost = <res, req>(endpoint: string, object:req) => {
    const apiClient = useApiClient(); 
    const [data, setData] = useState<res>();
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(true);
    useEffect(() => {
        apiClient.post<res>(endpoint,object)
            .then((response) => setData(response.data))
            .catch((error) => setError(error.message))
            .finally(() => setIsLoading(false))
    }, [])
    return { data, error, isLoading };
}
const usePut = <res, req>(endpoint: string, object:req) => {
    const apiClient = useApiClient(); 
    const [data, setData] = useState<res>();
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(true);
    useEffect(() => {
        apiClient.put<res>(endpoint,object)
            .then((response) => setData(response.data))
            .catch((error) => setError(error.message))
            .finally(() => setIsLoading(false))
    }, [])
    return { data, error, isLoading };
}
export { useGet, usePost, usePut };