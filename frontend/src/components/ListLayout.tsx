import { Button, VStack, HStack, Box } from "@chakra-ui/react";
import { useState} from "react";

type Item = {
    id: number;
};

const ListItems = ({ id }: { id:number }) => {
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

const ListLayout = () => {
    const [items, setItems] = useState<Item[]>([]);

    const handleAddItem = () => {
        setItems((prevItems) => [
            ...prevItems,
            { id: prevItems.length + 1 },
        ]);
    };

    return (
        <VStack spacing={4} p={6} bg="gray.300" align="stretch">
            {items.map((item) => (
                <ListItems key={item.id} id={item.id} />
            ))}

            <Box display="flex" justifyContent="flex-end" p={4}>
                <Button colorScheme="green" onClick={handleAddItem}>New</Button>
            </Box>
        </VStack>
    );
};

export default ListLayout;