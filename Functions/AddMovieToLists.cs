#nullable enable
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Arcadia.Challenge.Repositories;
using System.Collections.Generic;

namespace Arcadia.Challenge
{
    public static class AddMovieToLists
    {
        [FunctionName("AddMovieToLists")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //TODO: get this from ClaimsPrincipal
            var userid = "1";

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var movieId = data?.movieId;

            if (movieId == null)
            {
                return new BadRequestObjectResult("Must Provide Id.");
            }

            var lists = data?.lists?.ToObject<List<int>>();
            if (lists == null || lists?.Count == 0)
            {
                return new OkObjectResult("Added to 0 lists.");
            }
            var movieListRepository = new MovieListRepository(ConnectionStringRepository.GetSqlAzureConnectionString("SQLConnectionString"));

            await movieListRepository.AddMovieToListsAsync((int)movieId, lists, userid);

            return new OkObjectResult(new { Message = $"Added to {lists?.Count} lists." });
        }
    }
}
