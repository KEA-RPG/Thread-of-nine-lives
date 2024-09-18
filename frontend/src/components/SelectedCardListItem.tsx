import { Box, HStack, Image, Text } from "@chakra-ui/react";
import { CardProp } from "./CardCard";

interface SelectedCardListItemProps {
    count: number;
    card: CardProp;
}
const SelectedCardListItem = (props: SelectedCardListItemProps) => {
    const { card: { title, image_path } } = props;

    return (
        <Box>
            <HStack>
                <Image src={image_path} alt={title} height={20} />
                <Text>{title}</Text>
                {props.count > 1 && (
                    <Box>
                        <Text>{props.count}</Text>
                    </Box>
                )}

            </HStack>
        </Box>
    );
};

export default SelectedCardListItem;