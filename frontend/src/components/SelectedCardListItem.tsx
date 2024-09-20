import { Box, HStack, Text } from "@chakra-ui/react";

interface SelectedCardListItemProps {
    count: number;
    title: string;
}
const SelectedCardListItem = ({ title, count }: SelectedCardListItemProps) => {

    return (
        <Box w="100%" h="25px" mb="10px">
            <HStack justifyContent="space-between" h="25px" p="2"  boxShadow="1px 1px 2px black" rounded="10px" userSelect="none">
                <Text>{title}</Text>
                {count > 1 && (
                    <Box>
                        <Text>({count})</Text>
                    </Box>
                )}

            </HStack>
        </Box>
    );
};

export default SelectedCardListItem;