import React, { useEffect, useState } from "react";
import axios from "axios";

const API_URL = "https://localhost:7014/api/tasks";

function App() {
  const [tasks, setTasks] = useState([]);
  const [newName, setNewName] = useState("");
  const [editingTaskId, setEditingTaskId] = useState(null);
  const [editingName, setEditingName] = useState("");

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
    if (!newName.trim()) return;
    const res = await axios.post(API_URL, { name: newName }); // expects Name in backend DTO
    setTasks([...tasks, res.data]);
    setNewName("");
  };

  // Delete task
  const deleteTask = async (id) => {
    await axios.delete(`${API_URL}/${id}`);
    setTasks(tasks.filter((t) => t.id !== id));
  };

  // Start editing
  const startEditing = (task) => {
    setEditingTaskId(task.id);
    setEditingName(task.name ?? task.name); // depending on backend DTO consistency
  };

  // Save edit
  const saveEdit = async (id) => {
    await axios.put(`${API_URL}/${id}`, { name: editingName });
    setTasks(tasks.map((t) => (t.id === id ? { ...t, name: editingName } : t)));
    setEditingTaskId(null);
    setEditingName("");
  };

  return (
    <div style={{ maxWidth: "600px", margin: "auto", padding: "20px" }}>
      <h1>Task Manager</h1>

      {/* Add new task */}
      <div>
        <input
          type="text"
          placeholder="New task name"
          value={newName}
          onChange={(e) => setNewName(e.target.value)}
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
                  value={editingName}
                  onChange={(e) => setEditingName(e.target.value)}
                />
                <button onClick={() => saveEdit(task.id)}>Save</button>
                <button onClick={() => setEditingTaskId(null)}>Cancel</button>
              </>
            ) : (
              <>
                {task.name ?? task.name}
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
