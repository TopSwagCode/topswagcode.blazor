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
        private readonly IHub _sentryHub;

        public AdminController(IHub sentryHub)
        {
            _sentryHub = sentryHub;
        }

        [HttpGet]
        public AdminWelcomeMessage Get()
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");
            try
            {
                childSpan?.Finish(SpanStatus.Ok);
            }
            catch (Exception e)
            {
                childSpan?.Finish(e);
                throw;
            }

            return new AdminWelcomeMessage("Welcome Admin!");
        }


    }
}
