import { useParams } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { useCombat } from '../hooks/useGame';

const CombatPage = () => {
    const { id: fightId } = useParams<{ id: string }>();
    const [enemyName, setEnemyName] = useState("");
    const [enemyHealth, setEnemyHealth] = useState(0);
    const [playerHealth, setPlayerHealth] = useState(0);
    const [currentTurn, setCurrentTurn] = useState("PLAYER");
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        // Fetch initial state when the page loads
        const fetchInitialState = async () => {
            try {
                const { data } = await useCombat(Number(fightId));
                if (data) {
                    setEnemyName(data.EnemyName);
                    setEnemyHealth(data.EnemyHealth);
                    setPlayerHealth(data.PlayerHealth);
                }
            } catch (error) {
                console.error("Error fetching initial state:", error);
            }
        };
        fetchInitialState();
    }, [fightId]);

    const sendGameAction = async (actionType: string) => {
        if (loading) return;

        setLoading(true);
        try {
            const action = { type: actionType };
            const { data } = await useCombat(action);
            if (data) {
                setEnemyHealth(data.EnemyHealth);
                setPlayerHealth(data.PlayerHealth);
            }
        } catch (error) {
            console.error(`Error performing action: ${actionType}`, error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h1>Combat Page</h1>
            <h2>Enemy Name: {enemyName}</h2>
            <h2>Enemy Health: {enemyHealth}</h2>
            <h2>Player Health: {playerHealth}</h2>
            <h3>Current Turn: {currentTurn}</h3>

            <div>
                <button
                    onClick={() => sendGameAction("ATTACK")}
                    disabled={loading || currentTurn !== "PLAYER"}
                >
                    Attack
                </button>
                <button
                    onClick={() => sendGameAction("END_TURN")}
                    disabled={loading || currentTurn !== "PLAYER"}
                >
                    End Turn
                </button>
            </div>
        </div>
    );
};

export default CombatPage;
