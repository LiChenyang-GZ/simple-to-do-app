"use client";
import { ITask } from "@/types/tasks";
import { FiEdit, FiTrash2 } from "react-icons/fi";
import Modal from "./Modal";
import { FormEventHandler, useState } from "react";
import { useRouter } from "next/navigation";
// import { deleteTodo, editTodo } from "@/api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import TaskForm from "./TaskForm";
import { useTodoStore } from "../store/todoStore";

interface TaskProps {
    task: ITask;
}

const Task: React.FC<TaskProps> = ({ task }) => {
    const router = useRouter();
    const [modalEdit, setModalEdit] = useState<boolean>(false);
    const [taskToEdit, setTaskToEdit] = useState<string>(task.text);
    const [openModalDeleted, setOpenModalDeleted] = useState<boolean>(false);
    // const [taskDelete, setNewTaskDelete] = useState<boolean>(false);
    const [descriptionToEdit, setDescriptionToEdit] = useState(task.description ?? "");

    const { editTodo } = useTodoStore();
    const { deleteTodo } = useTodoStore();
    
    const handleSubmitEditTodo: FormEventHandler<HTMLFormElement> = async(e) => {
        e.preventDefault();
        await editTodo({
            id: task.id,
            text: taskToEdit,
            description: descriptionToEdit // Assuming description is part of the task
        });
        setModalEdit(false); // Close modal after submission
        router.refresh(); // Refresh the page to show the new task
    }

    const handleDeleteTask = async (id: number) => {
        await deleteTodo(id);
        setOpenModalDeleted(false); // Close modal after deletion
        router.refresh(); // Refresh the page to show the updated task list
    }

    return (
        <tr key={task.id}>
            <td className="w-full text-center">{task.text}</td>
            <td className="w-full">
                <span
                    style={{
                    display: "inline-block",
                    padding: "0.125rem 0.5rem",
                    borderRadius: 6,
                    backgroundColor: task.category?.color || "#FA0101",
                    color: "#0F172A",
                    fontWeight: 600,
                    fontSize: "0.875rem"
                    }}
                >
                    {task.category?.name ?? "No category"}
                </span>
            </td>
            <td className="w-full">{task.description}</td>
            <td className="flex gap-5">
                <FiEdit onClick={() => setModalEdit(true)} cursor="pointer" className="text-blue-500" size={25}/>
                <Modal modalOpen={modalEdit} setModalOpen={setModalEdit}>
                    {/* <form onSubmit={handleSubmitEditTodo}>
                        <h3 className="font-bold text-lg">Edit Task</h3>
                        <div className="modal-action">
                            <Input 
                                value={taskToEdit} 
                                onChange={e => setTaskToEdit(e.target.value)} 
                                type="text" 
                                placeholder="Type here" 
                                className="input input-bordered w-full" />
                            <Textarea 
                                value={descriptionToEdit}
                                onChange={e => setDescriptionToEdit(e.target.value)}
                                placeholder="Description"
                                className="textarea textarea-bordered w-full mt-2"
                            />
                            <Button type="submit" className="btn">Submit</Button>
                        </div>
                    </form> */}
                    <TaskForm
                            defaultValue={{ text: taskToEdit, description: descriptionToEdit }}
                            onSubmit={async (data) => {
                                await editTodo({
                                    id: task.id,
                                    text: data.text,
                                    description: data.description
                                });
                                setModalEdit(false);
                                router.refresh(); // Refresh the page to show the updated task
                            }}
                        />
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