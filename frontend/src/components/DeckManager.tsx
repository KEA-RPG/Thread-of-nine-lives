import { Box, Heading, HStack, SimpleGrid, VStack } from "@chakra-ui/react";
import CardCard, { CardProp } from "./CardCard";
import SelectedCardListItem from "./SelectedCardListItem";
import { useState } from "react";

interface CardNumber {
    card: CardProp;
    count: number;
}
const DeckManager = () => {
    const [cardDeck, setCardDeck] = useState<CardProp[]>([]);

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
    let cards: CardProp[] = [
        { id: 1, title: "Card 1", image_path: "https://loremflickr.com/320/240", content: "This is card 1", defence: 5 },
        { id: 2, title: "Card 2", image_path: "https://loremflickr.com/320/241", content: "This is card 2", attack: 8, },
        { id: 3, title: "Card 3", image_path: "https://loremflickr.com/320/242", content: "This is card 3"},
        { id: 4, title: "Card 4", image_path: "https://loremflickr.com/320/243", content: "This is card 4", attack: 6, defence: 8 },
        { id: 5, title: "Card 5", image_path: "https://loremflickr.com/320/244", content: "This is card 5", attack: 9, defence: 5 },
        { id: 6, title: "Card 6", image_path: "https://loremflickr.com/320/245", content: "This is card 6", attack: 5, defence: 9 },
        { id: 7, title: "Card 7", image_path: "https://loremflickr.com/320/246", content: "This is card 7", attack: 10, defence: 4 },
        { id: 8, title: "Card 8", image_path: "https://loremflickr.com/320/247", content: "This is card 8", attack: 3, defence: 7 },
        { id: 9, title: "Card 9", image_path: "https://loremflickr.com/320/248", content: "This is card 9", attack: 4, defence: 6 },
        { id: 10, title: "Card 10", image_path: "https://loremflickr.com/320/249", content: "This is card 10", attack: 6, defence: 5 },
        { id: 11, title: "Card 11", image_path: "https://loremflickr.com/322/241", content: "This is card 11", attack: 8, defence: 3 },
        { id: 12, title: "Card 12", image_path: "https://loremflickr.com/323/242", content: "This is card 12", attack: 5, defence: 10 },
        { id: 13, title: "Card 13", image_path: "https://loremflickr.com/324/243", content: "This is card 13", attack: 7, defence: 4 },
        { id: 14, title: "Card 14", image_path: "https://loremflickr.com/325/244", content: "This is card 14", attack: 9, defence: 3 },
        { id: 15, title: "Card 15", image_path: "https://loremflickr.com/326/245", content: "This is card 15", attack: 6, defence: 7 },
        { id: 16, title: "Card 16", image_path: "https://loremflickr.com/327/246", content: "This is card 16", attack: 4, defence: 9 },
        { id: 17, title: "Card 17", image_path: "https://loremflickr.com/328/247", content: "This is card 17", attack: 8, defence: 5 },
        { id: 18, title: "Card 18", image_path: "https://loremflickr.com/329/248", content: "This is card 18", attack: 6, defence: 4 },
        { id: 19, title: "Card 19", image_path: "https://loremflickr.com/321/249", content: "This is card 19", attack: 9, defence: 7 },
        { id: 20, title: "Card 20", image_path: "https://loremflickr.com/320/240", content: "This is card 20", attack: 7, defence: 6 }
    ];

    const addCard = (card: CardProp) => {
        setCardDeck(prevDeck => [...prevDeck, card]);
    };

    const removeCard = (card: CardProp) => {
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
                        {cards.map(card => (
                            <Box onClick={() => addCard(card)} key={card.id}>
                                <CardCard {...card} />
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
                    height="80vh"
                    overflowY="scroll"
                    overflowX="hidden">

                    {cardDeck.length === 0 ? (
                        <>
                            <Heading>No cards added</Heading>
                        </>
                    ) : (
                        <>
                            {getGroupedDeck()}
                        </>
                    )
                    }
                </SimpleGrid>
            </HStack>
        </Box>
    );
};

export default DeckManager;
