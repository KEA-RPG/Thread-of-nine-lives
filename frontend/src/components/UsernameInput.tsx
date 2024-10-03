import { FormControl, FormLabel, Input } from '@chakra-ui/react'
//import { ChangeEvent } from 'react';

interface Props {
    type: string;
    placeholder: string;
    value: string;
    onChange: (event: string) => void;
}

const UsernameInput = ({ type, placeholder, value, onChange }: Props) => (
    <FormControl>
        <FormLabel>Username</FormLabel>
        <Input type={type}  placeholder={placeholder} value={value} onChange={ (e) => onChange(e.target.value)} />
    </FormControl>
);

export default UsernameInput;