interface TaskFormProps {
    defaultValue: {
        text: string;
        description: string;
    }
    onSubmit: (data: { text: string; description: string }) => void;
}

