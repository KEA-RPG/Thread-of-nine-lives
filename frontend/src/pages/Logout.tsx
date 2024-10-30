import { useUserContext } from '../components/UserContext';

const Logout = () => {
    const { logout } = useUserContext();

    return (
        <>
            {logout()}
        </>
    )
};

export default Logout;