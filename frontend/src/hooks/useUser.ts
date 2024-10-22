import { LoginCredentials } from "../login/LoginBox";
import { usePost } from "./useData";

const useLogin = () => {
    const { data, error, isLoading, triggerPost } = usePost<string, LoginCredentials>(`/auth/login`);

    // Return the triggerPost function, data, error, and isLoading state
    return { login: triggerPost, data, error, isLoading };
};
const useLogin = () => usePost<Enemy, Enemy>(`/enemies`);

export default useLogin;
