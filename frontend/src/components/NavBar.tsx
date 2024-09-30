import { AddIcon, EditIcon, ExternalLinkIcon, HamburgerIcon, RepeatIcon } from "@chakra-ui/icons"
import { HStack, IconButton, Menu, MenuButton, MenuItem, MenuList } from "@chakra-ui/react"

const NavBar = () => {
    return <HStack justifyContent="space-between">
        
        <Menu>
            <MenuButton
                as={IconButton}
                aria-label='Options'
                icon={<HamburgerIcon />}
                variant='outline'
            />
            <MenuList>
                <MenuItem icon={<AddIcon />} command='⌘T'>
                    New Tab
                </MenuItem>
                <MenuItem icon={<ExternalLinkIcon />} command='⌘N'>
                    New Window
                </MenuItem>
                <MenuItem icon={<RepeatIcon />} command='⌘⇧N'>
                    Open Closed Tab
                </MenuItem>
                <MenuItem icon={<EditIcon />} command='⌘O'>
                    Open File...
                </MenuItem>
            </MenuList>
        </Menu>
    </HStack>
}

export default NavBar