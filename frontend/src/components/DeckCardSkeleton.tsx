import { Card, CardBody, Skeleton, SkeletonText } from "@chakra-ui/react";

const DeckCardSkeleton = () => {
  return (
    <Card>
      <Skeleton height="150px" />
      <CardBody>
        <SkeletonText mt="4" noOfLines={2} spacing="4" />
      </CardBody>
    </Card>
  );
};

export default DeckCardSkeleton;
