import {
  Card,
  CardBody,
  CardFooter,
  Heading,
  Text,
  Button,
  useDisclosure,
  Center,
  Stack,
} from "@chakra-ui/react";
import { Deck } from "../hooks/usePublicDecks";
import React from "react";
import DeckModal from "./DeckModal";

interface Props {
  deck: Deck;
}

const DeckCard = ({ deck }: Props) => {
  const { isOpen, onOpen, onClose } = useDisclosure();
  const finalRef = React.useRef(null);

  return (
    <Card>
      <CardBody>
        <Stack mt="6" spacing="3">
          <Heading fontSize="2xl">{deck.name}</Heading>
          <Text>Created by {deck.userName}</Text>
        </Stack>
      </CardBody>
      <Center>
        <CardFooter>
          <Button mt={4} onClick={onOpen}>
            View Comments
          </Button>
          <DeckModal
            deck={deck}
            isOpen={isOpen}
            onClose={onClose}
            finalRef={finalRef}
          />
        </CardFooter>
      </Center>
    </Card>
  );
};

export default DeckCard;
