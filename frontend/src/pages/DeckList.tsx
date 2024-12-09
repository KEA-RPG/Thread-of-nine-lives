import { useNavigate } from "react-router-dom";
import ListLayout from "../components/ListLayout";
import { useEffect, useState } from "react";
import { Deck, deleteDeck, useUserDecks } from "../hooks/useDeck";
import { useDisclosure } from "@chakra-ui/react";
import ConfirmationModal from "../components/ConfirmModal";

const DeckList = () => {
    const { onClose, isOpen, onOpen } = useDisclosure()
    const [deleteItem, setDeleteItem] = useState<Deck>({} as Deck);

    const navigate = useNavigate();

    const onAdd = () => {
        navigate("/decks/create");
    }
    const onEdit = (item: Deck) => {
        console.log(item);
        navigate(`/decks/${item.id}`);
    }
    const onDelete = (item: Deck) => {
        onOpen();
        setDeleteItem(item);
    }
    const [decks, setDecks] = useState<Deck[]>([]);
    const { data, error } = useUserDecks();

    useEffect(() => {
        if (data !== undefined) {
            setDecks(data);
        }
    }, [data]);

    if (error !== null) {
        return <div>{error?.message}</div>
    }

    else {
        return (
            <>
                <ListLayout
                    data={decks}
                    onAdd={onAdd}
                    onEdit={(item: Deck) => onEdit(item)}
                    onDelete={(item: Deck) => onDelete(item)}></ListLayout>
                <ConfirmationModal
                    isOpen={isOpen}
                    onClose={onClose}
                    item={deleteItem}
                    confirmCallback={async () => {
                        await deleteDeck(deleteItem.id!);
                    }}
                    entityName={"card"}>
                </ConfirmationModal>

            </>
        )
    }
}

export default DeckList;
