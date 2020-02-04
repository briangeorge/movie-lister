using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Arcadia.Challenge
{
    public static class GetMovieList
    {
        [FunctionName("GetMovieList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            int? id,
            ILogger log)
        {
            //TODO: get this from ClaimsPrincipal
            var userid = "1";

            if (id == null)
            {
                return new BadRequestObjectResult("Must Provide Id.");
            }

            var repository = new MovieListRepository(ConnectionStringRepository.GetSqlAzureConnectionString("SQLConnectionString"));

            MovieList data = await repository.GetAsync(userid, id.Value);

            return data != null
                ? (ActionResult)new OkObjectResult(data)
                : new BadRequestObjectResult("Invalid Id Provided.");
        }
    }
}
