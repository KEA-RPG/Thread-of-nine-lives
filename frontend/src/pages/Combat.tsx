import { useState, useEffect } from 'react';

const CombatPage = () => {
    const [enemyName, setEnemyName] = useState(null);
    const [enemyHealth, setEnemyHealth] = useState(null);
    const [playerAttackValue] = useState(10);
    const [playerHealth, setPlayerHealth] = useState(null);
    const [loading, setLoading] = useState(false);
    const [currentTurn, setCurrentTurn] = useState("PLAYER");
    const [usedAttacks, setUsedAttacks] = useState([false, false, false, false, false]);


    useEffect(() => {
        const fetchGameState = async () => {
            try {
                const response = await fetch('http://localhost:5281/game-state');
                if (!response.ok) {
                    throw new Error('Failed to fetch game state');
                }
                const data = await response.json();
                setEnemyName(data.enemyDTO.name);
                setEnemyHealth(data.enemyDTO.health);
                setPlayerHealth(data.playerDTO.health);
            } catch (error) {
                console.error('Error fetching game state:', error);
            }
        };

        fetchGameState();
    }, []);
    

    const handleAttack = async (index: number) => {
        if (usedAttacks[index]) return;
        setLoading(true);
        try {
            const action = {
                type: 'ATTACK',
                value: playerAttackValue,
            };

            const response = await fetch('http://localhost:5281/combat', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(action),
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const updatedState = await response.json();
            setEnemyHealth(updatedState.enemyDTO.health);

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
            const action = {
                type: 'END_TURN',
            };

            const response = await fetch('http://localhost:5281/combat', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(action),
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const updatedState = await response.json();
            setPlayerHealth(updatedState.playerDTO.health);

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
                {usedAttacks.map((used, index) => (
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
                            cursor: used || loading || currentTurn !== "PLAYER" ? 'not-allowed' : 'pointer',
                        }}
                    >
                        <button
                            onClick={() => handleAttack(index)}
                            disabled={used || loading || currentTurn !== "PLAYER"}
                            style={{
                                border: 'none',
                                backgroundColor: 'transparent',
                                cursor: 'inherit',
                                fontSize: '14px',
                                fontWeight: 'bold',
                                color: used ? '#999' : '#000',
                            }}
                        >
                            {used ? 'Used' : `Attack ${index + 1}`}
                        </button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default CombatPage;
