# DNDCampaign
A Dungeons and Dragons campaign tracker. Tracks users, campaigns, and player characters in the campaigns

## SECURITY WARNING
Do NOT use real passwords or usernames that you use for other services.
 
This project is not designed for production use without additional security hardening.  
The maintainers are not responsible for any security breaches or data loss resulting from the use of this software.

## DOCUMENTATION
- [Disclaimer](./DISCLAIMER.md)
- [Database Tables](./TABLES.md)
- [Database ERD](./DatabaseERD.svg)

## FEATURES
- User authentication (login/register)
- Users can create campaigns and invite other users
- Users can create and delete characters, then join campaigns using a randomly generated invite code
- Many-to-many relationship between characters and campaigns via the `CharacterCampaign` table
- JWT authentication with role-based access control
- Automatic level calculation based on XP
- Automatic death detection when HP reaches 0
- WebSocket-powered live updates when players join or leave a campaign (visible to all connected users)

## TECH STACK

### BACK END
- API Host: Render
- Language: C#
- IDE: Visual Studio
- Database: Supabase 
- Entity Relationship Diagram: dbdiagram.io

### FRONT END
- Framework: React 
- Hosting: Vercel
- Language: Typescript
- IDE: VSCode

### UTILS
- Database tool - SQL/Postgres extension in VS Code
- API testing - postman 
- Version control - GitHub/sourcetree 

## WEB PAGES

### Login/Register Page
- Enter Username
- Enter Password
- (If in Register Mode) Re-enter Password
- Register/Login button

### Dashboard 
- Shows List of campaigns
- Quick Links to Players and Characters
- List of campaign links
- Create Character/Campaign button
- Delete User

### Campaign Page (Edit/Display campaign details)
- List of character links, their XP, HP, Class, Name, Joined Date, puts them in a separate section if they're dead (<= 0hp)
- Generate Invite Link button
- Campaign Description
- Campaign Name
- End Campaign button

### Character Page
- Displays character name, class, created date
- Delete Character button 
- Join Campaign button
- Leave Campaign button

### Create Character Page
- Enter Character details
