import { useEffect, useState } from 'react';
import { useCombat, useGameState } from '../hooks/useGame';

interface CombatProps {
    fightId: number;
}

const Combat = ({ fightId }: CombatProps) => {
    const [enemyHealth, setEnemyHealth] = useState(0);
    const [playerHealth, setPlayerHealth] = useState(0);
    const [loading, setLoading] = useState(false);
    const { data } = useGameState(fightId);
    useEffect(() => {
        if (data !== undefined) {
            
        if (data) {
            setEnemyHealth(data.enemyHealth);
            setPlayerHealth(data.playerHealth);
        }

        }
    }, [data])


    const sendGameAction = async (actionType: string, actionValue?: number) => {
        if (loading) return;

        setLoading(true);
        try {
            const action = { type: actionType, value: actionValue };
            const { data } = await useCombat(fightId, action);

            if (data) {
                setEnemyHealth(data.enemyHealth);
                setPlayerHealth(data.playerHealth);
            }
        } catch (error) {
            console.error(`Error performing action: ${actionType} or ${actionValue}`, error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h2>Enemy Health: {enemyHealth}</h2>
            <h2>Player Health: {playerHealth}</h2>

            <div>
                <button
                    onClick={() => sendGameAction("ATTACK", 10)} // Sending a value of 10 with the attack action
                    disabled={loading}
                >
                    Attack
                </button>
                <button
                    onClick={() => sendGameAction("END_TURN")}
                    disabled={loading}
                >
                    End Turn
                </button>
            </div>
        </div>
    );
};

export default Combat;
