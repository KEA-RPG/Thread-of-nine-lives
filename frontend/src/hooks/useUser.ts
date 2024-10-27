import { usePost } from "./useData";
export interface Token {
    token: string;
}
export interface Credentials {
    username: string;
    password: string; //Skal hedde noget andet når api er opdateret
}
const useLogin = (credentials: Credentials) => usePost<Credentials, Token>(`/auth/login`, credentials);
const useSignUp = (credentials: Credentials) => usePost<Credentials, string/*no return data*/>(`/auth/signup`, credentials);


export { useLogin, useSignUp };
