import React, { useState} from "react";
import type { AuthResponse } from "../../interfaces/auth/AuthInterfaces";
import { styles } from "../../styles/auth/AuthStyle";
import { Link, useNavigate } from "react-router-dom";
import { API_BASE } from "../../api/config";
import { extractError } from "../../api/apiClient";
import { saveAuth } from "../../auth/auth";

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const Authenticate = async () => {
        setLoading(true);
        setError("");

        try {
            const response = await fetch(`${API_BASE}/api/auth/login`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ email, password })
            });

            // Trust the HTTP status, not just the body shape.
            if (!response.ok) {
                setError(await extractError(response));
                return;
            }

            const result: AuthResponse = await response.json();

            if (!result.token || result.token.trim() === "") {
                setError("Unexpected server response");
                return;
            }

            saveAuth({
                token: result.token,
                userID: String(result.userID),
                username: result.username
            });

            navigate("/dashboard");
            return;
        }
        catch (err) {
            if (err instanceof Error) {
            setError(err.message); // shows backend error
            } else {
            setError("An unknown error occurred");
            }
        }
        finally {
            setLoading(false);
        }
    };


    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        Authenticate();
    };


    return (
        <div style={styles.container}>
            <h2 style={styles.title}>Login</h2>

            <form onSubmit={handleSubmit} style={styles.form}>
                <input
                    type="email"
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                    style={styles.input}
                />

                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                    style={styles.input}
                />

                {error && <p style={{ color: "red" }}>{error}</p>}

                <button type="submit" style={styles.button} disabled={loading}>
                    {loading ? "Logging in..." : "Login"}
                </button>

                <p style={{ marginTop: "12px" }}>
                    Don’t have an account? <Link to="/register">Register</Link>
                </p>

                <button
                    type="button"
                    style={{ ...styles.button, backgroundColor: "#28a745" }}
                    onClick={() => navigate("/register")}
                >
                    Create an Account
                </button>
            </form>
        </div>
    );
}




