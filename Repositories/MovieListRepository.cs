using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

public class MovieListRepository
{
    private readonly string _connectionString;

    public MovieListRepository(string connectionString)
    {
        this._connectionString = connectionString;
    }

    public async Task<IEnumerable<MovieList>> GetMovieListsAsync(string userId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @"SELECT 
                            ml.Title, ml.Id, COUNT(mtml.Id), AVG(m.UserRating) 
                        FROM
                            MovieList ml
                        JOIN 
                            MovieToMovieList mtml on ml.Id = mtml.MovieListId and ml.UserId=@UserId
                            Movie m on m.Id = mtml.MovieId
                        GROUP BY
                            ml.Title, ml.Id";
            return await connection.QueryAsync<MovieList>(sql, new { userId = userId });
        }
    }

    public async Task<int> CreateMovieListAsync(MovieList data)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @"INSERT INTO MovieList 
                            (Name, UserId) VALUES(@Name, @UserId);
                        SELECT SCOPE_IDENTITY()";
            return await connection.ExecuteScalarAsync<int>(sql, data);
        }
    }
}