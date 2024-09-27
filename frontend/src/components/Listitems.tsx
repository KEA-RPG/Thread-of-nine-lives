import { Button, HStack, Box } from "@chakra-ui/react";

type ListItemProps = {
    id: number;
  };

const ListItem = ({ id }: ListItemProps) => {
    return (
        <HStack
            justifyContent="space-between"
            w="100%"
            bg="gray.600"
            p={4}
            borderRadius="md"
        >
            <Box flex="1" color="white">
               Name {id}
            </Box>
            <HStack spacing={4}>
                <Button colorScheme="blue">Select</Button>
                <Button colorScheme="gray">Edit</Button>
                <Button colorScheme="red">Delete</Button>
            </HStack>
        </HStack>
    );
};

export default ListItem;