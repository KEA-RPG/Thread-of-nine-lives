import { FormControl, FormLabel, Input } from '@chakra-ui/react';
import { ChangeEvent } from 'react';

interface Props {
    value: string;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void;
}

const PasswordInput = ({ value, onChange }: Props) => (
    <FormControl>
        <FormLabel>Password</FormLabel>
        <Input type="password" placeholder="Password" value={value} onChange={onChange} />
    </FormControl>
);

export default PasswordInput;