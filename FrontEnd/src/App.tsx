import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "./components/auth/Login";
import Register from "./components/auth/Register";
import Welcome from "./components/welcome/Welcome";
import Dashboard from "./components/dashboard/dashboard";

function App() {
  return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Navigate to="/welcome" />} />
                <Route path="/welcome" element={<Welcome />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/dashboard" element={<Dashboard />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;