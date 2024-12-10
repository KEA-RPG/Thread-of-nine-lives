import { usePost } from "./useData";
export interface TokenResponse {
    token: string;
    requestToken : string;
}
export interface Credentials {
    username: string;
    password: string;
}
const login = (credentials: Credentials) => usePost<Credentials, TokenResponse>(`/auth/login`, credentials);
const signUp = (credentials: Credentials) => usePost<Credentials, string/*no return data*/>(`/auth/signup`, credentials);


export { login, signUp };
