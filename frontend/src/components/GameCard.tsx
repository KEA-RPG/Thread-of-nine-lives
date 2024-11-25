import { Box, Card, CardBody, CardFooter, CardHeader, Heading, Image, VStack } from '@chakra-ui/react';
import { motion } from "framer-motion"
import CardEffectBadge from './CardEffectBadge';
import { Card as CardModel } from '../hooks/useCard';

interface Props {
    card: CardModel;
}


const GameCard = (props: Props) => {
    const { name, imagePath, attack, defence, description } = props.card as CardModel;

    return (
        <motion.div
            whileHover={{ scale: 1.03 }}
            whileTap={{ scale: 0.98 }}>
            <Card w={200} userSelect="none">
                <CardHeader>
                    <Heading as="h4" size="md">{name}</Heading>
                </CardHeader>
                <CardBody>
                    <VStack>
                        <Image src={imagePath} alt={name} border="1px solid black" w="160px" h="160px" />
                        <Box>{description}</Box>
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