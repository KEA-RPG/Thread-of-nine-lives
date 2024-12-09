import {
    Modal,
    ModalOverlay,
    ModalContent,
    ModalHeader,
    ModalCloseButton,
    ModalBody,
    Text,
    Button,
    ModalFooter,
} from "@chakra-ui/react";
import { ListItemBase } from "../hooks/useData";

interface Props<T extends ListItemBase> {
    item: T;
    confirmCallback: (item: T) => Promise<void>;
    onClose: () => void;
    entityName: string;
    isOpen: boolean;
}

const ConfirmationModal = <T extends ListItemBase>({ item, confirmCallback, onClose, entityName, isOpen }: Props<T>) => {

    const handleConfirm = async () => {
        await confirmCallback(item);
        onClose();
        window.location.reload();
    }


    return (
        <Modal
            isOpen={isOpen}
            onClose={onClose}
            size="lg"
            closeOnOverlayClick={true} 
        >
            <ModalOverlay />
            <ModalContent>
                <ModalHeader>Confirm deletion of {item.name}</ModalHeader>
                <ModalCloseButton />
                <ModalBody>
                    <Text>Are you sure you want to delete {entityName} {item.name}(ID:{item.id})</Text>
                </ModalBody>
                <ModalFooter>
                    <Button colorScheme='red' mr={3} onClick={handleConfirm}>
                        Delete
                    </Button>
                    <Button onClick={onClose}>Cancel</Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
};

export default ConfirmationModal;
