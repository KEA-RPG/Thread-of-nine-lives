import { useNavigate } from "react-router-dom";
import ListLayout from "../components/ListLayout";
import { Spinner, useDisclosure } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { Card, deleteCard, useCards } from "../hooks/useCard";
import ConfirmationModal from "../components/ConfirmModal";

const AdminCardList = () => {
    const { onClose,isOpen, onOpen } = useDisclosure()
    const [deleteItem, setDeleteItem] = useState<Card>({} as Card);
    
    const navigate = useNavigate();

    const onAdd = () => {
        navigate("/admin/cards/create");
    }
    const onEdit = (item: Card) => {
        navigate(`/admin/cards/${item.id}`);
    }
    const onDelete = (item: Card) => {
        onOpen();
        setDeleteItem(item);
    }
    const [cards, setCards] = useState<Card[]>([]);
    const { data, error } = useCards();

    useEffect(() => {
        if (data !== undefined) {
            setCards(data);
        }
    }, [data]);

    if (error !== null) {
        return <div>{error?.message}</div>
    }
    else if (cards?.length === 0) {
        return <Spinner />
    }

    else {
        return (
            <>
                <ListLayout
                    data={cards}
                    onAdd={onAdd}
                    onEdit={(item: Card) => onEdit(item)}
                    onDelete={(item: Card) => onDelete(item)}>

                </ListLayout>
                <ConfirmationModal
                    isOpen={isOpen}
                    onClose={onClose}
                    item={deleteItem}
                    confirmCallback={async () => {
                        await deleteCard(deleteItem.id!);
                    }}
                    entityName={"card"}>
                </ConfirmationModal>
            </>
        )
    }
}

export default AdminCardList;
