import { Card, CardBody, Divider } from "@chakra-ui/react";
import NavigationItem from "./NavigationItem";

const CenterNavigation = () => {
    const data = [
        { link: "/combat", children: "Fight!" },
        { link: "/deck/create", children: "Create deck" },
        { link: "/deck", children: "Select deck" },
        { link: "/signout", children: "Log out" }
    ];


    return (
        <Card minW="40vw" mt="10vh">
            <CardBody>
                {data.map((item, index) => (
                    <>
                        <NavigationItem key={index} link={item.link}>
                            {item.children}
                        </NavigationItem>
                        {index < data.length - 1 && <Divider />}
                    </>
                ))}
            </CardBody>
        </Card>
    );
}

export default CenterNavigation;