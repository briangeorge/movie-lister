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

    public async Task<IEnumerable<MovieList>> GetAsync(string userId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @"SELECT 
                            ml.Title as Name, ml.Id, COUNT(mtml.Id) AS MovieCount, AVG(m.UserRating) AS AverageRating
                        FROM
                            MovieList ml
                        LEFT OUTER JOIN MovieToMovieList mtml on ml.Id = mtml.MovieListId 
                                        and ml.UserId=@userId
                        LEFT OUTER JOIN Movie m on m.Id = mtml.MovieId
                        GROUP BY
                            ml.Title, ml.Id";
            return await connection.QueryAsync<MovieList>(sql, new { userId = userId });
        }
    }

    internal async Task<MovieList> GetAsync(string userid, int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @"SELECT 
                            ml.Title as Name, ml.Id, COUNT(mtml.Id) AS MovieCount, AVG(m.UserRating) AS AverageRating
                        FROM
                            MovieList ml
                        LEFT OUTER JOIN MovieToMovieList mtml on ml.Id = mtml.MovieListId 
                                        and ml.Id=@Id 
                                        and ml.UserId=@userId
                        LEFT OUTER JOIN Movie m on m.Id = mtml.MovieId
                        GROUP BY
                            ml.Title, ml.Id";
            return await connection.QueryFirstOrDefaultAsync<MovieList>(sql, new { Id = id });
        }
    }

    public async Task<int> CreateMovieListAsync(MovieList data)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @"INSERT INTO MovieList 
                            (Title, UserId) VALUES(@Name, @UserId);
                        SELECT SCOPE_IDENTITY()";
            return await connection.ExecuteScalarAsync<int>(sql, data);
        }
    }
}