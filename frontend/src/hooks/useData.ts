import useApiClient from "../services/apiClient";
export type ListItemBase = {
    id?: number;
    name: string;
};

const apiClient = new useApiClient();
const useGet = <T>(endpoint: string) => {
    let data = apiClient.get<T>(endpoint);
    return data;
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