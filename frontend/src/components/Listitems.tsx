import { Button, HStack, Box } from "@chakra-ui/react";
import { ListItemBase } from "../hooks/useData";

type Props<T extends ListItemBase> = {
    data: T;
    onEdit?: (item: T) => void;
    onDelete?: (item: T) => void;
    onSelection?: (item: T) => void;
};

const ListItem = <T extends ListItemBase>({ data, onEdit, onDelete, onSelection, }: Props<T>) => {
    return (
        <HStack
            justifyContent="space-between"
            w="100%"
            bg="gray.600"
            p={4}
            borderRadius="md"
        >
            <Box flex="1" color="white">
                {data.name}
            </Box>
            <HStack spacing={4}>
                {onSelection && <Button colorScheme="blue" onClick={() => onSelection(data)}>Select</Button>}
                {onEdit && <Button colorScheme="gray" onClick={() => onEdit(data)}>Edit</Button>}
                {onDelete &&<Button colorScheme="red" onClick={() => onDelete(data)}>Delete</Button>}
            </HStack>
        </HStack >
    );
};

export default ListItem;