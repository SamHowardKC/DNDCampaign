namespace BackEnd.Tests.Services;

using BackEnd.Entities.Campaign;
using BackEnd.Entities.Character;
using BackEnd.Services.Character.Implementation;
using BackEnd.Services.Character.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;

public class CharacterServiceTests
{
    // Existing Characters, Existing User
    // characterCount: how many characters the mocked repository returns for the user
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public async Task GetCharactersForUserAsync_UserHasCharacters_ReturnsMappedCharacters(int characterCount)
    {
        var userId = Guid.NewGuid();

        var characterList = new List<BackEnd.Entities.Character.Character> { };

        for (int i = 0; i < characterCount; i++)
        {
            characterList.Add(
                new BackEnd.Entities.Character.Character
                {
                    Name = "NameExample" + i,
                    ClassID = Guid.NewGuid(),
                    CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero).AddDays(i * 1000)
                }
            );
        }

        var repositoryMock = new Mock<ICharacterRepository>();

        repositoryMock
            .Setup(r => r.GetByUserAsync(userId))
            .ReturnsAsync(characterList);

        var service = new CharacterService(repositoryMock.Object);

        var result = await service.GetCharactersForUserAsync(userId);

        Assert.True(result.Success);
        Assert.Equal(characterCount, result.Data.Characters.Count);

        foreach (var character in characterList)
        {
            var returnedCharacter = Assert.Single(result.Data.Characters, c => c.Id == character.Id);

            Assert.Equal(character.Name, returnedCharacter.Name);
            Assert.Equal(character.ClassID, returnedCharacter.ClassID);
            Assert.Equal(character.CreatedAt, returnedCharacter.CreatedAt);
        }
    }

    // Existing Character with a campaign: checks CharacterXP/HP/MaxHP map to Xp/Hp/MaxHp
    [Theory]
    [InlineData(0, 9, 10)]
    [InlineData(300, 5, 12)]
    [InlineData(6500, 0, 45)]
    public async Task GetCharactersForUserAsync_CharacterInCampaign_MapsCampaignStats(int xp, int hp, int maxHp)
    {
        var userId = Guid.NewGuid();

        var character = new BackEnd.Entities.Character.Character
        {
            Name = "Test Character",
            UserID = userId,
            CharacterCampaigns = new List<CharacterCampaign>
            {
                new CharacterCampaign
                {
                    CharacterXP = xp,
                    CharacterHP = hp,
                    CharacterMaxHP = maxHp
                }
            }
        };

        var repositoryMock = new Mock<ICharacterRepository>();
        repositoryMock
            .Setup(r => r.GetByUserAsync(userId))
            .ReturnsAsync(new List<BackEnd.Entities.Character.Character> { character });

        var service = new CharacterService(repositoryMock.Object);

        var result = await service.GetCharactersForUserAsync(userId);

        Assert.True(result.Success);

        var returnedCharacter = Assert.Single(result.Data.Characters);
        var campaign = Assert.Single(returnedCharacter.Campaigns);

        Assert.Equal(xp, campaign.Xp);
        Assert.Equal(hp, campaign.Hp);
        Assert.Equal(maxHp, campaign.MaxHp);
    }

    // Existing Character with no campaigns
    [Fact]
    public async Task GetCharactersForUserAsync_CharacterHasNoCampaigns_ReturnsEmptyCampaignList()
    {
        var userId = Guid.NewGuid();

        var character = new BackEnd.Entities.Character.Character
        {
            Name = "Test Character",
            UserID = userId,
            ClassID = Guid.NewGuid(),
            CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
        };

        var repositoryMock = new Mock<ICharacterRepository>();
        repositoryMock
            .Setup(r => r.GetByUserAsync(userId))
            .ReturnsAsync(new List<BackEnd.Entities.Character.Character> { character });

        var service = new CharacterService(repositoryMock.Object);

        var result = await service.GetCharactersForUserAsync(userId);

        Assert.True(result.Success);

        var returnedCharacter = Assert.Single(result.Data.Characters, c => c.Id == character.Id);

        Assert.Empty(returnedCharacter.Campaigns);
    }

    // No Characters, Existing user
    [Fact]
    public async Task GetCharactersForUserAsync_UserHasNoCharacters_ReturnsEmptyList()
    {
        var userId = Guid.NewGuid();


        var repositoryMock = new Mock<ICharacterRepository>();
        repositoryMock
            .Setup(r => r.GetByUserAsync(userId))
            .ReturnsAsync(new List<BackEnd.Entities.Character.Character> ());

        var service = new CharacterService(repositoryMock.Object);

        var result = await service.GetCharactersForUserAsync(userId);

        Assert.True(result.Success);
        Assert.Empty(result.Data.Characters);
    }


    //character in multiple campiagns
    [Fact]
    public async Task GetCharactersForUserAsync_CharacterInTwoCampaigns_ReturnsCharacter()
    {
        var userId = Guid.NewGuid();
        int campaignCount = 3;
        var campaignList = new List<Campaign>();

        var character = new Character
        {
            ClassID = Guid.NewGuid(),
            CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            Name = "CharacterNameExample",
            Id = Guid.NewGuid(),
            UserID = userId
        };

        campaignList.Add(new Campaign
        {
            CreatedAt = new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero),
            DungeonMasterID = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            IsActive = true,
            IsEnded = true,
            Name = "Campaign 1",
            CharacterCampaigns = new List<CharacterCampaign> { }
        });

        for (int i = 0; i < campaignCount; i++)
        {
            campaignList.Add(new Campaign
            {
                CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero).AddDays(i * 1000),
                IsActive = true,
            });
        };

        //var campaign = new Campaign
        //{
        //    CreatedAt = new DateTimeOffset(2027, 1, 1, 0, 0, 0, TimeSpan.Zero),
        //    DungeonMasterID = Guid.NewGuid(),
        //    Id = Guid.NewGuid(),
        //    IsActive = true,
        //    IsEnded = true,
        //}

        //for (int i = 0; i < campaignCount; i++)
        //{
        //    campaignList.Add(new CharacterCampaign
        //    {
        //        CampaignID = Guid.NewGuid(),
        //        CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero).AddDays(i*1000),
        //        Character = character,
        //        Campaign = 
        //    })
        //};

        //var repositoryMock = new Mock<ICharacterRepository>();
        //repositoryMock
        //    .Setup(r => r.GetByUserAsync(userId))
        //    .ReturnsAsync(characterList);

        //var service = new CharacterService(repositoryMock.Object);

        //var result = await service.GetCharactersForUserAsync(userId);

        //Assert.True(result.Success);

        //Assert.Equal(characterCount, result.Data.Characters.Count);

        //foreach (var character in characterList)
        //{
        //    var returnedCharacter = Assert.Single(result.Data.Characters, c => c.Id == character.Id);

        //    Assert.Equal(character.Name, returnedCharacter.Name);
        //    Assert.Equal(character.ClassID, returnedCharacter.ClassID);
        //    Assert.Equal(character.CreatedAt, returnedCharacter.CreatedAt);
        //}

    }

}
