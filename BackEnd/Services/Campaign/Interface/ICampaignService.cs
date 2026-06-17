using BackEnd.ErrorHandling;
using BackEnd.DTOs.Campaign;

namespace BackEnd.Services.Campaign.Interface
{
    public interface ICampaignService
    {
        Task<Result<ActiveCampaignListResponse>> GetActiveCampaignsForUserAsync(Guid userID);
        Task<Result<ActiveCampaignListItem>> CreateCampaignAsync(CreateCampaignRequest request, Guid userID);
    }
}
