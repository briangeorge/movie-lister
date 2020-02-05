using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arcadia.Challenge.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Arcadia.Challenge.Repositories
{
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
                            ml.Title as Name, ml.Id, COUNT(mtml.Id) AS MovieCount, AVG(mr.Rating) AS AverageRating
                        FROM
                            MovieList ml
                        LEFT OUTER JOIN MovieToMovieList mtml on ml.Id = mtml.MovieListId 
                                        and ml.UserId=@UserId
                        LEFT OUTER JOIN Movie m on m.Id = mtml.MovieId
                        LEFT OUTER JOIN MovieRating mr on mtml.MovieId = mr.MovieId
                        GROUP BY
                            ml.Title, ml.Id";
                return await connection.QueryAsync<MovieList>(sql, new { UserId = userId });
            }
        }

        internal async Task<MovieList> GetAsync(string userId, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT 
                            ml.Title as Name, ml.Id, COUNT(mtml.Id) AS MovieCount, AVG(mr.Rating) AS AverageRating
                        FROM
                            MovieList ml
                        LEFT OUTER JOIN MovieToMovieList mtml on ml.Id = mtml.MovieListId 
                                        and ml.Id=@Id 
                                        and ml.UserId=@UserId
                        LEFT OUTER JOIN MovieRating mr on mr.MovieId = mtml.MovieId
                        GROUP BY
                            ml.Title, ml.Id";
                return await connection.QueryFirstOrDefaultAsync<MovieList>(sql, new { Id = id, UserId = userId });
            }
        }

        internal async Task AddMovieToListsAsync(int movieId, List<int> lists, string userid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT MovieListId
                            FROM MovieToMovieList
                            WHERE MovieId=@MovieId AND MovieListId IN @MovieListIds";
                var existing = (await connection.QueryAsync<int>(sql, new
                {
                    MovieId = movieId,
                    MovieListIds = lists
                })).ToList();
                var filteredLists = lists.Except(existing).ToList();
                sql = @"INSERT INTO MovieToMovieList 
                            (MovieListId, MovieId)
                            SELECT ml.Id, @MovieId
                            FROM MovieList ml
                            WHERE ml.UserId=@UserId AND ml.Id IN @MovieListIds";
                await connection.ExecuteScalarAsync<int>(sql, new
                {
                    MovieId = movieId,
                    UserId = userid,
                    MovieListIds = filteredLists
                });
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
}