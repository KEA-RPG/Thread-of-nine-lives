// DeckModal.tsx
import React, { useState } from "react";
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  Text,
  Box,
} from "@chakra-ui/react";
import { Deck, Comment } from "../hooks/usePublicDecks";
import CommentForm from "./CommentForm";

interface Props {
  deck: Deck;
  isOpen: boolean;
  onClose: () => void;
  finalRef: React.RefObject<any>;
}

const DeckModal = ({ deck, isOpen, onClose, finalRef }: Props) => {
  const [comments, setComments] = useState<Comment[]>(deck.comments || []);

  const handleCommentAdded = (newComment: Comment) => {
    setComments((prevComments) => [...prevComments, newComment]);
  };

  return (
    <Modal finalFocusRef={finalRef} isOpen={isOpen} onClose={onClose} size="lg">
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Comments for {deck.name}</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          {comments.length > 0 ? (
            comments.map((comment) => (
              <Box
                key={comment.id}
                mb={4}
                p={3}
                borderWidth="1px"
                borderRadius="md"
              >
                <Text fontSize="sm" color="gray.500">
                  {new Date(comment.createdAt || "").toLocaleString()}
                </Text>
                <Text mt={2}>{comment.text}</Text>
              </Box>
            ))
          ) : (
            <Text>No comments available.</Text>
          )}

          {/* Include the CommentForm component */}
          <CommentForm deckId={deck.id!} onCommentAdded={handleCommentAdded} />
        </ModalBody>
      </ModalContent>
    </Modal>
  );
};

export default DeckModal;
