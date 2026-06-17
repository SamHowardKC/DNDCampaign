using BackEnd.DTOs.Campaign;
using BackEnd.ErrorHandling;
using BackEnd.Services.Campaign.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Superpower.Model;
using Superpower.Parsers;
using System.Security.Claims;

namespace BackEnd.Controllers.Campaign
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        // GET: api/campaign/user
        [Authorize]
        [HttpGet("activeuser")]
        public async Task<IActionResult> GetActiveCampaignsForUser()
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (userIdClaim == null)
                return Unauthorized(BackEnd.ErrorHandling.Result<ActiveCampaignListResponse>.Fail("User ID not found in token"));

            var userId = Guid.Parse(userIdClaim);

            var result = await _campaignService.GetActiveCampaignsForUserAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCampaign(CreateCampaignRequest request)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (userIdClaim == null)
                return Unauthorized(BackEnd.ErrorHandling.Result<ActiveCampaignListResponse>.Fail("User ID not found in token"));

            var userId = Guid.Parse(userIdClaim);

            var result = await _campaignService.CreateCampaignAsync(request, userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
