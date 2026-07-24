using BackEnd.DTOs.Campaign;
using BackEnd.ErrorHandling;
using BackEnd.Services.Campaign.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace BackEnd.Controllers.Campaign
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("Fixed")]
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
            if (!TryGetUserId(out var userId))
                return Unauthorized(Result<ActiveCampaignListResponse>.Fail("User ID not found in token"));

            var result = await _campaignService.GetActiveCampaignsForUserAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCampaign(CreateCampaignRequest request)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(Result<ActiveCampaignListResponse>.Fail("User ID not found in token"));

            var result = await _campaignService.CreateCampaignAsync(request, userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Pulls the authenticated user's id out of the validated token. We emit the id
        // as the standard "sub" claim (see JwtProvider); the NameIdentifier fallback
        // keeps this working if the claim-type mapping ever changes.
        private bool TryGetUserId(out Guid userId)
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return Guid.TryParse(claim, out userId);
        }

    }
}
