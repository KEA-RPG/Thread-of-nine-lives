import { Button, VStack, Box, Card, CardBody } from "@chakra-ui/react";
import ListItem from "./Listitems";
import { ListItemBase } from "../hooks/useData";


type Props<T extends ListItemBase> = {
    data: T[];
    onAdd?: () => void;
    onEdit?: (item: T) => void;
    onDelete?: (item: T) => void;
    onSelection?: (item: T) => void;
};

const ListLayout = <T extends ListItemBase>({ data, onAdd, onEdit, onDelete, onSelection }: Props<T>) => {
    return (
        <VStack spacing={4} p={6} bg="gray.300" align="stretch" w="80%">
            <Box display="flex" justifyContent="flex-end">
                <Button colorScheme="green" onClick={onAdd}>New</Button>
            </Box>
            {data.length === 0 &&
                <Card w={200} userSelect="none" alignSelf="center">
                    <CardBody>
                        <VStack>
                            <Box>No data</Box>
                        </VStack>
                    </CardBody>
                </Card>

            }
            {data.map((item) => (
                <ListItem
                    key={item.id}
                    data={item}
                    onEdit={onEdit && (() => onEdit(item))}
                    onDelete={onDelete && (() => onDelete(item))}
                    onSelection={onSelection && (() => onSelection(item))}
                />
            ))}

        </VStack>
    );
};

export default ListLayout;