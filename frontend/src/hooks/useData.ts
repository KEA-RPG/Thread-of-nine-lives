import { useState } from "react";
import useApiClient from "../services/apiClient";


const apiClient = useApiClient();
const useGet = <T>(endpoint: string) => {
    const [data, setData] = useState<T>();
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    const triggerGet = () => {
        setIsLoading(true);
        setError(null);

        apiClient.get<T>(endpoint)
            .then((response) => setData(response.data))
            .catch((error) => setError(error.message))
            .finally(() => setIsLoading(false))
    }

    return { data, error, isLoading, triggerGet };
}

const usePost = <res, req>(endpoint: string,) => {
    const [data, setData] = useState<res | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const triggerPost = (object: req) => {
        setIsLoading(true);
        setError(null);

        apiClient.post<res>(endpoint, object)
            .then((response) => setData(response.data))
            .catch((error) => setError(error.message))
            .finally(() => setIsLoading(false));
    };

    return { data, error, isLoading, triggerPost };
};

const usePut = <res, req>(endpoint: string) => {
    const [data, setData] = useState<res>();
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const triggerPut = (object: req) => {
        setIsLoading(true);
        setError(null);

        apiClient.put<res>(endpoint, object)
            .then((response) => setData(response.data))
            .catch((error) => setError(error.message))
            .finally(() => setIsLoading(false));
    }
    return { data, error, isLoading, triggerPut };
}
export { useGet, usePost, usePut };