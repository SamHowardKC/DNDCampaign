using BackEnd.DTOs.Campaign;
using BackEnd.Entities.Campaign;
using BackEnd.Entities.Auth;
using BackEnd.ErrorHandling;
using BackEnd.Services.Campaign.Interface;
using BackEnd.Services.Character.Interface;
using BackEnd.Services.Auth;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;
using System.Runtime.InteropServices;
using BackEnd.Services.Auth.Interface;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using BackEnd.Services.CharacterCampaign.Interface;
using BackEnd.Services.Level;

namespace BackEnd.Services.Campaign.Implementation
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICharacterCampaignRepository _characterCampaignRepository;
        public CampaignService(ICampaignRepository campaignRepository,
            ICharacterRepository characterRepository,
            IUserRepository userRepository,
            ICharacterCampaignRepository characterCampaignRepository)
        {
            _campaignRepository = campaignRepository;
            _characterRepository = characterRepository;
            _userRepository = userRepository;
            _characterCampaignRepository = characterCampaignRepository;
        }

        /*
         * to do:
         * this is a bit of a mess this method (understatement), specifically the average level stuff so here's the plan, in order of which to do first
         *      So the goal is to reduce total network calls and make it less code, these should be the network calls, once for each of these tables
         *                  Campaign (Will need a new method in the service file that does a query where it gets the Campaign records where the player is the DM OR a player, because currently it does 2 campaign calls)
         *                  Level
         *                  CharacterCampaign (All Character Campaigns associated with the Campaigns that the Campaign query got)
         *                  User
         *                  
         *      1) before the foreach campaign loop it should grab the entire level table (only 20 records ever), and every character campaign of the campaigns in campaignItems.
         *      this will reduce the amount of network calls to 3, once for character campaigns (Get, once for levels, once for campaigns
         *      2) investigate whether we should include the IsDungeonMaster in GetByCharactersAsync, if we should it should probably mean we make another campaign service call for this, because currently getting the user inside the loop is not ideal as it drastically increases network calls
         *      3) inside the charactercampaign loop, do a lookup against the level table we already got if the charactercampaign is for the player, so we can assign level.
         *  
         *  reminder: don't forget that we need to reset our variables at the end/beginning of the loop
         */
        public async Task<Result<ActiveCampaignListResponse>> GetActiveCampaignsForUserAsync(Guid userID)
        {
            int totalLevel = 0;
            int xp;
            decimal averageLevel;
            int level;
            int numCharacters = 1;
            int userLevel = 0; // the level of the logged in player

            var user = await _userRepository.GetByIdAsync(userID);
            if (user == null)
                return Result<ActiveCampaignListResponse>.Fail("No user found.");

            // campaigns where user is dm
            var dmCampaignList = await _campaignRepository.GetByDMAsync(userID);
            if (dmCampaignList == null)
                return Result<ActiveCampaignListResponse>.Fail("No campaigns found for the user.");

            // all characters of user
            var characterList = await _characterRepository.GetByUserAsync(userID);
            var myCharacterIds = characterList.Select(c => c.Id).ToHashSet();

            // all campaigns where the user has a character
            var playerCampaignList = await _campaignRepository.GetByCharactersAsync(characterList);

            // list of active campaigns where user is either a player or dm
            var campaignList = dmCampaignList
                .Union(playerCampaignList)
                .Distinct()
                .Where(c => c.IsActive && !c.IsEnded)
                .ToList();

            var campaignItems = new List<ActiveCampaignListItem>();


            foreach (var c in campaignList)
            {
                var isDungeonMaster = c.DungeonMasterID == userID;

                // The requesting user's username is only correct when they ARE the DM;
                // for campaigns they're a player in, look up the actual DM.
                var dungeonMasterName = isDungeonMaster
                    ? user.Username
                    : (await _userRepository.GetByIdAsync(c.DungeonMasterID))?.Username ?? "Unknown";

                // Calculate average level
                // O(N^2) is fine here because we will be capping players at 10 campaigns, and realistically no one is in more than 3 at once
                var characterCampaigns = await _characterCampaignRepository.GetByCampaignAsync(c.Id);
                foreach (var charCampaigns in characterCampaigns)
                {
                    xp = charCampaigns.CharacterXP;
                    level = await _levelRepository.GetLevelByXp(xp);
                    numCharacters = numCharacters + 1;
                    totalLevel = totalLevel + level;

                    foreach (var CharId in myCharacterIds)
                    {
                        if (CharId == charCampaigns.CharacterID)
                            userLevel = level;
                    }
                }
                averageLevel = totalLevel / numCharacters;

                // add a function which calculates average player level
                campaignItems.Add(new ActiveCampaignListItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    DungeonMasterID = c.DungeonMasterID,
                    DungeonMasterName = dungeonMasterName,
                    CreatedAt = c.CreatedAt,
                    IsDungeonMaster = isDungeonMaster,
                    NumberOfPlayers = c.CharacterCampaigns.Count,
                    AveragePlayerLevel = averageLevel,
                    PlayerLevel = userLevel
                });
                
            }

            var response = new ActiveCampaignListResponse
            {
                Campaigns = campaignItems
            };

            return Result<ActiveCampaignListResponse>.Ok(response);
        }

        public async Task<Result<ActiveCampaignListItem>> CreateCampaignAsync(CreateCampaignRequest request, Guid userID)
        {
            int count;

            var user = await _userRepository.GetByIdAsync(userID);
            if (user == null)
                return Result<ActiveCampaignListItem>.Fail("No user found.");

            var existingCampaign = await _campaignRepository.GetByDMAsync(userID);

            if (existingCampaign != null)
            {
                count = existingCampaign.Count;
                request.Name = 
                    request.Name 
                    + "-" 
                    + (count + 1).ToString();
            }

            var newCampaign = new BackEnd.Entities.Campaign.Campaign
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                DungeonMasterID = userID,
                IsActive = true,
                IsEnded = false,
                CreatedAt = DateTime.UtcNow
            };
            var createdCampaign = await _campaignRepository.AddAsync(newCampaign);
            if (createdCampaign == null)
            {
                return Result<ActiveCampaignListItem>.Fail("Failed to create campaign.");
            }
            var response = new ActiveCampaignListItem
            {
                Id = createdCampaign.Id,
                Name = createdCampaign.Name,
                DungeonMasterName = user.Username,
                DungeonMasterID = createdCampaign.DungeonMasterID,
                CreatedAt = createdCampaign.CreatedAt,
                PlayerLevel = 0,
                AveragePlayerLevel = 0,
                NumberOfPlayers = 0,
                IsDungeonMaster = true
            };
            return Result<ActiveCampaignListItem>.Ok(response);
        }
    }
}
