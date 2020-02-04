using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

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
}
