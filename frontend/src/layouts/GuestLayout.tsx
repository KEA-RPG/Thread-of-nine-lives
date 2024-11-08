import MainLayout, { LayoutProps } from './MainLayout';


const GuestLayout = ({ children, header }: LayoutProps) => {
    return (
        <MainLayout header={header}>
            {children}
        </MainLayout>
    );
};

export default GuestLayout;
