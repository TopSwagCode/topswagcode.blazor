using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopSwagCode.Blazor.Shared;

namespace TopSwagCode.Blazor.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ScrumController : ControllerBase
    {
        private readonly ILogger<ScrumController> _logger;

        public ScrumController(ILogger<ScrumController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ScrumTeamMember>> Get()
        {
            return new List<ScrumTeamMember>
            {
                new(Guid.NewGuid(), "Dipak"),
                new(Guid.NewGuid(), "Amarnath"),
                new(Guid.NewGuid(), "Prakash R"),
                new(Guid.NewGuid(), "Artur"),
                new(Guid.NewGuid(), "Era"),
                new(Guid.NewGuid(), "Han-Ru"),
                new(Guid.NewGuid(), "Joshua"),
                new(Guid.NewGuid(), "Mohit"),
                new(Guid.NewGuid(), "Kush"),
                new(Guid.NewGuid(), "Paranjay"),
                new(Guid.NewGuid(), "Purushotth"),
                new(Guid.NewGuid(), "Prabhat"),
                new(Guid.NewGuid(), "Swapnil"),
                new(Guid.NewGuid(), "Yeshwanth"),
            };
        }
    }
}