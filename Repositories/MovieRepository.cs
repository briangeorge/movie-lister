using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;


namespace Arcadia.Challenge.Repositories
{
    public class MovieRepository
    {
        private readonly string _connectionString;

        public MovieRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<IEnumerable<Movie>> GetByListIdAsync(int listId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT
                            m.Id,
                            ImdbId,
                            Title,
                            Rated,
                            Year,
                            Genres,
                            Director,
                            Runtime,
                            mr.Rating
                        FROM MovieToMovieList mtml
                        INNER JOIN Movie m on m.Id = mtml.MovieId and mtml.MovieListId=@ListId
                        LEFT OUTER JOIN MovieRating mr
                                        on mr.MovieId = m.Id";
                return await connection.QueryAsync<Movie>(sql, new { ListId = listId });
            }
        }

        public async Task<Movie> GetOrInsertAsync(Movie movie)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT
                            m.Id,
                            ImdbId,
                            Title,
                            Rated,
                            Year,
                            Genres,
                            Director,
                            Runtime,
                            mr.Rating
                        FROM Movie m
                        LEFT OUTER JOIN MovieRating mr
                                        on mr.MovieId = m.Id
                        where m.ImdbId=@ImdbId";
                var result = await connection.QueryFirstOrDefaultAsync<Movie>(sql, new { ImdbId = movie.ImdbId });
                if (result != null)
                {
                    return result;
                }
                sql = @"INSERT INTO Movie
                        (ImdbId, Title, Year, DataPopulated)
                        VALUES
                        (@ImdbId, @Title, @Year, 0)
                        SELECT SCOPE_IDENTITY()";
                var idResult = await connection.ExecuteScalarAsync<int>(sql, movie);
                movie.Id = idResult;
                return movie;
            }
        }
    }
}