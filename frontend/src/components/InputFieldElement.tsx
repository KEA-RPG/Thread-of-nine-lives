import { FormControl, FormErrorMessage, FormLabel, Input } from "@chakra-ui/react";

interface Props {
    type: string;
    name?: string;
    placeholder?: string;
    value?: string;
    onChange?: (e: string) => void;
    errorText?: string;
}

const InputFieldElement = (props: Props) => {
    const { name, placeholder, type, value, onChange, errorText } = props


    const isError = value === 'error';
    return (
        <FormControl isInvalid={isError} >
            <FormLabel>{name}</FormLabel>
            {type === 'file' ? (
                <input type={type} onChange={(e) => onChange && onChange(e.target.value)} placeholder={placeholder} />
            ) : (
                <Input width={60} type={type} value={value} onChange={(e) => onChange && onChange(e.target.value)} placeholder={placeholder} />
            )}
            {errorText ?? (
                <>
                    {!isError ? (null) : (
                        <FormErrorMessage>Invalid {name}</FormErrorMessage>
                    )}
                </>
            )
            }
        </FormControl>
    )
}

export default InputFieldElement