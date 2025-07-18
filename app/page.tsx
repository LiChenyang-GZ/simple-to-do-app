import { getAllTodos } from "@/api";
import AddTask from "./components/AddTask";
import TodoList from "./components/TodoList";

export default async function Home() {
  const tasks = await getAllTodos();
  console.log(tasks);
  return (
    <main className="max-w-4xl mx-auto mt-4 bg-white text-black">
      <div className="text-center my-5 flex flex-col gap-4"> 
        <h1 className="text-2xl font-bold">To do list app</h1>
        <AddTask />
      </div>
      <TodoList tasks={tasks} />
    </main>
     
  );
}
