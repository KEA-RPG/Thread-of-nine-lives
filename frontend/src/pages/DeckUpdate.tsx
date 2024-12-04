import { Spinner, useToast } from "@chakra-ui/react";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import DeckManager from "../components/DeckManager";
import { Deck, useDeck } from "../hooks/useDeck";


const DeckUpdate = () => {
    const toast = useToast()

    const param = useParams().deckId!;
    const value = Number(param);
    const [deck, setDeck] = useState<Deck>();
    const { data, error } = useDeck(value);
    useEffect(() => {
        if (data !== undefined) {
            setDeck(data)
        }
    }, [data])
    useEffect(() => {
        if (error != null) {
            console.log(error)
            handleError();
        }
    }, [error])
    const handleError = () => {
        let message = "Something went wrong";
        if(error?.status == 401){
            message = "You are not authorized to see the given deck "
        }
        else if(error?.status == 404)
        {
            message = "Deck not found"
        }
        toast({
            description: message,
            status: "error",
        });
    }
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

export default DeckUpdate;
