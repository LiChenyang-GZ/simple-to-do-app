"use client";
import { useForm } from "react-hook-form";
import { Textarea } from "@/components/ui/textarea";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

interface TaskFormProps {
    defaultValue: {
        text: string;
        description: string;
    }
    onSubmit: (data: { text: string; description: string }) => void;
}

export default function TaskForm({defaultValue, onSubmit }: TaskFormProps) {
    const { register, handleSubmit } = useForm({
        defaultValues: {
            text: defaultValue.text,
            description: defaultValue.description,
        },
    });

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
            <Input 
                {...register("text", { required: true })} 
                type="text" 
                placeholder="Task Title" 
                className="input input-bordered w-full" />
            <Textarea 
                {...register("description")} 
                placeholder="Task Description" 
                className="textarea textarea-bordered w-full" />
            <Button type="submit" className="btn btn-primary">Submit</Button>
        </form>
    );
}