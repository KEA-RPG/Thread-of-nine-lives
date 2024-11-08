import { useState, useEffect } from 'react';
import { Card, useCards } from '../hooks/useCard';
import { useCombat, gameState, GameAction } from '../hooks/useGame';

const CombatPage = () => {
    const [enemyName, setEnemyName] = useState("");
    const [enemyHealth, setEnemyHealth] = useState(0);
    const [playerHealth, setPlayerHealth] = useState(0);
    const [loading, setLoading] = useState(false);
    const [currentTurn, setCurrentTurn] = useState("PLAYER");
    const [usedAttacks, setUsedAttacks] = useState([false, false, false, false, false]);
    const [cards, setCards] = useState<Card[]>([]);

    useEffect(() => {
        const fetchGameState = async () => {
            try {
                const { data } = await gameState();
                if (data !== undefined) {
                setEnemyName(data.enemyDTO.name);
                setEnemyHealth(data.enemyDTO.health);
                setPlayerHealth(data.playerDTO.health);
                }
            } catch (error) {
                console.error('Error fetching game state:', error);
            }
        };

        const fetchCards = async () => {
            try {
                const {data} = await useCards();
                if (data) {
                    setCards(data);
                }
            } catch (error) {
                console.error('Error fetching cards:', error);
            }
        };

        fetchGameState();
        fetchCards();
    }, []);

    const handleAttack = async (index: number) => {
        if (usedAttacks[index] || !cards[index]) return;
        setLoading(true);
        try {
            const card = cards[index];
            const action : GameAction = {
                type: 'ATTACK',
                value: card.attack
            };

            const response = await useCombat(action);             
            const updatedState = response.data;
            if (updatedState !== undefined) {
            setEnemyHealth(updatedState.enemyDTO.health);
            }

            const newUsedAttacks = [...usedAttacks];
            newUsedAttacks[index] = true;
            setUsedAttacks(newUsedAttacks);
            
        } catch (error) {
            console.error('Error during attack:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleEndTurn = async () => {
        if (currentTurn !== "PLAYER") return;

        setLoading(true);
        try {
            const action : GameAction = {
                type: 'END_TURN',
            };

            const response = await useCombat(action);
            const updatedState = response.data;
            if (updatedState !== undefined) {
            setPlayerHealth(updatedState.playerDTO.health);
            }
            setUsedAttacks([false, false, false, false, false]);
            setCurrentTurn("ENEMY");

            setTimeout(() => setCurrentTurn("PLAYER"), 2000);
            
        } catch (error) {
            console.error('Error during end turn:', error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ padding: '20px', textAlign: 'center' }}>
            <h1>Combat Page</h1>
            <div>
                <h2>{enemyName}</h2>
                <h2>Enemy Health: {enemyHealth}</h2>
                <h2>Player Health: {playerHealth}</h2>
                <h3>Current Turn: {currentTurn}</h3>
            </div>
            <button onClick={handleEndTurn} disabled={loading || currentTurn !== "PLAYER"}>
                {loading ? 'Ending Turn...' : 'End Turn'}
            </button>
            <div style={{ display: 'flex', justifyContent: 'center', gap: '10px', marginTop: '20px' }}>
                {cards.map((card, index) => (
                    <div
                        key={index}
                        style={{
                            width: '80px',
                            height: '120px',
                            border: '2px solid #333',
                            borderRadius: '8px',
                            backgroundColor: '#f4f4f4',
                            boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)',
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            cursor: usedAttacks[index] || loading || currentTurn !== "PLAYER" ? 'not-allowed' : 'pointer',
                        }}
                    >
                        <button
                            onClick={() => handleAttack(index)}
                            disabled={usedAttacks[index] || loading || currentTurn !== "PLAYER"}
                            style={{
                                border: 'none',
                                backgroundColor: 'transparent',
                                cursor: 'inherit',
                                fontSize: '14px',
                                fontWeight: 'bold',
                                color: usedAttacks[index] ? '#999' : '#000',
                            }}
                        >
                            {usedAttacks[index] ? 'Used' : `${card.name} Attack: ${card.attack}`}
                        </button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default CombatPage;