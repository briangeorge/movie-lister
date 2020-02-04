#nullable enable
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using Arcadia.Challenge.Models;
using Arcadia.Challenge.Repositories;

namespace Arcadia.Challenge
{
    public static class SearchMovies
    {
        [FunctionName("SearchMovies")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "SearchMovies/{searchValue}")] HttpRequest req,
            string? searchValue,
            ILogger log)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return new BadRequestObjectResult("Must Provide Search Value.");
            }

            string omdbResult = string.Empty;
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"http://www.omdbapi.com/?type=movie&s={searchValue}&apiKey=a3762861");
                if (result == null || result.Content == null)
                {
                    return new BadRequestObjectResult("An error ocurred in the OMDB API");
                }
                omdbResult = await result.Content.ReadAsStringAsync();
            }

            var data = JsonConvert.DeserializeObject<SearchResult>(omdbResult);
            if (data == null || data.Search == null)
            {
                return new BadRequestObjectResult("An error ocurred in the OMDB API");
            }
            var movieRepository = new MovieRepository(ConnectionStringRepository.GetSqlAzureConnectionString("SQLConnectionString"));

            var movies = new List<Movie>();
            foreach (var movie in data.Search)
            {
                movies.Add(await movieRepository.GetOrInsertAsync(new Movie
                {
                    ImdbId = movie.imdbID,
                    Title = movie.Title,
                    Year = movie.Year
                }));
            }
            return new OkObjectResult(movies);
        }
    }
}
