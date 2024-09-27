import { Card, CardBody, Divider } from "@chakra-ui/react";
import NavigationItem from "./NavigationItem";

const CenterNavigation = () => {
    const data = [
        { link: "/combat", text: "Fight!" },
        { link: "/deck/create", text: "Create deck" },
        { link: "/deck", text: "Select deck" },
        { link: "/signout", text: "Log out" }
    ];


    return (
        <Card minW="40vw" mt="10vh">
            <CardBody>
                {data.map((item, index) => (
                    <>
                        <NavigationItem key={index} link={item.link}>
                            {item.text}
                        </NavigationItem>
                        {index < data.length - 1 && <Divider />}
                    </>
                ))}
            </CardBody>
        </Card>
    );
}

export default CenterNavigation;