import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { initGame, StateGameInit } from '../hooks/useGame';
import { useToast } from '@chakra-ui/react';
import { useEnemies, Enemy } from '../hooks/useEnemy';

const SelectionPage = () => {
    const [selectedEnemyId, setSelectedEnemyId] = useState<number | null>(null);
    const { data: enemies, error } = useEnemies();
    const navigate = useNavigate();
    const toast = useToast()

    useEffect(() => {
        if (error) {
            toast({
                description: 'Failed to fetch enemies',
                status: 'error',
            });
        }
    }, [error, toast]);


    const initializeGameState = async () => {
        if (selectedEnemyId == null) {
            toast({
                description: 'Please select an enemy',
                status: 'warning',
            });
            return;
        }

        const { data, error } = await initGame({ enemyId: selectedEnemyId } as StateGameInit);
        if (data == undefined || error) {
            toast({
                description: 'Error: Failed to create combat',
                status: 'error',
            });
            return;
        }

        navigate('/combat/' + data.fightId);
    };

    return (
        <div style={{ padding: '20px', textAlign: 'center' }}>
            <h1>Select Enemy</h1>
            {enemies ? (
                <div>
                    {enemies.map((enemy: Enemy) => (
                        <label key={enemy.id} style={{ display: 'block', margin: '10px 0' }}>
                            <input
                                type="radio"
                                value={enemy.id}
                                checked={selectedEnemyId === enemy.id}
                                onChange={() => setSelectedEnemyId(enemy.id!)}
                            />
                            {enemy.name} (Health: {enemy.health})
                        </label>
                    ))}
                </div>
            ) : (
                <p>Loading enemies...</p>
            )}

            <button onClick={initializeGameState} disabled={selectedEnemyId == null}>
                Start Game
            </button>
        </div>
    );
};

export default SelectionPage;
