import { Box, Container, Heading, Text } from "@chakra-ui/react";
import DeckGrid from "../components/DeckGrid";

export default function PublicDecksPage() {
  return (
      <Box py={8} textAlign="center">
        <Container maxW="container.lg">
          <Heading as="h1" size="xl" mb={4}>
          </Heading>
          <Text fontSize="xl" mb={8}>
            Browse all public decks shared by other users. Click on a deck to view its comments.
          </Text>
        </Container>
        <DeckGrid />
      </Box>
  );
}
