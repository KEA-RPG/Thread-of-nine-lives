import { useNavigate } from "react-router-dom";
import ListLayout from "../components/ListLayout";
import { useEffect, useState } from "react";
import { Deck, useUserDecks } from "../hooks/useDeck";

const DeckList = () => {
    const navigate = useNavigate();

    const onAdd = () => {
        navigate("/decks/create");
    }
    const onEdit = (item: Deck) => {
        console.log(item);
        navigate(`/decks/${item.id}`);
    }
    const onDelete = (item: Deck) => {
        navigate(`/decks/${item.id}`);
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
        return <ListLayout
            data={decks}
            onAdd={onAdd}
            onEdit={(item: Deck) => onEdit(item)}
            onDelete={(item: Deck) => onDelete(item)}></ListLayout>
    }
}

export default DeckList;
