import ApiClient from "../services/apiClient";
import { ListItemBase } from "./useData";

export interface Card extends ListItemBase {
    id?: number;
    name: string;
    description: string;
    attack: number;
    defence: number;
    cost: number;
    imagePath: string;
}
const apiClient = new ApiClient();

const useCards = () => apiClient.get<Card[]>(`/cards`);
const useCard = (id: number) => apiClient.get<Card>(`/cards/${id}`)
const postCard = (car: Card) => apiClient.post<Card, Card>(`/cards`, car);
const putCard = (id: number, card: Card) => apiClient.put<Card, Card>(`/cards/${id}`, card);

export { useCards, useCard, postCard, putCard };