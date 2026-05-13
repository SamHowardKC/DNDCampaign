# Tables

## Main Tables

### User table
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
- ClassID
- CharacterName
- CreatedDate

### CharacterCampaign Table
NOTE: A character can be in multiple campaigns at once, it will have a different XP and HP in each one
- CharacterID
- CampaignID
- CharacterXP
- CharacterHP
- CharacterMaxHP
- JoinedDate

### CampaignAuditLog table:
- CampaignID
- UserID
- EventTime
- EventTypeID

## LOOKUP TABLES

### CharacterClass table
NOTE: Every time a character levels up, their hp will increase by a random number. The maximum value of this increase is CharacterClass.IncreaseToMaxHpPerLevel
- ClassID
- ClassName
- IncreaseToMaxHpPerLevel

### EventType table
(Events include Player dies/joins/leaves)
- EventTypeID
- EventName 

### Level table
- LevelID
- XPToReach
- LevelNumber 
