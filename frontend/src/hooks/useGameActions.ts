import { useState } from "react";
import { useCombat } from "../hooks/useGame";

export const useGameActions = (fightId: number) => {
    const [loading, setLoading] = useState(false);

    const sendGameAction = async (actionType: string, actionValue?: number) => {
        if (loading) return;

        setLoading(true);
        try {
            const action = { type: actionType, value: actionValue };
            const { data } = await useCombat(fightId, action);
            return data; 
        } catch (error) {
            console.error(`Error performing action: ${actionType} or ${actionValue}`, error);
            throw error;
        } finally {
            setLoading(false);
        }
    };

    return { sendGameAction, loading };
};
