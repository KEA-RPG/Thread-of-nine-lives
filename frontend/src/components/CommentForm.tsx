import React, { useState } from "react";
import { Box, Button, useToast } from "@chakra-ui/react";
import { Comment, postComment } from "../hooks/usePublicDecks";
import { useUserContext } from "./UserContext";
import InputFieldElement from "./InputFieldElement";

interface CommentFormProps {
  deckId: number;
}

const CommentForm = ({ deckId }: CommentFormProps) => {
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
          isClosable: true,
        });
      } else if (response.error) {
        toast({
          title: "Error",
          description:
          response.error.message || "An error occurred while submitting your comment.",
          status: "error",
          isClosable: true,
        });
      } else {
        toast({
          title: "Error",
          description: "An unexpected error occurred.",
          status: "error",
          isClosable: true,
        });
      }
    } catch (err: any) {
      toast({
        title: "Error",
        description:
          err.message || "An error occurred while submitting your comment.",
        status: "error",
        isClosable: true,
      });
    }
  };

  return (
    <Box mt={4}>
      <form onSubmit={handleSubmit}>
        <InputFieldElement
          name="Add a Comment"
          placeholder="Enter your comment"
          component="Textarea"
          value={commentText}
          onChange={setCommentText}
        />
        <Button mt={2} type="submit">
          Submit
        </Button>
      </form>
    </Box>
  );
};

export default CommentForm;
