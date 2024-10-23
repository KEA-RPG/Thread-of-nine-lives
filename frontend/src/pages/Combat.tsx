import { useState, useEffect } from 'react';

const CombatPage = () => {
    const [enemyName, setEnemyName] = useState(null);
    const [enemyHealth, setEnemyHealth] = useState(null);
    const [playerAttackValue] = useState(10);
    const [playerHealth, setPlayerHealth] = useState(null);
    const [loading, setLoading] = useState(false);
    const [currentTurn] = useState("PLAYER");

    useEffect(() => {
        const fetchGameState = async () => {
            try {
                const response = await fetch('http://localhost:5281/game-state');
                if (!response.ok) {
                    throw new Error('Failed to fetch game state');
                }
                const data = await response.json();
                setEnemyName(data.enemy.name);
                setEnemyHealth(data.enemy.health);
                setPlayerHealth(data.player.health);
            } catch (error) {
                console.error('Error fetching game state:', error);
            }   
        };

        fetchGameState();
    }, []);

    const handleAttack = async () => {
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

            if (updatedState.enemy && typeof updatedState.enemy.health === 'number') {
                setEnemyHealth(updatedState.enemy.health);
            } else {
                console.error('Unexpected response structure:', updatedState);
            }
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

            setPlayerHealth(updatedState.player.health);
            
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
            <button onClick={handleAttack} disabled={loading}>
                {loading ? 'Attacking...' : 'ATTACK'}
            </button>
            <button onClick={handleEndTurn} disabled={loading || currentTurn !== "PLAYER"}>
                {loading ? 'Ending Turn...' : 'End Turn'}
            </button>
        </div>
    );
};

export default CombatPage;
