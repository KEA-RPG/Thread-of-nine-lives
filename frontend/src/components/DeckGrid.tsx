// DeckGrid.tsx
import { SimpleGrid, Text } from "@chakra-ui/react";
import { usePublicDecks } from "../hooks/usePublicDecks";
import DeckCard from "./DeckCard";
import DeckCardSkeleton from "./DeckCardSkeleton";

const DeckGrid = () => {
  const { data: decks, error } = usePublicDecks();
  console.log('Decks:', decks);
  const skeletons = [...Array(20).keys()];

  return (
    <div>
      {error && <Text>{error}</Text>}
      <SimpleGrid
        columns={{ sm: 1, md: 2, lg: 3, xl: 4 }}
        spacing={10}
        padding="10"
      >
        {!decks &&
          skeletons.map((skeleton) => <DeckCardSkeleton key={skeleton} />)}
        {decks &&
          decks.map((deck) => <DeckCard key={deck.id} deck={deck} />)}
      </SimpleGrid>
    </div>
  );
};

export default DeckGrid;
