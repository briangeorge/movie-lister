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

namespace Arcadia.Challenge
{
    public static class RateMovie
    {
        [FunctionName("RateMovie")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "RateMovie/{id}")] HttpRequest req,
            int? id,
            ILogger log)
        {
            //TODO: get this from ClaimsPrincipal
            var userid = "1";

            if (id == null)
            {
                return new BadRequestObjectResult("Must Provide Id.");
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            if (data == null || data.rating == null)
            {
                return new BadRequestObjectResult("Must provide rating.");
            }

            var movieRepository = new MovieRepository(ConnectionStringRepository.GetSqlAzureConnectionString("SQLConnectionString"));

            try
            {
                await movieRepository.SaveRatingAsync(userid, id, (int)data.rating);
                return new OkObjectResult(new { message = "Rating saved!" });
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
