import React, { useState} from "react";
import type { AuthResponse } from "../../interfaces/auth/AuthInterfaces";
import { CheckEmailFormat, CheckUsername, CheckPasswordStrength } from "../../validation/auth/AuthValidation";
import { styles } from "../../styles/auth/AuthStyle";
import { useNavigate } from "react-router-dom";

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
            const response = await fetch("https://dndcampaign.onrender.com/api/auth/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({ email, password, username, confirmPassword })
            });

            const result: AuthResponse = await response.json();

            // Backend business logic error
            if (result.error != "Default Error") {
                setError(result.error);
                return;
            }

            // Success response (token, userID, username)
            if (result.token && typeof result.token === "string" && result.token.trim() !== "") {
                console.log("Login successful:", result);

                localStorage.setItem("jwt", result.token);
                localStorage.setItem("userID", result.userID);
                localStorage.setItem("username", result.username);

                navigate("/dashboard");
                return;
            }

            // Fallback (should never happen)
            throw new Error("Unexpected server response");
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