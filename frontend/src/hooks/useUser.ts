import { usePost } from "./useData";
export interface Token {
    token: string;
}
export interface LoginCredentials {
    username: string;
    passwordHash: string; //Skal hedde noget andet når api er opdateret
}
const useLogin = (credentials: LoginCredentials) => usePost<LoginCredentials, Token>(`/auth/login`, credentials)


export default useLogin;
