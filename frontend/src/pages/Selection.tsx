import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const SelectionPage = () => {
    const [enemyId, setEnemyId] = useState('');
    const [playerId, setPlayerId] = useState('');
    const navigate = useNavigate();

    const initializeGameState = async () => {
        try {
            const response = await fetch(`http://localhost:5281/init-game-state?enemyId=${enemyId}&playerId=${playerId}`, {
                method: 'POST',
            });

            if (!response.ok) {
                throw new Error('Failed to initialize game state');
            }

            await response.json();
            console.log("Game initialized");

            // Navigate to CombatPage after initialization
            navigate('/combat');
        } catch (error) {
            console.error("Error initializing game state:", error);
        }
    };

    return (
        <div style={{ padding: '20px', textAlign: 'center' }}>
            <h1>Select Enemy and Player</h1>
            <div>
                <label>
                    Enemy ID:
                    <input
                        type="number"
                        value={enemyId}
                        onChange={(e) => setEnemyId(e.target.value)}
                        placeholder="Enter enemy ID"
                    />
                </label>
            </div>
            <div>
                <label>
                    Player ID:
                    <input
                        type="number"
                        value={playerId}
                        onChange={(e) => setPlayerId(e.target.value)}
                        placeholder="Enter player ID"
                    />
                </label>
            </div>
            <button onClick={initializeGameState} disabled={!enemyId || !playerId}>
                Start Game
            </button>
        </div>
    );
};

export default SelectionPage;
