import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { initGame, StateGameInit } from '../hooks/useGame';
import { useToast, Spinner } from '@chakra-ui/react';
import { useEnemies, Enemy } from '../hooks/useEnemy';
import ListLayout from '../components/ListLayout';

const SelectionPage = () => {
    const [selectedEnemy, setSelectedEnemy] = useState<Enemy | null>(null);
    const [enemies, setEnemies] = useState<Enemy[]>([]);
    const { data, error } = useEnemies();
    const navigate = useNavigate();
    const toast = useToast()

    const onSelection = (item: Enemy) => {
        setSelectedEnemy(item);
    };

    const initializeGameState = async () => {
        if (!selectedEnemy) {
            toast({
                description: 'Please select an enemy',
                status: 'warning',
            });
            return;
        }

        const { data, error } = await initGame({ enemyId: selectedEnemy.id! } as StateGameInit);
        if (!data || error) {
            toast({
                description: 'Error: Failed to create combat',
                status: 'error',
            });
            return;
        }

        navigate('/combat/' + data.fightId);
    };

    useEffect(() => {
        if (data !== undefined) {
            setEnemies(data);
        }
    }, [data]);

    if (error) {
        return <div>{error.message}</div>;
    }

    if (enemies.length === 0) {
        return <Spinner />;
    }

    return (
        <div style={{ padding: '20px', textAlign: 'center' }}>
            <h1>Select Enemy</h1>
            <ListLayout
                data={enemies}
                onSelection={(item: Enemy) => onSelection(item)}
            />
            <button onClick={initializeGameState} disabled={!selectedEnemy}>
                Start Game
            </button>
        </div>
    );
};

export default SelectionPage;
