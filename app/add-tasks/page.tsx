'use client';
import { useState } from "react";
import { useRouter } from "next/navigation";
import { addTodo } from "@/api";
import { v4 as uuidv4 } from 'uuid';

export default function AddTasksPage() {
    const router = useRouter();
    const [newTaskValue, setNewTaskValue] = useState<string>("");

    const handleSubmitNewTodo = async (e) => {
        e.preventDefault();

        await addTodo({
            id: uuidv4(),
            text: newTaskValue
        });
        setNewTaskValue(""); // Clear input after submission
        router.push('/'); // Redirect to the home page after adding the task
    };

    return (
        <main className="flex justify-center items-center min-h-screen">
            <form onSubmit={handleSubmitNewTodo}>
                <h3 className="text-2xl font-bold mb-4 text-white">Add new Task</h3>
                <div className="modal-action">
                    <input value={newTaskValue} onChange={e => setNewTaskValue(e.target.value)} type="text" placeholder="Type here" className="input input-bordered w-full" />
                    <button type="submit" className="btn">Submit</button>
                </div>
            </form>
        </main>
    )
}
