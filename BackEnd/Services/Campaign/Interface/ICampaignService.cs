using BackEnd.ErrorHandling;
using BackEnd.DTOs.Campaign;

namespace BackEnd.Services.Campaign.Interface
{
    public interface ICampaignService
    {
        Task<Result<CampaignListResponse>> GetCampaignsForUserAsync(Guid userID);
    }
}
