import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { initGame, StateGameInit } from '../hooks/useGame';

const SelectionPage = () => {
    const [enemyId, setEnemyId] = useState('');
    const navigate = useNavigate();

    const initializeGameState = async () => {
        try {
            await initGame( {enemyId: Number(enemyId) } as StateGameInit
            );

            console.log("Game initialized");

            // Navigate to CombatPage after initialization
            navigate('/combat');
        } catch (error) {
            console.error("Error initializing game state:", error);
        }
    };

    return (
        <div style={{ padding: '20px', textAlign: 'center' }}>
            <h1>Select Enemy</h1>
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
            
            <button onClick={initializeGameState} disabled={!enemyId}>
                Start Game
            </button>
        </div>
    );
};

export default SelectionPage;
