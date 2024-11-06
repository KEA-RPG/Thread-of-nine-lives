import { useGet, usePost, usePut } from "./useData";

export interface Card {
    id?: number;
    name: string;
    description: string;
    attack: number;
    defence: number;
    cost: number;
    imagePath: string;
}

const useCards = () => useGet<Card[]>(`/cards`)
const useCard = (id: number) => useGet<Card>(`/cards/${id}`)
const postCard = (enemy: Card) => usePost<Card, Card>(`/cards`,enemy);
const putCard = (id: number,enemy: Card) => usePut<Card, Card>(`/cards/${id}`,enemy); 

export { useCards, useCard, postCard,putCard };