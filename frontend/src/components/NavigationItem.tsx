import { Box, Heading, Link } from "@chakra-ui/react";

interface NavigationItemProps {
    link: string;
    children: React.ReactNode;
}

const NavigationItem = ({ link, children }: NavigationItemProps) => {
    return (
        <Link href={link}>
            <Box p="10px">
                <Heading size="lg" textAlign="center">{children}</Heading>

            </Box>
        </Link>
    );
};

export default NavigationItem;