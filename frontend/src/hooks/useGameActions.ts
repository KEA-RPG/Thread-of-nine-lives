import { useState } from "react";
import { useCombat } from "../hooks/useGame";

export const useGameActions = (fightId: number) => {
    const [loading, setLoading] = useState(false);

    const sendGameAction = (actionType: string, actionValue?: number) => {
        if (loading) Promise.reject("Action already in progress");

        setLoading(true);
        const action = { type: actionType, value: actionValue };
        return useCombat(fightId, action)
            .then(({ data }) => {
                return data;
            })
            .catch((error) => {
                console.error(`Error performing action: ${actionType} or ${actionValue}`, error);
                throw error;
            })
            .finally(() => {
                setLoading(false);
            });
    };

    return { sendGameAction, loading };
};
