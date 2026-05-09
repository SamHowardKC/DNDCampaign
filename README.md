# DNDCampaign
A Dungeons and Dragons campaign tracker. Tracks users, campaigns, and player characters in the campaigns

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
- API Host: Railway
- Language: C#
- IDE: Visual Studio
- Database: Supabase 

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

### TABLES 
- User table
- UserID
- Username
- Password
- CreatedDate 
- IsActive

### Campaign table
- CampaignID
- CampaignName
- DungeonMasterID (UserID foreign key)
- CampaignStartDate
- CreatedDate
- IsActive

### Character Table
- CharacterID
- UserID
- CharacterName
- CharacterClass
- CreatedDate

### CharacterCampaign Table
NOTE: A character can be in multiple campaigns at once, it will have a different XP and HP in each one
- CharacterID
- CampaignID
- CharacterXP
- CharacterHP
- JoinedDate

### CampaignAuditLog table:
- CampaignID
- UserID
- EventTime
- EventTypeID

## ENUM TABLES

### CharacterClass table
- ClassID
- ClassName

### EventType table
(Events include Player dies/joins/leaves)
- EventTypeID
- EventName 

### Level table
- LevelID
- XPToReach
- LevelNumber 
