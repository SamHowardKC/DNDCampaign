namespace BackEnd.Tests.Services;

using BackEnd.Entities.Character;
using BackEnd.Services.Character.Implementation;
using BackEnd.Services.Character.Interface;
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
        var campaign = Assert.Single(returnedCharacter.Campaigns.Campaigns);

        Assert.Equal(xp, campaign.Xp);
        Assert.Equal(hp, campaign.Hp);
        Assert.Equal(maxHp, campaign.MaxHp);
    }

    // Existing Character with no campaigns
    [Fact]
    public async Task GetCharactersForUserAsync_CharacterHasNoCampaigns_ReturnsEmptyCampaignList()
    {

    }

    // No Characters, Existing user
    [Fact]
    public async Task GetCharactersForUserAsync_UserHasNoCharacters_ReturnsEmptyList()
    {

    }

    // No Characters, No User
    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("3f2b8c1e-9a4d-4e6f-b7c2-1d5e8a9f0b3c")]
    public async Task GetCharactersForUserAsync_UnknownUser_ReturnsEmptyList(string userId)
    {

    }

    public async Task GetCharactersForUserAsync_CharacterInTwoCampaigns_ReturnsCharacter()
    {

    }

}
