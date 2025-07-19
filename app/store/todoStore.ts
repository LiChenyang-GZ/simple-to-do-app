"use client";
import { create } from 'zustand';
import { getAllTodos, addTodo as apiAddTodo, editTodo as apiEditTodo, deleteTodo as apiDeleteTodo } from '@/api';
import type { ITask } from '@/types/tasks';

type TodoStore = {
    todos: ITask[];
    fetchTasks: () => Promise<void>;
    setTasks: (tasks: ITask[]) => void;
    addTodo: (task: Omit<ITask, 'id'>) => Promise<void>;
    editTodo: (updatedTask: ITask) => Promise<void>;
    deleteTodo: (id: string) => Promise<void>;
};

export const useTodoStore = create<TodoStore>((set, get) => ({
    todos: [],
    fetchTasks: async () => {
    const tasks = await getAllTodos();
    set({ todos: tasks });
    },

    setTasks: (tasks) => set({ todos: tasks }),

    addTodo: async (task) => {
    const newTask = await apiAddTodo(task);
    const { todos } = get();
    set({ todos: [...todos, newTask] });
    },

    editTodo: async (updatedTask: ITask) => {
    const editedTask = await apiEditTodo(updatedTask);
    set((state) => ({
        todos: state.todos.map((task) =>
        task.id === editedTask.id ? editedTask : task
        ),
    }));
    },

    deleteTodo: async (id: string) => {
        await apiDeleteTodo(id);
        set((state) => ({
            todos: state.todos.filter((task) => task.id !== id),
        }));
    },
}));
