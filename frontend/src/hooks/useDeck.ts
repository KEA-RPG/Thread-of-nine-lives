import ApiClient from "../services/apiClient";
import { Card } from "./useCard";

export interface Deck {
    id?: number;
    name: string;
    userId: number;
    cards: Card[];
    isPublic: boolean;
    comments: Comment[];
}

export interface Comment {
    id?: number;
    deckId: number;
    text: string;
    createdAt: Date;
}
const apiClient = new ApiClient();

const useUserDecks = () => apiClient.get<Deck[]>(`/decks`);
const usePublicDecks = () => apiClient.get<Deck[]>(`/decks/public`)
const useDeck = (id: number) => apiClient.get<Deck>(`/decks/${id}`);
const useDeckComments = (id: number) => apiClient.get<Comment[]>(`/decks/${id}/comments`);
const postComment = (comment: Comment, id: number) => apiClient.post<Comment, Deck>(`/decks/${id}/comments`, comment);
const postDeck = (Deck: Deck) => apiClient.post<Deck, Deck>(`/decks`, Deck);
const deleteDeck = (id: number) => apiClient.delete<Deck>(`/decks/${id}`);
const putDeck = (id: number, Deck: Deck) => apiClient.put<Deck, Deck>(`/decks/${id}`, Deck);

export { useUserDecks, usePublicDecks, useDeck, useDeckComments, postComment, deleteDeck, postDeck, putDeck }