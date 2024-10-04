import { Box, Card, CardBody, CardFooter, CardHeader, Heading, Image, VStack } from '@chakra-ui/react';
import { motion } from "framer-motion"
import CardEffectBadge from './CardEffectBadge';

interface Props {
    card: Card;
}

export interface Card {
    id: number;
    title: string;
    image_path: string;
    content: string;
    attack?: number;
    defence?: number;
}

const GameCard = (props: Props) => {
    const { title, image_path, content, attack, defence } = props.card;

    return (
        <motion.div
            whileHover={{ scale: 1.03 }}
            whileTap={{ scale: 0.98 }}>
            <Card w={200} userSelect="none">
                <CardHeader>
                    <Heading as="h4" size="md">{title}</Heading>
                </CardHeader>
                <CardBody>
                    <VStack>
                        <Image src={image_path} alt={title} border="1px solid black" w="160px" h="160px" />
                        <Box>{content}</Box>
                    </VStack>

                </CardBody>
                <CardFooter>
                    <CardEffectBadge defence={defence} attack={attack} />
                </CardFooter>
            </Card>
        </motion.div>
    );
};

export default GameCard;