import { FormControl, FormLabel, Input } from '@chakra-ui/react'
import { ChangeEvent } from 'react';

interface UsernameInputProps {
    value: string;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void;
}

const UsernameInput = ({ value, onChange }: UsernameInputProps) => (
    <FormControl>
        <FormLabel>Username</FormLabel>
        <Input type="text" placeholder="Username" value={value} onChange={onChange} />
    </FormControl>
);

export default UsernameInput;