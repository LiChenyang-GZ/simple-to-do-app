import React from "react";
import { ITask } from "@/types/tasks";
import Task from "./Task";

interface TodoListProps {
  tasks: ITask[];
}

const TodoList: React.FC<TodoListProps> = ({ tasks = []}) => {
    return <div className="overflow-x-auto">
  <table className="table">
    {/* head */}
    <thead>
      <tr>
        <th>Tasks</th>
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
  </table>
</div>;
};

export default TodoList;