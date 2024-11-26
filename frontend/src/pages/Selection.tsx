import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { initGame, StateGameInit } from '../hooks/useGame';
import { useToast } from '@chakra-ui/react';

const SelectionPage = () => {
    const [enemyId, setEnemyId] = useState('');
    const navigate = useNavigate();
    const toast = useToast()

    const initializeGameState = async () => {

        const { data, error } = await initGame({ enemyId: Number(enemyId) } as StateGameInit );
        if (data == undefined || error) {
            toast({
                description: `Error: Failed to create combat`,
                status: "error",
            });
            return;
        }
        console.log("Game initialized");
        console.log(data);
        console.log('/combat/' + data.fightId)
        // Navigate to CombatPage after initialization
        navigate('/combat/' + data.fightId);
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
