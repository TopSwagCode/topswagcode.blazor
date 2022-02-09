using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopSwagCode.Blazor.Shared;

namespace TopSwagCode.Blazor.Server.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {

        [HttpGet]
        public AdminWelcomeMessage Get()
        {
            return new AdminWelcomeMessage("Welcome Admin!");
        }


    }
}
