# REPORTS
- All reports are (currently intended) for developer usage, not user usage
- They will all be written in raw sql for performance

## CampaignCharacterSheet
- generate all the character sheets for one campaign and their level and HP%, in order of highest to lowest xp

##  CharacterProgress
- makes a leaderboard of the top 10 characters globally in terms of combining xp and activity in a single score

## ClassDistribution
- Shows most popular classes per user that is a dungeon master
- Most popular classes overall throughout system with trend detection

## DataQualityCheck
- verifies all data in a campaign. 
- No characters with mismatched level to xp
- campaigns with no activity in last 90 days
- any orphaned records