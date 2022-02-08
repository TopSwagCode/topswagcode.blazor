using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using AzureFunctions.OidcAuthentication;

[assembly: FunctionsStartup(typeof(Serverless.Azure.Startup))]
namespace Serverless.Azure
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOidcApiAuthorization();
        }
    }

    public class UserFunction
    {
        private readonly IApiAuthentication _apiAuthentication;

        public UserFunction(IApiAuthentication apiAuthentication)
        {
            _apiAuthentication = apiAuthentication;
        }

        [FunctionName("UserFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            // Authenticate the user
            var authResult = await _apiAuthentication.AuthenticateAsync(req.Headers);

            // Check the authentication result
            if (authResult.Failed)
            {
                return new ForbidResult(authenticationScheme: "Bearer");
            }

            var claims = authResult.User.Claims.Select(x => x.Value).ToList();
            // User is authenticated. Proceed with function logic
            string name = authResult.User.Identity.Name; // This gives us the unique user name

            string responseMessage = $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(claims);
        }
    }
}
