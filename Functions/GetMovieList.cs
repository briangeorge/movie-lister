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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GetMovieList/{id}")] HttpRequest req,
            int? id,
            ILogger log)
        {
            //TODO: get this from ClaimsPrincipal
            var userid = "1";

            if (id == null)
            {
                return new BadRequestObjectResult("Must Provide Id.");
            }

            var listRepository = new MovieListRepository(ConnectionStringRepository.GetSqlAzureConnectionString("SQLConnectionString"));
            var movieRepository = new MovieRepository(ConnectionStringRepository.GetSqlAzureConnectionString("SQLConnectionString"));
            MovieList data = await listRepository.GetAsync(userid, id.Value);

            if (data == null)
            {
                return new BadRequestObjectResult("Invalid Id Provided.");
            }

            data.Movies = await movieRepository.GetByListIdAsync(data.Id);

            return new OkObjectResult(data);
        }
    }
}
