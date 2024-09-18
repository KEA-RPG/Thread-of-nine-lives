import { Box, Heading, HStack, SimpleGrid, Text } from "@chakra-ui/react";
import CardCard, { CardProp } from "./CardCard";

const DeckManager = () => {
    let selectedDeck: { name: string; cards: CardProp[] } = {
        name: "Deck 1",
        cards: [
            { title: "Card 1", image_path: "/images/card1.png", content: "This is card 1", attack: 10, defense: 5 },
            { title: "Card 2", image_path: "/images/card2.png", content: "This is card 2", attack: 8, defense: 6 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
        ]
    };
    let deck: { name: string; cards: CardProp[] } = {
        name: "Deck 1",
        cards: [
            { title: "Card 1", image_path: "/images/card1.png", content: "This is card 1", attack: 10, defense: 5 },
            { title: "Card 2", image_path: "/images/card2.png", content: "This is card 2", attack: 8, defense: 6 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
            { title: "Card 3", image_path: "/images/card3.png", content: "This is card 3", attack: 7, defense: 7 },
        ]
    };
    const addCard = (card: CardProp) => {
        
    };
    return (
        <Box backgroundColor="rgba(0, 0, 0, 0.3);" color="lightgray" py="20px" px="25px" rounded="10px" mt="20px" textAlign="center">
            <Heading>Deck Manager</Heading>
            <HStack spacing={10} mt={10} alignItems="flex-start">
                <Box w={{
                    md: "450px",
                    lg: "660px",
                    xl: "870px"
                }}
                    overflowY="scroll"
                    overflowX="hidden">
                    <SimpleGrid height="80vh"
                        w={{
                            md: "410px",
                            lg: "620px",
                            xl: "830px"
                        }}
                        columns={{
                            md: 2,
                            lg: 3,
                            xl: 4
                        }}
                        spacing={10}>
                        {deck.cards.map(card => (
                            <CardCard key={card.title} {...card} />
                        ))}
                    </SimpleGrid>
                </Box>
                <SimpleGrid
                    w={{
                        base: "200px",
                        xl: "300px"
                    }}
                    columns={1}
                    spacing={5}
                    backgroundColor="rgba(0, 0, 0, 0.3);"
                    h={"100%"}
                    overflowY="auto"
                    overflowX="hidden"
                    height="60vh">

                    {selectedDeck.cards.length === 0 ? (
                        <>
                            <Heading>No cards added</Heading>
                        </>
                    ) : (
                        selectedDeck.cards.map(card => (
                            <Text>ASDF x 2</Text>
                        ))
                    )}
                </SimpleGrid>
            </HStack>
        </Box>
    );
};

export default DeckManager;
