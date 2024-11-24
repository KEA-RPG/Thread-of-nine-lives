// CommentForm.tsx
import React, { useState } from "react";
import {
  Box,
  FormControl,
  FormLabel,
  Textarea,
  Button,
  useToast,
} from "@chakra-ui/react";
import { Comment, postComment } from "../hooks/usePublicDecks";
import { useUserContext } from "./UserContext";

interface CommentFormProps {
  deckId: number;
  // Remove onCommentAdded prop since it's no longer needed
  // onCommentAdded: (comment: Comment) => void;
}

const CommentForm: React.FC<CommentFormProps> = ({ deckId }) => {
  const [commentText, setCommentText] = useState("");
  const { token, username } = useUserContext();
  const toast = useToast();

  if (!token || !username) {
    return null;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!commentText.trim()) {
      toast({
        title: "Error",
        description: "Comment cannot be empty.",
        status: "error",
        duration: 5000,
        isClosable: true,
      });
      return;
    }

    const newComment: Comment = {
      text: commentText,
      username: username,
    };

    try {
      const response = await postComment(deckId, newComment);
      if (response.data) {
        setCommentText("");

        // Show success toast
        toast({
          title: "Comment Posted",
          description: "Your comment has been posted successfully.",
          status: "success",
          duration: 5000,
          isClosable: true,
        });

        // Do not call onCommentAdded, so the comment won't appear immediately
        // onCommentAdded(response.data);
      } else if (response.error) {
        toast({
          title: "Error",
          description: response.error,
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      } else {
        toast({
          title: "Error",
          description: "An unexpected error occurred.",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      }
    } catch (err: any) {
      toast({
        title: "Error",
        description:
          err.message || "An error occurred while submitting your comment.",
        status: "error",
        duration: 5000,
        isClosable: true,
      });
    }
  };

  return (
    <Box mt={4}>
      <form onSubmit={handleSubmit}>
        <FormControl>
          <FormLabel>Add a Comment</FormLabel>
        </FormControl>
        <Textarea
          value={commentText}
          onChange={(e) => setCommentText(e.target.value)}
          placeholder="Enter your comment"
        />
        <Button mt={2} type="submit">
          Submit
        </Button>
      </form>
    </Box>
  );
};

export default CommentForm;
