import { Box, Card, CardBody, CardFooter, CardHeader, Heading, Image } from '@chakra-ui/react';

export interface CardProp {
    title: string;
    image_path: string;
    content: string;
    attack?: number;
    defense?: number;

}

const CardCard = (props: CardProp) => {
    const { title, image_path, content, attack, defense } = props;
    return (
        <Card w={200}>
            <CardHeader>
                <Heading>{title}</Heading>
            </CardHeader>
            <CardBody>
                <Image src={image_path} alt={title} />
                <Box>{content}</Box>
                {attack !== undefined && <Box>Attack: {attack}</Box>}
                {defense !== undefined && <Box>Defense: {defense}</Box>}
            </CardBody>
            <CardFooter>

            </CardFooter>
        </Card>
    );
};

export default CardCard;