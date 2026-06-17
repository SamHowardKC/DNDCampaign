import React, { useState} from "react";
import type { AuthResponse } from "../../interfaces/auth/AuthInterfaces";
import type { ResultInterface } from "../../interfaces/Result";
import { styles } from "../../styles/auth/AuthStyle";
import { Link, useNavigate } from "react-router-dom";

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
            const response = await fetch("https://localhost:7228/api/auth/login", {
                method: "POST",
                credentials: "include",   // ← REQUIRED
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ email, password })
            });


            const result: ResultInterface<AuthResponse> = await response.json();

            // Business logic error (Result<T>)
            if ("success" in result && result.success === false) {
                throw new Error(result.error ?? "Login failed");
            }

            // Success response (token, userID, username)
            if ("userID" in result) {
                console.log("Login successful:", result);
                navigate("/dashboard");
                return;
            }
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




