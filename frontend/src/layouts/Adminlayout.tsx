import { useEffect } from 'react';
import { useUserContext } from '../components/UserContext';
import MainLayout, { LayoutProps } from './MainLayout';


const AdminLayout = ({ children, header }: LayoutProps) => {
    const {token, requireLogin } = useUserContext();
    useEffect(() => {
        requireLogin("admin");
    }, [token]);

    return (
        <MainLayout header={header}>
            {children}
        </MainLayout>
    );
};

export default AdminLayout;
