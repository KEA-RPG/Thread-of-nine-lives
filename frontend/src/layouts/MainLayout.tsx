import { ReactNode } from 'react';
import { Box, Heading, VStack } from '@chakra-ui/react';

export interface LayoutProps {
    children: ReactNode;
    header?: string;
}

const MainLayout = ({ children, header }: LayoutProps) => {
    return <VStack spacing={5}>
        {header !== undefined ? (
            <Heading as='h3' size='xl'>
                {header}
            </Heading>
        ) : null}
        <Box backgroundColor="rgba(0, 0, 0, 0.3);" color="lightgray" py="20px" px="25px" rounded="10px" mt="20px" minW="80vw" display="flex" justifyContent="center" alignItems="center">
            {children}
        </Box>
    </VStack>
};

export default MainLayout;
