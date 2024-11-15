import { Spinner } from "@chakra-ui/react";
import { Card, useCard } from "../hooks/useCard";
import { useParams } from "react-router-dom";
import CardUpsert from "./CardUpsert";
import { useEffect, useState } from "react";


const CardUpdate = () => {
    const param = useParams().cardId!;
    const value = Number(param);
    const [card, setCard] = useState<Card>();
    const { data } = useCard(value);
    useEffect(() => {
        if (data !== undefined) {
            setCard(data)
        }
    }, [data])

    return (
        <>
            {card === undefined ? (
                <Spinner />
            ) : (
                <CardUpsert cardModel={card} />
            )}
        </>
    );
}

export default CardUpdate;
