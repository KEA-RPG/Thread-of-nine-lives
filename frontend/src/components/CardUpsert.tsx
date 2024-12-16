import { Box, HStack, VStack, Button, useToast } from "@chakra-ui/react";
import InputFieldElement from "../components/InputFieldElement";
import { Card, postCard, putCard } from "../hooks/useCard";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import GameCard from "./GameCard";

interface Props {
    cardModel?: Card;
}
const CardUpsert = ({ cardModel }: Props) => {
    const navigate = useNavigate();
    const toast = useToast()

    const [card, setCard] = useState<Card>(cardModel ?? {
        name: '',
        description: '',
        attack: 0,
        defence: 0,
        cost: 0,
        imagePath: ''
    });
    const handleUpsert = async () => {
        try {
            const isNewCard = card.id === undefined;
            const result = isNewCard 
                ? await postCard(card)
                : await putCard(card.id!, card);
            if (result.error) {
                toast({
                    description: `Error: Failed to ${isNewCard ? 'save' : 'update'} card`,
                    status: "error",
                });
                return;
            }

            toast({
                description: `Card ${isNewCard ? 'created' : 'updated'} successfully`,
                status: "success",
            });

            navigate('/admin/cards');
        } catch (error) {
            toast({
                description: "Error: Failed to save card",
                status: "error",
            });
        }
    }

    return (
        <Box backgroundColor="white" color="lightgray" py="20px" px="25px" rounded="10px" mt="20px">
            <VStack>
                <HStack spacing='24px'>
                    <Box>
                        <InputFieldElement
                            type="text"
                            name="Name"
                            placeholder="Name"
                            value={card.name}
                            onChange={(name) => setCard({ ...card, name })}
                        />

                        <InputFieldElement
                            type="text"
                            name="Description"
                            placeholder="Description"
                            value={card.description}
                            onChange={(description) => setCard({ ...card, description })}
                        />

                        <InputFieldElement
                            type="number"
                            name="Attack"
                            placeholder="Attack"
                            value={card.attack?.toString()}
                            onChange={(attack) => setCard({ ...card, attack: Number(attack) })}
                        />

                        <InputFieldElement
                            type="number"
                            name="Cost"
                            placeholder="Cost"
                            value={card.cost?.toString()}
                            onChange={(cost) => setCard({ ...card, cost: Number(cost) })}
                        />

                        <InputFieldElement
                            type="number"
                            name="Defence"
                            placeholder="Defence"
                            value={card.defence?.toString()}
                            onChange={(defence) => setCard({ ...card, defence: Number(defence) })}
                        />
                        <InputFieldElement
                            type="text"
                            name="ImagePath"
                            placeholder="ImagePath"
                            value={card.imagePath}
                            onChange={(imagePath) => setCard({ ...card, imagePath })}
                        />
                    </Box>
                    <Box>
                        <p>Preview:</p>
                        <br/>
                        <GameCard card={card} />
                        <br/>

                    </Box>
                </HStack>
                <Button colorScheme="orange" onClick={handleUpsert}>
                    {card.id === undefined ? "Create" : "Update"}
                </Button>
            </VStack>
        </Box>

    )
}

export default CardUpsert;
