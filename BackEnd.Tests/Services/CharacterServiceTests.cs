namespace BackEnd.Tests.Services;

using Xunit;
using BackEnd.Services.Character.Implementation;
using BackEnd.Services.Character.Interface;

public class CharacterServiceTests
{
    // Existing Characters, Existing User
    // characterCount: how many characters the mocked repository returns for the user
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public async Task GetCharactersForUserAsync_UserHasCharacters_ReturnsMappedCharacters(int characterCount)
    {

    }

    // Existing Character with a campaign: checks CharacterXP/HP/MaxHP map to Xp/Hp/MaxHp
    [Theory]
    [InlineData(0, 9, 10)]
    [InlineData(300, 5, 12)]
    [InlineData(6500, 0, 45)]
    public async Task GetCharactersForUserAsync_CharacterInCampaign_MapsCampaignStats(int xp, int hp, int maxHp)
    {

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
