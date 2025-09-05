import React, { useEffect, useState } from "react";
import axios from "axios";

const API_URL = "https://localhost:7014/api/tasks";

function App() {
  const [tasks, setTasks] = useState([]);
  const [newTitle, setNewTitle] = useState("");
  const [editingTaskId, setEditingTaskId] = useState(null);
  const [editingTitle, setEditingTitle] = useState("");

  // Load tasks on startup
  useEffect(() => {
    const fetchTasks = async () => {
      const res = await axios.get(API_URL);
      setTasks(res.data);
    };
    fetchTasks();
  }, []);

  // Add task
  const addTask = async () => {
    if (!newTitle.trim()) return;
    const res = await axios.post(API_URL, { title: newTitle }); // expects Title in backend DTO
    setTasks([...tasks, res.data]);
    setNewTitle("");
  };

  // Delete task
  const deleteTask = async (id) => {
    await axios.delete(`${API_URL}/${id}`);
    setTasks(tasks.filter((t) => t.id !== id));
  };

  // Start editing
  const startEditing = (task) => {
    setEditingTaskId(task.id);
    setEditingTitle(task.title ?? task.name); // depending on backend DTO consistency
  };

  // Save edit
  const saveEdit = async (id) => {
    await axios.put(`${API_URL}/${id}`, { title: editingTitle });
    setTasks(tasks.map((t) => (t.id === id ? { ...t, title: editingTitle } : t)));
    setEditingTaskId(null);
    setEditingTitle("");
  };

  return (
    <div style={{ maxWidth: "600px", margin: "auto", padding: "20px" }}>
      <h1>Task Manager</h1>

      {/* Add new task */}
      <div>
        <input
          type="text"
          placeholder="New task title"
          value={newTitle}
          onChange={(e) => setNewTitle(e.target.value)}
        />
        <button onClick={addTask}>Add</button>
      </div>

      {/* Task list */}
      <ul>
        {tasks.map((task) => (
          <li key={task.id} style={{ margin: "8px 0" }}>
            {editingTaskId === task.id ? (
              <>
                <input
                  type="text"
                  value={editingTitle}
                  onChange={(e) => setEditingTitle(e.target.value)}
                />
                <button onClick={() => saveEdit(task.id)}>Save</button>
                <button onClick={() => setEditingTaskId(null)}>Cancel</button>
              </>
            ) : (
              <>
                {task.title ?? task.name}
                <button onClick={() => startEditing(task)}>Edit</button>
                <button onClick={() => deleteTask(task.id)}>Delete</button>
              </>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
