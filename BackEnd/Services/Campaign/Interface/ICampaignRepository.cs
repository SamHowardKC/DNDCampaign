namespace BackEnd.Services.Campaign.Interface
{
    public interface ICampaignRepository
    {
        Task<List<Entities.Campaign.Campaign>> GetByUserAsync(Guid userId);
    }
}
