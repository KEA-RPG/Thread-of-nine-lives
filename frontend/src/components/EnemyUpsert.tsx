import { Box, HStack, VStack, Image, Button, useToast } from "@chakra-ui/react";
import { Enemy, postEnemy, putEnemy } from "../hooks/useEnemy";
import { useState } from "react";
import InputFieldElement from "./InputFieldElement";
import { useNavigate } from "react-router-dom";

type Props = {
    data?: Enemy;
};

const EnemyUpsert = ({ data }: Props) => {
    const navigate = useNavigate();
    const toast = useToast()

    const [enemy, setEnemy] = useState<Enemy>(data ?? {} as Enemy);
    const handleUpsert = async () => {
        try {
            const isNewEnemy = enemy.id === undefined;
            const result = isNewEnemy 
                ? await postEnemy(enemy)
                : await putEnemy(enemy.id!, enemy);
            if (result.error) {
                toast({
                    description: `Error: Failed to ${isNewEnemy ? 'save' : 'update'} enemy`,
                    status: "error",
                });
                return;
            }

            toast({
                description: `Enemy ${isNewEnemy ? 'created' : 'updated'} successfully`,
                status: "success",
            });

            navigate('/admin/enemies');
        } catch (error) {
            toast({
                description: "Error: Failed to save enemy",
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
                            value={enemy.name}
                            onChange={(name) => setEnemy({ ...enemy, name })}
                        />

                        <InputFieldElement
                            type="text"
                            name="Health"
                            placeholder="Health"
                            value={enemy.health?.toString()}
                            onChange={(health) => setEnemy({ ...enemy, health: Number(health) })}
                        />

                        <InputFieldElement
                            type="text"
                            name="ImagePath"
                            placeholder="ImagePath"
                            value={enemy.imagePath}
                            onChange={(imagePath) => setEnemy({ ...enemy, imagePath })}
                        />
                    </Box>
                    <Box>
                        <Image src="https://loremflickr.com/320/240" border="1px solid black" w="160px" h="160px" />
                        <p>Name: {enemy.name}</p>
                        <p>Health: {enemy.health}</p>
                        <p>Image: {enemy.imagePath}</p>
                    </Box>
                </HStack>
                <Button colorScheme="orange" onClick={handleUpsert}>
                    {data === undefined ? "Create" : "Update"}
                </Button>
            </VStack>
        </Box>
    );
}

export default EnemyUpsert;
