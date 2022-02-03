using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jdenticon;

namespace TopSwagCode.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdenticonController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get(string name, int size)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var icon = Jdenticon.Identicon.FromValue(name, size);

                await icon.SaveAsPngAsync(ms);

                ms.Position = 0;
                return File(ms.ToArray(), "image/png");
            }
        }
    }
}
