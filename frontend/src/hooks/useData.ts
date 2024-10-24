import useApiClient from "../services/apiClient";


const apiClient = new useApiClient();
const useGet = <T>(endpoint: string) => {
    return apiClient.get<T>(endpoint);
}

const usePost = <TBody, TReturn>(endpoint: string, body: TBody) => {
    return apiClient.post<TBody, TReturn>(endpoint, body);
};

const usePut = <TBody, TReturn>(endpoint: string, body: TBody) => {
    return apiClient.put<TBody, TReturn>(endpoint, body);
};

const useDelete = <T>(endpoint: string) => {
    return apiClient.delete<T>(endpoint);
};
const setToken = (token: string) => {
    apiClient.setToken(token);
}
export { useGet, usePost, usePut, useDelete, setToken };