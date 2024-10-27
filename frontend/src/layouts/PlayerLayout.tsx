import { useEffect } from 'react';
import { useUserContext } from '../components/UserContext';
import MainLayout, { LayoutProps } from './MainLayout';


const PlayerLayout = ({ children, header }: LayoutProps) => {
    const {token, requireLogin } = useUserContext();
    useEffect(() => {
        requireLogin("player");
    }, [token]);

    return (
        <MainLayout header={header}>
            {children}
        </MainLayout>
    );
};

export default PlayerLayout;
