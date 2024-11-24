// CommentForm.tsx
import React, { useState } from "react";
import {
  Box,
  FormControl,
  FormLabel,
  Textarea,
  Button,
  Text,
} from "@chakra-ui/react";
import { Comment, postComment } from "../hooks/usePublicDecks";
import { useUserContext } from "./UserContext";

interface CommentFormProps {
  deckId: number;
  onCommentAdded: (comment: Comment) => void;
}

const CommentForm: React.FC<CommentFormProps> = ({ deckId, onCommentAdded }) => {
  const [commentText, setCommentText] = useState("");
  const [error, setError] = useState<string | null>(null);
  const { token } = useUserContext();

  if (!token) {
    // User is not logged in, do not display the form
    return null;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!commentText.trim()) {
      setError("Comment cannot be empty.");
      return;
    }

    const newComment: Comment = {
      text: commentText,
    };

    try {
      const response = await postComment(deckId, newComment);
      if (response.data) {
        onCommentAdded(response.data);
        setCommentText("");
        setError(null);
      } else if (response.error) {
        setError(response.error);
      } else {
        setError("An unexpected error occurred.");
      }
    } catch (err: any) {
      setError(err.message || "An error occurred while submitting your comment.");
    }
  };

  return (
    <Box mt={4}>
      <form onSubmit={handleSubmit}>
        <FormControl>
          <FormLabel>Add a Comment</FormLabel>
          <Textarea
            value={commentText}
            onChange={(e) => setCommentText(e.target.value)}
            placeholder="Enter your comment"
          />
        </FormControl>
        {error && (
          <Text color="red.500" mt={2}>
            {error}
          </Text>
        )}
        <Button mt={2} type="submit">
          Submit
        </Button>
      </form>
    </Box>
  );
};

export default CommentForm;
