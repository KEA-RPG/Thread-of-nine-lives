import {
  FormControl,
  FormErrorMessage,
  FormLabel,
  Input,
  Textarea,
} from "@chakra-ui/react";

interface Props {
  component?: "Input" | "Textarea" | "File";
  type?: string;
  name?: string;
  placeholder?: string;
  value?: string;
  onChange?: (value: string) => void;
  errorText?: string;
  isInvalid?: boolean;
}

const InputFieldElement = (props: Props) => {
  const {
    name,
    placeholder,
    component = "Input",
    type,
    value,
    onChange,
    errorText,
    isInvalid,
  } = props;

  return (
    <FormControl isInvalid={isInvalid}>
      {name && <FormLabel>{name}</FormLabel>}
      {component === "File" ? (
        <input
          type="file"
          onChange={(e) => onChange && onChange(e.target.value)}
          placeholder={placeholder}
        />
      ) : component === "Textarea" ? (
        <Textarea
          value={value}
          onChange={(e) => onChange && onChange(e.target.value)}
          placeholder={placeholder}
        />
      ) : (
        <Input
          width={60}
          type={type}
          value={value}
          onChange={(e) => onChange && onChange(e.target.value)}
          placeholder={placeholder}
        />
      )}
      {errorText && <FormErrorMessage>{errorText}</FormErrorMessage>}
    </FormControl>
  );
};

export default InputFieldElement;
