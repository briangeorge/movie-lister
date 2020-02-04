using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Arcadia.Challenge.Repositories;
using Arcadia.Challenge.Models;

namespace Arcadia.Challenge
{
    public static class CreateMovieList
    {
        [FunctionName("CreateMovieList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //TODO: get this from ClaimsPrincipal
            var userid = "1";

            var repository = new MovieListRepository(ConnectionStringRepository.GetSqlAzureConnectionString("SQLConnectionString"));

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<MovieList>(requestBody);
            if (string.IsNullOrEmpty(data.Name))
            {
                return new BadRequestObjectResult("Name is required");
            }
            data.UserId = userid;

            //eventually this would verify a unique name

            //TODO: add error handling
            var newMovieListId = await repository.CreateMovieListAsync(data);
            data.Id = newMovieListId;

            return new OkObjectResult(data);
        }
    }
}
