import { Button, VStack, Box } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import ListItem from "./Listitems";
import { useParams, useSearchParams } from "react-router-dom";
import { useToast } from '@chakra-ui/react'

type Item = {
    id: number;
};

const ListLayout = () => {
    const toast = useToast()

    const [items, setItems] = useState<Item[]>([]);
    const [searchParams, setSearchParams] = useSearchParams();
    searchParams.get("success");
    const handleAddItem = () => {
        setItems((prevItems) => [
            ...prevItems,
            { id: prevItems.length + 1 },
        ]);
    };
    useEffect(() => {
        displaySuccess();
    }, [searchParams.get("success")]);
    const displaySuccess = () => {
        if (searchParams.get("success") === "true") {
            toast({
                description: "Action successful",
                status: "success",
            })
        }
        else if (searchParams.get("success") === "false") {
            toast({
                description: "Action failed",
                status: "error",
            })
        }
    }
    displaySuccess();
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