using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Arcadia.Challenge
{
    public static class GetMovieLists
    {
        [FunctionName("GetMovieLists")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //TODO: get this from ClaimsPrincipal
            var userid = "1";

            var provider = new MovieListProvider("TODO: connection string");

            var movieLists = await provider.GetMovieListsAsync(userid);

            return new OkObjectResult(movieLists);
        }
    }
}
