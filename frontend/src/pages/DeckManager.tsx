import { Box, Button, Heading, HStack, SimpleGrid, VStack } from "@chakra-ui/react";
import CardCard, { Card } from "../components/GameCard";
import SelectedCardListItem from "../components/SelectedCardListItem";
import { useState } from "react";

interface CardNumber {
    card: Card;
    count: number;
}
const DeckManager = () => {
    const [cardDeck, setCardDeck] = useState<Card[]>([]);

    const condensedDeck: { [key: number]: CardNumber } = {};
    const getGroupedDeck = () => {
        for (let index = 0; index < cardDeck.length; index++) {
            const element = cardDeck[index];
            if (condensedDeck[element.id] === undefined) {
                condensedDeck[element.id] = { card: element, count: 1 };
            }
            else {
                condensedDeck[element.id].count++;
            }
        }
        return (
            <>
                {Object.keys(condensedDeck).length === 0 ? (
                    <>
                        <Heading>No cards added</Heading>
                    </>
                ) : (
                    <SimpleGrid gap="10px" templateRows="repeat(auto-fill, 20px)" w="100%" p="10px" >
                        {Object.entries(condensedDeck).map(([key, value]) => (
                            <VStack key={key} onClick={() => removeCard(value.card)} w="100%" cursor="pointer" h="25px" >

                                <SelectedCardListItem count={value.count} title={value.card.title} />
                            </VStack>
                        ))}
                    </SimpleGrid>

                )}
            </>
        );

    };
    let cards: Card[] = [
        { id: 1, title: "no attack", image_path: "https://loremflickr.com/320/240", content: "This is card 1", defence: 5 },
        { id: 2, title: "no defence", image_path: "https://loremflickr.com/320/241", content: "This is card 2", attack: 8, },
        { id: 3, title: "no nothing", image_path: "https://loremflickr.com/320/242", content: "This is card 3" },
        { id: 4, title: "Card 4", image_path: "https://loremflickr.com/320/243", content: "This is card 4", attack: 6, defence: 8 },
        { id: 5, title: "Card 5", image_path: "https://loremflickr.com/320/244", content: "This is card 5", attack: 9, defence: 5 },
        { id: 6, title: "Card 6", image_path: "https://loremflickr.com/320/245", content: "This is card 6", attack: 5, defence: 9 },
        { id: 7, title: "Card 7", image_path: "https://loremflickr.com/320/246", content: "This is card 7", attack: 10, defence: 4 },
        { id: 8, title: "Card 8", image_path: "https://loremflickr.com/320/247", content: "This is card 8", attack: 3, defence: 7 },
        { id: 9, title: "Card 9", image_path: "https://loremflickr.com/320/248", content: "This is card 9", attack: 4, defence: 6 },
        { id: 10, title: "Card 10", image_path: "https://loremflickr.com/320/249", content: "This is card 10", attack: 6, defence: 5 },
    ];

    const addCard = (card: Card) => {
        setCardDeck(prevDeck => [...prevDeck, card]);
    };

    const removeCard = (card: Card) => {
        setCardDeck(prevDeck => {
            const cardIndex = prevDeck.indexOf(card);
            if (cardIndex === -1) {
                return prevDeck;
            }
            else {
                let newdeck = [...prevDeck];
                newdeck.splice(cardIndex, 1)

                return newdeck

            }
        })
    }
    return (

        <HStack spacing={25} mt={10} alignItems="flex-start">
            <Box w={{
                md: "450px",
                lg: "660px",
                xl: "870px"
            }}
                overflowY="scroll"
                overflowX="hidden">
                <SimpleGrid height="70vh"
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
                    spacing={10}
                    p={1}>
                    {cards.map(card => (
                        <Box onClick={() => addCard(card)} key={card.id}>
                            <CardCard card={card} />
                        </Box>
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
                height="70vh"
                overflowY="scroll"
                overflowX="hidden"
                p={1}>

                <VStack align="stretch">
                    <Button onClick={() => setCardDeck([]) } colorScheme='red'>Clear Deck</Button>
                    {cardDeck.length === 0 ? (
                        <Heading>No cards added</Heading>
                    ) : (
                        getGroupedDeck()
                    )}
                </VStack>
            </SimpleGrid>
        </HStack>
    );
};

export default DeckManager;
