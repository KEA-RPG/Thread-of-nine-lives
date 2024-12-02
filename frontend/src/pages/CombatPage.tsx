import { useParams } from 'react-router-dom';
import Combat from '../components/Combat';

const CombatPage = () => {
    const fightId = Number(useParams().fightId);

    if (!fightId) {
        return <h1>Error: Invalid Fight ID</h1>;
    }

    return (
        <div>
            <Combat fightId={fightId} />
        </div>
    );
};

export default CombatPage;
