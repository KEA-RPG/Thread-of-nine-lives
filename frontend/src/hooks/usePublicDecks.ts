import { useGet, usePost } from "./useData";

export interface Deck {
  id?: number;
  userId?: number;
  name?: string;
  cards?: any[];
  userName?: string;
  isPublic?: boolean;
  comments?: Comment[];
}

export interface Comment {
  id?: number;
  deckId?: number;
  username?: string | null;
  text?: string;
  createdAt?: string;
}

export const usePublicDecks = () => useGet<Deck[]>("/decks/public");
export const postComment = (deckId: number, comment: Comment) =>
  usePost<Comment, Comment>(`/decks/${deckId}/comments`, comment);
