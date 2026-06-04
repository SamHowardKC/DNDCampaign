export function CheckPasswordStrength(password: string, confirmpassword: string): {
    isStrong: boolean;
    message: string;
} {
    // Add more logic
    if (password.length === 0)
        return { isStrong: false, message: "Password cannot be empty." };

    if (password.length < 6)
        return { isStrong: false, message: "Password must be at least 6 characters long." };

    if (password.length > 128)
        return { isStrong: false, message: "Password cannot be longer than 128 characters." };

    if (password.includes(" "))
        return { isStrong: false, message: "Password cannot contain spaces." };
    
    if (!/[A-Z]/.test(password)) return { isStrong: false, message: "Password must contain at least one uppercase letter." };
    if (!/[a-z]/.test(password)) return { isStrong: false, message: "Password must contain at least one lowercase letter." };
    if (!/[0-9]/.test(password)) return { isStrong: false, message: "Password must contain at least one digit." };
    
    const requiredChar = /[^A-Za-z0-9]/;
    if (!requiredChar.test(password)) {
        return { isStrong: false, message: "Password must contain a special character." };
    }

    if (confirmpassword.length === 0)
        return { isStrong: false, message: "Please confirm your password." };

    if (password !== confirmpassword)
        return { isStrong: false, message: "Passwords do not match." };

    return { isStrong: true , message: "" };
}

export function CheckEmailFormat(email: string) {
    if (!email)
        return { isValid: false, message: "Email is required." };

    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!regex.test(email))
        return { isValid: false, message: "Invalid email format." };

    return { isValid: true, message: "" };
}

export function CheckUsername (username: string): {
    isValid: boolean;
    message: string;
} {
    if (username.length === 0)
        return { isValid: false, message: "Username cannot be empty." };
    if (username.length > 50)
        return { isValid: false, message: "Username cannot be longer than 50 characters." };
    if (username.length < 3)
        return { isValid: false, message: "Username must be at least 3 characters long." };

    return { isValid: true, message: "" };
}