# Privacy Policy

## Information I Collect

### Account Information
When a user registers or logs in, the application may collect:
- Email address
- Username
- Password (hashed and never stored in plain text, although never use real passwords you use for other services as this is not a security hardened program)

### Authentication Data
Upon successful login, the server issues an authentication token stored in an HttpOnly cookie.
This token is used only to keep the user authenticated.

### Usage Data
The application may log basic technical information such as:
- IP address
- Browser type
- Timestamps
- Error logs

This is used solely for debugging and improving the project.

## How Your Information Is Used
Your information is used only for:
- Creating and managing your account
- Authenticating you securely
- Operating core features of the application
- Debugging issues and improving stability

I do not:
- Sell your data
- Share your data with third parties
- Use your data for advertising
- Track you across other websites

### Data Storage & Security
- Passwords are hashed using industry‑standard algorithms.
- Authentication tokens are stored in HttpOnly cookies to prevent JavaScript access.
- Data is stored in a secure database (e.g., Supabase/PostgreSQL).
- Access to the database is restricted and protected by environment variables.

While reasonable security measures are in place, this project is not intended for storing sensitive or confidential information.
This is a personal project provided “as‑is,” and it should not be used to store sensitive or confidential information.

### Data Retention
Data is retained only as long as necessary to operate the application.
You may request deletion of your account and associated data at any time.
However, because this is a personal project, there is no guaranteed response time.

### Third‑Party Services
- Supabase / PostgreSQL (database)
- Hosting providers (Vercel and Render)

These services may process data on my behalf, but do not have permission to use it for their own purposes.

### Changes to This Policy
This Privacy Policy may be updated as the project evolves.
Any significant changes will be reflected in this file.

### Contact
If you have questions about this Privacy Policy or want your data removed, please reach out via GitHub Issues