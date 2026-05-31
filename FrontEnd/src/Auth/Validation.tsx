export function CheckPasswordStrength(password: string): {
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

    return { isStrong: true , message: "" };
}

// I included this so it (as closely as I can) matches the .net email validation rules"
export function CheckEmailFormat(email: string): {
    isValid: boolean;
    message: string;
} {

    if (email.length > 254) {
        return { isValid: false, message: "Email cannot be longer than 254 characters." };
    }

    if (email.includes(" ")) {
        return { isValid: false, message: "Email cannot contain spaces." };
    }

    const parts = email.split("@");
    if (parts.length !== 2) {
        return { isValid: false, message: "Email must contain exactly one '@' symbol." };
    }

    const [local,domain] = parts;
    
    //local rules
    if (local.length === 0 || !local) return { isValid: false, message: "Email local part cannot be empty." };
    if (local.startsWith(".") || local.endsWith(".")) return { isValid: false, message: "Email local part cannot start or end with a period." };
    if (local.includes("..")) return { isValid: false, message: "Email local part cannot contain consecutive periods." };

    // domain rules
    if (domain.length === 0 || !domain) return { isValid: false, message: "Email domain part cannot be empty." };
    if (domain.startsWith("-") || domain.endsWith("-")) return { isValid: false, message: "Email domain part cannot start or end with a hyphen." };
    if (!domain.includes(".")) return { isValid: false, message: "Email domain part must contain at least one period." };
    if (domain.includes("..")) return { isValid: false, message: "Email domain part cannot contain consecutive periods." };

    const illegal = /[(),:;<>[\]\s]/;
    if (illegal.test(local) && !(local.startsWith('"') && local.endsWith('"'))) {
        return { isValid: false, message: "Email contains illegal characters." };
    }

    // domain labels rules
    const labels = domain.split(".");
    for (const label of labels) {
        if (!label) return { isValid: false, message: "Email domain part contains an empty label." };
        if (label.startsWith("-") || label.endsWith("-")) return { isValid: false, message: "Email domain part cannot contain labels that start or end with a hyphen." };
    }

    // TLD must be at least 2 chars
    const tld = labels[labels.length - 1];
    if (tld.length < 2) return { isValid: false, message: "Email domain part must have a valid top-level domain." };
    

    return { isValid: true, message: "" };
}