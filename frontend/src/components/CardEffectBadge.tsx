import { HStack, Icon, Text } from "@chakra-ui/react"
import { FaFistRaised } from "react-icons/fa"
import { FaShieldCat } from "react-icons/fa6"

interface Props {
    attack?: number;
    defence?: number;
}

const CardEffectBadge = ({ attack, defence }: Props) => {

    const GetIcon = () => {
        return (
            <HStack>
                {attack !== undefined && (
                    <>
                        <Icon as={FaFistRaised} />
                        <Text>
                            {attack}
                        </Text>
                    </>
                )}

                {defence !== undefined && (
                    <>
                        <Icon as={FaShieldCat} />
                        <Text>
                            {defence}
                        </Text>
                    </>
                )}

                {attack === undefined && defence === undefined && (
                    <>
                       
                        <Text>
                           -
                        </Text>
                    </>
                )}
            </HStack>);

    }
    return (
        <HStack>
            <>
                {GetIcon()}
            </>
        </HStack>
    )
}

export default CardEffectBadge