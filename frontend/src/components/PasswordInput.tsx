import { FormControl, FormLabel, Input } from '@chakra-ui/react';

interface Props {
    type: string;
    placeholder: string;
    value: string;
    onChange: (event: string) => void;
}

const PasswordInput = ({ type, placeholder, value, onChange }: Props) => (
    <FormControl>
        <FormLabel>Password</FormLabel>
        <Input type={type} placeholder={placeholder} value={value} onChange={ (e) => onChange(e.target.value)} />
    </FormControl>
);

export default PasswordInput;