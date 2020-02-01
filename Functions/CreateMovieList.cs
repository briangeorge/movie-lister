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
    public static class CreateMovieList
    {
        [FunctionName("CreateMovieList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //TODO: get this from ClaimsPrincipal
            var userid = "1";

            var repository = new MovieListRepository("TODO: connection string");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<MovieList>(requestBody);
            data.UserId = userid;

            //eventually this would verify a unique name

            //TODO: add error handling
            var newMovieListId = await repository.CreateMovieListAsync(data);

            return new OkObjectResult(newMovieListId);
        }
    }
}
