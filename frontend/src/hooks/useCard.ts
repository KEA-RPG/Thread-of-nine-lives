import { useGet } from "./useData";

export interface Card {
    id: number;
    name: string;
    attack: number;
}

const useCards = () => useGet<Card[]>(`/cards`)


export { useCards };