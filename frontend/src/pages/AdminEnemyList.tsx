import { useNavigate } from "react-router-dom";
import ListLayout from "../components/ListLayout";
import { deleteEnemy, Enemy, useEnemies } from "../hooks/useEnemy";
import { Spinner, useDisclosure } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import ConfirmationModal from "../components/ConfirmModal";

const AdminEnemyList = () => {
    const { onClose, isOpen, onOpen } = useDisclosure()
    const [deleteItem, setDeleteItem] = useState<Enemy>({} as Enemy);

    const navigate = useNavigate();

    const onAdd = () => {
        navigate("/admin/enemies/create");
    }
    const onEdit = (item: Enemy) => {
        navigate(`/admin/enemies/${item.id}`);
    }
    const onDelete = (item: Enemy) => {
        onOpen();
        setDeleteItem(item);
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
        return (<><ListLayout
            data={enemies}
            onAdd={onAdd}
            onEdit={(item: Enemy) => onEdit(item)}
            onDelete={(item: Enemy) => onDelete(item)}></ListLayout>
            <ConfirmationModal
                isOpen={isOpen}
                onClose={onClose}
                item={deleteItem}
                confirmCallback={async () => {
                    await deleteEnemy(deleteItem.id!);
                }}
                entityName={"card"}>
            </ConfirmationModal>
        </>);
    }
}

export default AdminEnemyList;
