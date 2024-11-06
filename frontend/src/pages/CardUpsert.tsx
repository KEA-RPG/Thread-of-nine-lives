import { Box, HStack, VStack, Image, Button, useToast } from "@chakra-ui/react";
import InputFieldElement from "../components/InputFieldElement";
import { Card, useCard, postCard, putCard } from "../hooks/useCard";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";


const CardUpsert = () => {
    const navigate = useNavigate();
    const toast = useToast()

    const param = useParams().cardId;
    const [card, setCard] = useState<Card>({id: undefined, name: '', description: '', attack: 0, defence: 0, cost: 0, imagePath: ''});
    const value = param !== undefined && !isNaN(Number(param)) ? Number(param) : null;

    const handleUpsert = () => { 
        let toastMessage = "";
        if (card.id === undefined) {
            postCard(card);
            toastMessage = "Card created";
        }
        else {
            putCard(card.id, card);
            toastMessage = "Card updated";
        }
        toast({
            description: toastMessage,
            status: "success",
        })
        navigate('/admin/cards');


    }
    useEffect(() => {
        const useFetchCardData = async () => {
            if (value !== null) {
                const cardData = (await useCard(value)).data;
                if (cardData) {
                    setCard(cardData);
                }
            }
        };
    
        useFetchCardData();
    }, []);
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
                        <Image src="https://loremflickr.com/320/240" border="1px solid black" w="160px" h="160px" />
                        <p>Name: {card.name}</p>
                        <p>Description: {card.description}</p>
                        <p>Attack: {card.attack}</p>
                        <p>Defence: {card.defence}</p>
                        <p>Cost: {card.cost}</p>
                        <p>Image: {card.imagePath}</p>
                    </Box>
                </HStack>
                <Button colorScheme="orange" onClick={handleUpsert}>
                    {param === undefined ? "Create" : "Update"}
                </Button>
            </VStack>
        </Box>

    )
}

export default CardUpsert;
