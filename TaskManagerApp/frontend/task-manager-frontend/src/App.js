import React, { useEffect, useState } from "react";
import axios from "axios";

const API_URL = "https://localhost:7014/api/tasks";

function App() {
  const [tasks, setTasks] = useState([]);
  const [newName, setNewName] = useState("");

  // Load tasks
  useEffect(() => {
    axios.get(API_URL).then((res) => setTasks(res.data));
  }, []);

  const addTask = async () => {
    if (!newName.trim()) return;
    const res = await axios.post(API_URL, { name: newName });
    setTasks([...tasks, res.data]);
    setNewName("");
  };

  const toggleTask = async (task) => {
    const updated = { ...task, isCompleted: !task.isCompleted };
    await axios.put(`${API_URL}/${task.id}`, updated);
    setTasks(tasks.map((t) => (t.id === task.id ? updated : t)));
  };

  const deleteTask = async (id) => {
    await axios.delete(`${API_URL}/${id}`);
    setTasks(tasks.filter((t) => t.id !== id));
  };

  return (
    <div style={{ margin: "2rem" }}>
      <h1>Task Manager</h1>

      <div>
        <input
          value={newName}
          onChange={(e) => setNewName(e.target.value)}
          placeholder="New task..."
        />
        <button onClick={addTask}>Add</button>
      </div>

      <ul>
        {tasks.map((task) => (
          <li key={task.id} style={{ margin: "0.5rem 0" }}>
            <span
              style={{
                textDecoration: task.isCompleted ? "line-through" : "none",
                cursor: "pointer",
              }}
              onClick={() => toggleTask(task)}
            >
              {task.name}
            </span>
            <button
              style={{ marginLeft: "1rem", color: "red" }}
              onClick={() => deleteTask(task.id)}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
