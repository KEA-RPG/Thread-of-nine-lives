import { useNavigate } from "react-router-dom";
import ListLayout from "../components/ListLayout";
import { Spinner } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { Card, useCards } from "../hooks/useCard";

const AdminCardList = () => {
    const navigate = useNavigate();

    const onAdd = () => {
        navigate("/admin/cards/create");
    }
    const onEdit = (item: Card) => {
        console.log(item);
        navigate(`/admin/cards/${item.id}`);
    }
    const onDelete = (item: Card) => {
        navigate(`/admin/cards/${item.id}`);
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
        return <ListLayout
            data={cards}
            onAdd={onAdd}
            onEdit={(item: Card) => onEdit(item)}
            onDelete={(item: Card) => onDelete(item)}></ListLayout>
    }
}

export default AdminCardList;
