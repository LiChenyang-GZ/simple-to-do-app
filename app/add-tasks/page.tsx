'use client';
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { addTodo } from "@/api";
import { v4 as uuidv4 } from 'uuid';
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useForm } from "react-hook-form";
import { Textarea } from "@/components/ui/textarea";
import TaskForm from "../components/TaskForm";
import { useTodoStore } from "../store/todoStore";


type FormData = {
    text: string;
    description: string; // Optional field for description
};



export default function AddTasksPage() {
    const router = useRouter();
    // const [newTaskValue, setNewTaskValue] = useState<string>("");
    const { register, handleSubmit, reset } = useForm<FormData>();
    const { addTodo } = useTodoStore();

    // const handleSubmitNewTodo = async (e) => {
    //     e.preventDefault();

    //     await addTodo({
    //         id: uuidv4(),
    //         text: newTaskValue
    //     });
    //     setNewTaskValue(""); // Clear input after submission
    //     router.push('/'); // Redirect to the home page after adding the task
    // };
    const onSubmit = async (data: FormData) => {
        await addTodo({
            // id: uuidv4(),
            text: data.text,
            description: data.description
        });
        reset();
        router.push('/'); // Redirect to the home page after adding the task

    }

    return (
        <main className="flex justify-center items-center min-h-screen">
            {/* <form onSubmit={handleSubmit(onSubmit)}>
                <h3 className="text-2xl font-bold mb-4 text-black">Add new Task</h3>
                <div className="modal-action"> */}
                    {/* <Input 
                        value={newTaskValue} 
                        onChange={e => setNewTaskValue(e.target.value)} 
                        type="text" 
                        placeholder="Type here" 
                        className="input input-bordered w-full" /> */}
                    {/* <Input 
                        {...register("text", { required: true })}
                        type="text"
                        placeholder="Type here"
                        className="input input-bordered w-full"
                    />
                    <Textarea 
                        {...register("description", { required: false })}
                        placeholder="Description"
                        className="textarea textarea-bordered w-full mt-2"
                    />
                    <Button type="submit" className="btn">Submit</Button> */}
                    
                {/* </div>
            </form> */}
            <TaskForm defaultValue={{ text: "", description: "" }} onSubmit={onSubmit} />
        </main>
    )
}
