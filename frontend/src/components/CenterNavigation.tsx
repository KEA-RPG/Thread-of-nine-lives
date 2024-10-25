import { Card, CardBody, Divider, Text } from "@chakra-ui/react";
import NavigationItem from "./NavigationItem";
import { useUserContext } from "./UserContext";

const CenterNavigation = () => {
    const { role } = useUserContext();
    const data = [
        { link: "/combat", text: "Fight!" },
        { link: "/decks", text: "Decks" },
        { link: "/logout", text: "Log out" }
    ];
    const adminData = [
        { link: "/admin/enemies", text: "Enemies" },
        { link: "/admin/cards", text: "Cards" },
        // { link: "", text: "Users" },
        { link: "/logout", text: "Log out" }
    ];


    return (
        <Card p={4} w="100%">
            <CardBody w="100%">
                {role === "admin" ? (
                    adminData.map((item, index) => (
                        <>
                            <NavigationItem key={index} link={item.link}>
                                <Text>{item.text}</Text>
                            </NavigationItem>
                            {index < adminData.length - 1 && <Divider />}
                        </>
                    ))
                ) : role == "player" ? (
                    data.map((item, index) => (
                        <>
                            <NavigationItem key={index} link={item.link}>
                                <Text>{item.text}</Text>
                                </NavigationItem>
                            {index < data.length - 1 && <Divider />}
                        </>
                    ))
                ) : null}
            </CardBody>
        </Card >
    )
}

export default CenterNavigation;