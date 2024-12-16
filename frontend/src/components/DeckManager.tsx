import { Box, Button, Checkbox, Heading, HStack, SimpleGrid, Spacer, useToast, VStack } from "@chakra-ui/react";
import CardCard from "../components/GameCard";
import SelectedCardListItem from "../components/SelectedCardListItem";
import { useEffect, useState } from "react";
import { Deck, postDeck, putDeck } from "../hooks/useDeck";
import { Card, useCards } from "../hooks/useCard";
import InputFieldElement from "./InputFieldElement";
import { useNavigate } from "react-router-dom";

interface CardNumber {
    card: Card;
    count: number;
}
interface Props {
    deckModel?: Deck
}
const DeckManager = ({ deckModel }: Props) => {
    const { data } = useCards();
    const navigate = useNavigate();
    const toast = useToast();
    const [condensedDeckState, setCondensedDeckState] = useState<{ [key: number]: CardNumber }>({});

    const [cards, setCards] = useState<Card[]>([]);
    const [deck, setDeck] = useState<Deck>(
        deckModel ?? {
            name: "",
            cards: [],
            isPublic: false,
            comments: [],
            userId: 0,
        }
    );

    useEffect(() => {
        if (data !== undefined) {
            setCards(data);
        }
    }, [data]);




    useEffect(() => {
        const updatedCondensedDeck: { [key: number]: CardNumber } = {};
        for (const element of deck.cards) {
            if (updatedCondensedDeck[element.id!] === undefined) {
                updatedCondensedDeck[element.id!] = { card: element, count: 1 };
            } else {
                updatedCondensedDeck[element.id!].count++;
            }
        }
        setCondensedDeckState(updatedCondensedDeck);
    }, [deck]); 

    const getGroupedDeck = () => {
        return (
            <>
                {Object.keys(condensedDeckState).length === 0 ? (
                    <Heading>No cards added</Heading>
                ) : (
                    <SimpleGrid gap="10px" templateRows="repeat(auto-fill, 20px)" w="100%" p="10px">
                        {Object.entries(condensedDeckState).map(([key, value]) => (
                            <VStack key={key} onClick={() => removeCard(value.card)} w="100%" cursor="pointer" h="25px">
                                <SelectedCardListItem count={value.count} title={value.card.name} />
                            </VStack>
                        ))}
                    </SimpleGrid>
                )}
            </>
        );
    };

    const addCard = (card: Card) => {
        setDeck((prevDeck) => ({
            ...prevDeck,
            cards: [...prevDeck.cards, card],
        }));
    };

    const handleUpsert = async () => {
        try {
            const isNewDeck = deck.id === undefined;
            const result = isNewDeck ? await postDeck(deck) : await putDeck(deck.id!, deck);

            if (result.error) {
                toast({
                    description: `Error: Failed to ${isNewDeck ? "save" : "update"} deck`,
                    status: "error",
                });
                return;
            }

            toast({
                description: `Deck ${isNewDeck ? "created" : "updated"} successfully`,
                status: "success",
            });

            navigate("/decks");
        } catch (error) {
            toast({
                description: "Error: Failed to save deck",
                status: "error",
            });
        }
    };

    const removeCard = (card: Card) => {
        setDeck((prevDeck) => {
            const cardIndex = prevDeck.cards.indexOf(card);
            if (cardIndex === -1) {
                return prevDeck;
            } else {
                const newDeck = { ...prevDeck };
                newDeck.cards.splice(cardIndex, 1);
                return newDeck;
            }
        });
    };

    return (
        <VStack justifyItems={"center"}>
            <HStack>
                <Spacer />
                <InputFieldElement
                    type="text"
                    placeholder="Deck name"
                    name="Deck Name"
                    onChange={(e) =>
                        setDeck((prevDeck) => ({ ...prevDeck, name: e }))
                    }
                    value={deck.name}
                />
                <Button onClick={() => handleUpsert()} colorScheme="green">
                    Save deck
                </Button>
                <Checkbox
                    isChecked={deck.isPublic}
                    onChange={(e) => {
                        setDeck((prevDeck) => ({
                            ...prevDeck,
                            isPublic: e.target.checked,
                        }));
                    }}
                >
                    Is deck public?
                </Checkbox>
                <Spacer />
            </HStack>
            <HStack spacing={25} mt={10} alignItems="flex-start">
                <Box
                    w={{
                        md: "450px",
                        lg: "660px",
                        xl: "870px",
                    }}
                    overflowY="scroll"
                    overflowX="hidden"
                >
                    <SimpleGrid
                        height="70vh"
                        w={{
                            md: "410px",
                            lg: "620px",
                            xl: "830px",
                        }}
                        columns={{
                            md: 2,
                            lg: 3,
                            xl: 4,
                        }}
                        spacing={10}
                        p={1}
                    >
                        {cards.map((card) => (
                            <Box onClick={() => addCard(card)} key={card.id}>
                                <CardCard card={card} />
                            </Box>
                        ))}
                    </SimpleGrid>
                </Box>
                <SimpleGrid
                    w={{
                        base: "200px",
                        xl: "300px",
                    }}
                    columns={1}
                    spacing={5}
                    backgroundColor="rgba(0, 0, 0, 0.3);"
                    height="70vh"
                    overflowY="scroll"
                    overflowX="hidden"
                    p={1}
                >
                    <VStack align="stretch">
                        <Button
                            onClick={() =>
                                setDeck((prevDeck) => ({ ...prevDeck, cards: [] }))
                            }
                            colorScheme="red"
                        >
                            Clear Deck
                        </Button>
                        {deck.cards.length === 0 ? (
                            <Heading>No cards added</Heading>
                        ) : (
                            getGroupedDeck()
                        )}
                    </VStack>
                </SimpleGrid>
            </HStack>
        </VStack>
    );
};

export default DeckManager;
