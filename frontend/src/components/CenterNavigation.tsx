import { Card, CardBody, Divider } from "@chakra-ui/react";
import NavigationItem from "./NavigationItem";
import { useUserContext } from "./UserContext";

const CenterNavigation = () => {
    const { role, token } = useUserContext();
    const data = [
        { link: "/combat", text: "Fight!" },
        { link: "/decks", text: "Decks" },
        { link: "/", text: "Log out" }
    ];
    const adminData = [
        { link: "/admin/enemies", text: "Enemies" },
        { link: "/admin/cards", text: "Cards" },
        { link: "", text: "Users" },
        { link: "/", text: "Log out" }
    ];


    return (
        <Card>
            <CardBody>
                {role == "admin" ? (
                    adminData.map((item, index) => (
                        <>
                            <NavigationItem key={index} link={item.link}>
                                {item.text}
                            </NavigationItem>
                            {index < adminData.length - 1 && <Divider />}
                        </>
                    ))
                ) : role == "player" ? (
                    data.map((item, index) => (
                        <>
                            <NavigationItem key={index} link={item.link}>
                                {item.text}
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