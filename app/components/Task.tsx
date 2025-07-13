"use client";
import { ITask } from "@/types/tasks";
import { FiEdit, FiTrash2 } from "react-icons/fi";
import Modal from "./Modal";
import { FormEventHandler, useState } from "react";
import { useRouter } from "next/navigation";
import { deleteTodo, editTodo } from "@/api";

interface TaskProps {
    task: ITask;
}

const Task: React.FC<TaskProps> = ({ task }) => {
    const router = useRouter();
    const [modalEdit, setModalEdit] = useState<boolean>(false);
    const [taskToEdit, setTaskToEdit] = useState<string>(task.text);
    const [openModalDeleted, setOpenModalDeleted] = useState<boolean>(false);
    // const [taskDelete, setNewTaskDelete] = useState<boolean>(false);
    

    const handleSubmitEditTodo: FormEventHandler<HTMLFormElement> = async(e) => {
        e.preventDefault();
        await editTodo({
            id: task.id,
            text: taskToEdit
        });
        setModalEdit(false); // Close modal after submission
        router.refresh(); // Refresh the page to show the new task
    }

    const handleDeleteTask = async (id: string) => {
        await deleteTodo(id);
        setOpenModalDeleted(false); // Close modal after deletion
        router.refresh(); // Refresh the page to show the updated task list
    }

    return (
        <tr key={task.id}>
            <td className="w-full">{task.text}</td>
            <td className="flex gap-5">
                <FiEdit onClick={() => setModalEdit(true)} cursor="pointer" className="text-blue-500" size={25}/>
                <Modal modalOpen={modalEdit} setModalOpen={setModalEdit}>
                    <form onSubmit={handleSubmitEditTodo}>
                        <h3 className="font-bold text-lg">Edit Task</h3>
                        <div className="modal-action">
                            <input value={taskToEdit} onChange={e => setTaskToEdit(e.target.value)} type="text" placeholder="Type here" className="input input-bordered w-full" />
                            <button type="submit" className="btn">Submit</button>
                        </div>
                    </form>
                </Modal>
                <FiTrash2 onClick={() => setOpenModalDeleted(true)} cursor="pointer" className="text-red-500" size={25}/>
                <Modal modalOpen={openModalDeleted} setModalOpen={setOpenModalDeleted}>
                    <h3 className="text-lg">Are you sure, you want to delete this task?</h3>
                    <div className="modal-action">
                        <button onClick={() => handleDeleteTask(task.id)} className="btn">Yes</button>
                    </div>
                </Modal>
            </td>
        </tr>
    );
};

export default Task;