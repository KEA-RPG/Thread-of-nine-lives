import { ReactNode } from 'react';
import { Box, Grid, GridItem, Heading, VStack } from '@chakra-ui/react';
import Header from '../components/Header';

export interface LayoutProps {
    children: ReactNode;
    header?: string;
}

const MainLayout = ({ children, header }: LayoutProps) => {
    return <Grid
        templateAreas={`"header" "main"`}
        h='200px'
        gap='1'
        color='blackAlpha.700'
        fontWeight='bold'
    >
        <GridItem pl='2' bg='orange.300' area={'header'}>
            <Header text={header ?? 'Default Title'} />
        </GridItem>
        <GridItem pl='2' area={'main'} display="flex" justifyContent="center" alignItems="center">
            <VStack spacing={5}>
                <Box backgroundColor="rgba(0, 0, 0, 0.3);" color="lightgray" py="20px" px="25px" rounded="10px" mt="20px" minW="80vw" display="flex" justifyContent="center" alignItems="center">
                    {children}
                </Box>
            </VStack>
        </GridItem>
    </Grid>




};

export default MainLayout;
