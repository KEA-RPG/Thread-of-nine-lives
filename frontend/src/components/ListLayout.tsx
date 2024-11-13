import { Button, VStack, Box } from "@chakra-ui/react";
import { useState } from "react";
import ListItem from "./Listitems";

type Item = {
    id: number;
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
                <ListItem key={item.id} id={item.id} />
            ))}

            <Box display="flex" justifyContent="flex-end" p={5}>
                <Button colorScheme="green" onClick={handleAddItem}>New</Button>
            </Box>
        </VStack>
    );
};

export default ListLayout;