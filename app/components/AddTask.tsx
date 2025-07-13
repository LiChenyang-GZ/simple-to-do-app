'use client';
import React, { FormEventHandler } from "react";
import {AiOutlinePlus } from "react-icons/ai"
import Modal from "./Modal";
import { addTodo } from "@/api";
import { useRouter } from "next/navigation";
import { v4 as uuidv4 } from 'uuid';

const AddTask = () => {
    const router = useRouter();
    const [modalOpen, setModalOpen] = React.useState<boolean>(false);
    const [newTaskValue, setNewTaskValue] = React.useState<string>("");

    const handleSubmitNewTodo: FormEventHandler<HTMLFormElement> = async(e) => {
        e.preventDefault();
        await addTodo({
            id: uuidv4(),
            text: newTaskValue
        })
        setNewTaskValue(""); // Clear input after submission
        setModalOpen(false); // Close modal after submission
        router.refresh(); // Refresh the page to show the new task
    }

    return(
        <div>
            <button onClick={() => setModalOpen(true)} className="btn btn-primary w-full">
                Add new task <AiOutlinePlus size={18} />
            </button>
            <Modal modalOpen={modalOpen} setModalOpen={setModalOpen}>
                <form onSubmit={handleSubmitNewTodo}>
                    <h3 className="font-bold text-lg">Add new Task</h3>
                    <div className="modal-action">
                        <input value={newTaskValue} onChange={e => setNewTaskValue(e.target.value)} type="text" placeholder="Type here" className="input input-bordered w-full" />
                        <button type="submit" className="btn">Submit</button>
                    </div>
                </form>
            </Modal>
        </div>
    )
};

export default AddTask;