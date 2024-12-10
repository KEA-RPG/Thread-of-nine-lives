import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { initGame, StateGameInit } from '../hooks/useGame';
import { useToast, Text } from '@chakra-ui/react';
import { useEnemies, Enemy } from '../hooks/useEnemy';
import ListLayout from '../components/ListLayout';

const SelectionPage = () => {
    const [enemies, setEnemies] = useState<Enemy[]>([]);
    const { data, error } = useEnemies();
    const navigate = useNavigate();
    const toast = useToast()

    const onSelection = async (item: Enemy) => {
        const { data, error } = await initGame({ enemyId: item.id! } as StateGameInit);
        if (!data || error) {
            toast({
                description: 'Failed to start the game',
                status: 'error',
            });
            return;
        }

        navigate(`/combat/${data.fightId}`);
    };

    useEffect(() => {
        if (data !== undefined) {
            setEnemies(data);
        }
    }, [data]);

    if (error) {
        return <div>{error.message}</div>;
    }

    return ( 
        <ListLayout
            data={enemies}
            onSelection={(item: Enemy) => onSelection(item)}    
        /> 
    );
};

export default SelectionPage;
