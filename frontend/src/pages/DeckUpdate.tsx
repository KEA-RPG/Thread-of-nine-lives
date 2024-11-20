import { Spinner } from "@chakra-ui/react";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import DeckManager from "../components/DeckManager";


const DeckUpdate = () => {
    const param = useParams().deckId!;
    const value = Number(param);
    const [deck, setDeck] = useState<Deck>();
    const { data } = useDeck(value);
    useEffect(() => {
        if (data !== undefined) {
            setDeck(data)
        }
    }, [data])

    return (
        <>
            {deck === undefined ? (
                <Spinner />
            ) : (
                <DeckManager deckModel={deck} />
            )}
        </>
    );
}

export default CardUpdate;
