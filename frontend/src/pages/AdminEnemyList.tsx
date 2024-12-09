import { useNavigate } from "react-router-dom";
import ListLayout from "../components/ListLayout";
import { Enemy, useEnemies } from "../hooks/useEnemy";
import { Spinner } from "@chakra-ui/react";
import { useEffect, useState } from "react";

const AdminEnemyList = () => {
    const navigate = useNavigate();

    const onAdd = () => {
        navigate("/admin/enemies/create");
    }
    const onEdit = (item: Enemy) => {
        console.log(item);
        navigate(`/admin/enemies/${item.id}`);
    }
    const onDelete = (item: Enemy) => {
        navigate(`/admin/enemies/${item.id}`);
    }
    const [enemies, setEnemies] = useState<Enemy[]>([]);
    const { data, error } = useEnemies();
    
    useEffect(() => {
        if (data !== undefined) {
            setEnemies(data);
        }
    }, [data]);

    if (error !== null) {
        return <div>{error?.message}</div>
    }
    else if (enemies?.length === 0) {
        return <Spinner />
    }

    else {
        return <ListLayout
            data={enemies}
            onAdd={onAdd}
            onEdit={(item: Enemy) => onEdit(item)}
            onDelete={(item: Enemy) => onDelete(item)}></ListLayout>
    }
}

export default AdminEnemyList;
