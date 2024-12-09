import React from "react";
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
import { Deck } from "../hooks/usePublicDecks";
import CommentForm from "./CommentForm";

interface Props {
  deck: Deck;
  isOpen: boolean;
  onClose: () => void;
  finalRef: React.RefObject<any>;
}

const DeckModal = ({ deck, isOpen, onClose, finalRef }: Props) => {
  const comments = deck.comments || [];

  return (
    <Modal
      finalFocusRef={finalRef}
      isOpen={isOpen}
      onClose={onClose}
      size="lg"
    >
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
                  {comment.username} -{" "}
                  {new Date(comment.createdAt || "").toLocaleString()}
                </Text>
                <Text mt={2}>{comment.text}</Text>
              </Box>
            ))
          ) : (
            <Text>No comments available.</Text>
          )}

          <CommentForm deckId={deck.id!} />
        </ModalBody>
      </ModalContent>
    </Modal>
  );
};

export default DeckModal;
