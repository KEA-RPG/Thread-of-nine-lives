import { useEffect, useState } from "react";
import { useGameState } from "../hooks/useGame";
import { useGameActions as useGameActions } from "../hooks/useGameActions";
import { Box, Button, Text, useToast } from "@chakra-ui/react";

interface CombatProps {
    fightId: number;
}

const Combat = ({ fightId }: CombatProps) => {
    const [enemyHealth, setEnemyHealth] = useState(0);
    const [playerHealth, setPlayerHealth] = useState(0);
    const [gameOver, setGameOver] = useState(false);
    const [resultMessage, setResultMessage] = useState("");
    const { data } = useGameState(fightId);
    const { sendGameAction, loading } = useGameActions(fightId);
    const toast = useToast();

    useEffect(() => {
        if (data) {
            setEnemyHealth(data.enemyHealth);
            setPlayerHealth(data.playerHealth);
        }
    }, [data]);

    useEffect(() => {
        if (enemyHealth <= 0 && playerHealth > 0) {
            setGameOver(true);
            setResultMessage("You Win!");
            toast({ description: "Congratulations! You defeated the enemy!", status: "success" });
        } else if (playerHealth <= 0 && enemyHealth > 0) {
            setGameOver(true);
            setResultMessage("You Lose!");
            toast({ description: "You were defeated by the enemy.", status: "error" });
        }
    }, [enemyHealth, playerHealth, toast]);

    const handleGameAction = async (actionType: string, actionValue?: number) => {
        try {
            const updatedState = await sendGameAction(actionType, actionValue);
            if (updatedState) {
                setEnemyHealth(updatedState.enemyHealth);
                setPlayerHealth(updatedState.playerHealth);
            }
        } catch (error) {
            toast({ description: `Failed to perform action: ${actionType}`, status: "error" });
        }
    };

    return (
        <Box textAlign="center" p={4}>
            <Text fontSize="2xl" fontWeight="bold">Combat</Text>
            <Text fontSize="lg" mt={2}>Enemy Health: {enemyHealth}</Text>
            <Text fontSize="lg" mb={4}>Player Health: {playerHealth}</Text>

            {gameOver ? (
                <Text
                    fontSize="2xl"
                    fontWeight="bold"
                    mt={4}
                    color={
                        resultMessage === "You Win!"
                            ? "teal.500"
                            : resultMessage === "You Lose!"
                            ? "red.500"
                            : "gray.500"
                    }
                >
                    {resultMessage}
                </Text>
            ) : (
                <Box>
                    <Button
                        colorScheme="orange"
                        onClick={() => handleGameAction("ATTACK", 25)}
                        isLoading={loading}
                        mr={2}
                    >
                        Attack
                    </Button>
                    <Button
                        colorScheme="blue"
                        onClick={() => handleGameAction("END_TURN")}
                        isLoading={loading}
                    >
                        End Turn
                    </Button>
                </Box>
            )}
        </Box>
    );
};

export default Combat;
