import { usePost } from "./useData";
export interface Token {
    token: string;
}
export interface Credentials {
    username: string;
    password: string;
}
const login = (credentials: Credentials) => usePost<Credentials, Token>(`/auth/login`, credentials);
const signUp = (credentials: Credentials) => usePost<Credentials, string/*no return data*/>(`/auth/signup`, credentials);


export { login, signUp };
