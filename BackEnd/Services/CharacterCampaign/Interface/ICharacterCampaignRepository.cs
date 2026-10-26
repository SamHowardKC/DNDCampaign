namespace BackEnd.Services.CharacterCampaign.Interface
{
    public interface ICharacterCampaignRepository
    {
        Task<List<Entities.Character.CharacterCampaign>> GetByCampaignAsync(Guid campaignId);
    }
}
