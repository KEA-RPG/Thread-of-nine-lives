import { Button, VStack, HStack, Box } from "@chakra-ui/react";

const ListItems = () => {
    return (
        <HStack
            justifyContent="space-between"
            w="100%"
            bg="gray.600"
            p={4}
            borderRadius="md"
        >
            <Box flex="1" color="white">
                Name
            </Box>
            <HStack spacing={4}>
                <Button colorScheme="blue">Select</Button>
                <Button colorScheme="gray">Edit</Button>
                <Button colorScheme="red">Delete</Button>
            </HStack>
        </HStack>
    );
};

const ListLayout = () => {
    return (
        <VStack spacing={4} p={6} bg="gray.300" align="stretch">
            <ListItems />
            <ListItems />
            <ListItems />
            <Box display="flex" justifyContent="flex-end" p={4}>
                <Button colorScheme="green">New</Button>
            </Box>
        </VStack>
    );
};

export default ListLayout;