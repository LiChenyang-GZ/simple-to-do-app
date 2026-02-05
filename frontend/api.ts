import { ITask } from "./types/tasks";

const baseUrl = "http://localhost:5000/api";

export const getAllTodos = async (): Promise<ITask[]> => {
    const response = await fetch(`${baseUrl}/todos`);
    const todos = await response.json();
    return todos;
}

export const addTodo = async (todo: Omit<ITask, "id">): Promise<ITask> => {
    const res = await fetch(`${baseUrl}/todos`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(todo)
    });
    const newTodo = await res.json();
    return newTodo;
}

export const editTodo = async (todo: ITask): Promise<ITask> => {
    const res = await fetch(`${baseUrl}/todos/${todo.id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(todo)
    });

    // If the server returns 204 No Content, there's no JSON body to parse.
    if (res.status === 204) {
        return todo;
    }

    // Otherwise attempt to parse JSON if present
    const contentType = res.headers.get("content-type") ?? "";
    if (contentType.includes("application/json")) {
        const updatedTodo = await res.json();
        return updatedTodo;
    }

    return todo;
}

export const deleteTodo = async (id: number): Promise<void> => {
    await fetch(`${baseUrl}/todos/${id}`, {
        method: "DELETE",
    });

}