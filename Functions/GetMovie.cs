using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Arcadia.Challenge.Repositories;
using System.Net.Http;

namespace Arcadia.Challenge
{
    public static class GetMovie
    {
        [FunctionName("GetMovie")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GetMovie/{id}")] HttpRequest req,
            int? id,
            ILogger log)
        {
            //TODO: get this from ClaimsPrincipal
            var userid = "1";

            if (id == null)
            {
                return new BadRequestObjectResult("Must Provide Id.");
            }

            var movieRepository = new MovieRepository(ConnectionStringRepository.GetSqlAzureConnectionString("SQLConnectionString"));

            var movie = await movieRepository.GetMovieForUserAsync(id, userid);

            if (movie == null)
            {
                return new BadRequestObjectResult("Invalid Id.");
            }
            if (!movie.DataPopulated)
            {
                string omdbResult = string.Empty;
                using (var client = new HttpClient())
                {
                    var apiKey = Environment.GetEnvironmentVariable("OmdbAPIKey", EnvironmentVariableTarget.Process);
                    var result = await client.GetAsync($"http://www.omdbapi.com/?type=movie&i={movie.ImdbId}&apiKey={apiKey}");
                    if (result == null || result.Content == null)
                    {
                        return new BadRequestObjectResult("An error ocurred in the OMDB API");
                    }
                    omdbResult = await result.Content.ReadAsStringAsync();
                }

                dynamic data = JsonConvert.DeserializeObject(omdbResult);
                if (data == null || data?.Title == null)
                {
                    return new BadRequestObjectResult("An error ocurred in the OMDB API");
                }
                movie.ImdbId = data.imdbID;
                movie.DataPopulated = true;
                movie.Director = data.Director;
                movie.Genres = data.Genre;
                movie.Rated = data.Rated;
                movie.Runtime = data.Runtime;
                movie.Title = data.Title;
                movie.Year = data.Year;
                movie.Id = id.Value;
                await movieRepository.UpdateDataAsync(movie);
            }

            return new OkObjectResult(movie);
        }
    }
}
