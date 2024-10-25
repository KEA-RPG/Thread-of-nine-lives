import { useUserContext } from "./UserContext";

const Logout = () => {
    const { logout } = useUserContext();

    return (
        <>
            {logout()}
        </>
    )
};

export default Logout;