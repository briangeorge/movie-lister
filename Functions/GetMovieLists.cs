using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using Arcadia.Challenge.Repositories;

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

            var repository = new MovieListRepository(ConnectionStringRepository.GetSqlAzureConnectionString("SQLConnectionString"));

            try
            {
                var movieLists = await repository.GetAsync(userid);
                return new OkObjectResult(movieLists);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }

        }
    }
}
