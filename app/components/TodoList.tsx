import React from "react";
import { ITask } from "@/types/tasks";
import Task from "./Task";
import { Table } from "@/components/ui/table";

interface TodoListProps {
  tasks: ITask[];
}

const TodoList: React.FC<TodoListProps> = ({ tasks = []}) => {
    return <div className="overflow-x-auto">
  <Table className="table">
    {/* head */}
    <thead className="text-black text-center">
      <tr>
        <th>Tasks</th>
        <th>Description</th>
        <th>Actions</th>

      </tr>
    </thead>
    <tbody>
      {/* row 1 */}
      {/* {tasks.map((task) => ( 
        <tr key={task.id}>
          <td>{task.text}</td>
          <td>Blue</td>
      </tr>
      ))} */}
      {tasks.map((task) => (
        <Task key={task.id} task={task} />
      ))}
      
    </tbody>
  </Table>
</div>;
};

export default TodoList;