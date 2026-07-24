import React, { useState} from "react";
import type { AuthResponse } from "../../interfaces/auth/AuthInterfaces";
import { CheckEmailFormat, CheckUsername, CheckPasswordStrength } from "../../validation/auth/AuthValidation";
import { styles } from "../../styles/auth/AuthStyle";
import { useNavigate } from "react-router-dom";
import { API_BASE } from "../../api/config";
import { extractError } from "../../api/apiClient";
import { saveAuth } from "../../auth/auth";

export default function Register() {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const RegisterUser = async () => {
        setLoading(true);
        setError("");

        if (!ValidateInputFields()) {
            setLoading(false);
            return;
        }

        try {
            const response = await fetch(`${API_BASE}/api/auth/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password, username, confirmPassword })
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

    const ValidateInputFields = (): boolean => {
        // Add more validation logic here
        if (CheckEmailFormat(email).isValid === false) {
            setError(CheckEmailFormat(email).message);
            return false;
        }

        if (CheckUsername(username).isValid === false) {
            setError(CheckUsername(username).message);
            return false;
        }

        if (CheckPasswordStrength(password, confirmPassword).isStrong === false) {
            setError(CheckPasswordStrength(password, confirmPassword).message);
            return false;
        }

        if (!email || !password || !username || !confirmPassword) {
            setError("All fields are required.");
            return false;
        }

        return true;
    }

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        RegisterUser();
    };

    return (
        <div style={styles.container}>
            <h2 style={styles.title}>Register</h2>

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
                    type="text"
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
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
                <input
                    type="password"
                    placeholder="Confirm Password"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    required
                    style={styles.input}
                />

                {error && <p style={{ color: "red" }}>{error}</p>}

                <button type="submit" style={styles.button} disabled={loading}>
                    {loading ? "Registering..." : "Register"}
                </button>
            </form>
        </div>
    )
}