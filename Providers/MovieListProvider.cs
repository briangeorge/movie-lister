using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

public class MovieListProvider
{
    private readonly string _connectionString;

    public MovieListProvider(string connectionString)
    {
        this._connectionString = connectionString;
    }

    public async Task<IEnumerable<MovieList>> GetMovieListsAsync(string userId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @"SELECT 
                            *
                        FROM
                            MovieList
                        WHERE
                            UserId=@UserId";
            return await connection.QueryAsync<MovieList>(sql, new { userId = userId });
        }
    }
}