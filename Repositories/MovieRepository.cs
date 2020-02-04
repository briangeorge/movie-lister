using System;
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

        internal async Task<Movie> GetMovieForUserAsync(int? id, string userid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT COUNT(1) FROM MovieRating mr
                            where mr.MovieId=@Id and mr.UserId=@UserId";
                var exists = await connection.ExecuteScalarAsync<int>(sql, new { Id = id, UserId = userid });
                if (exists == 0)
                {
                    sql = @"INSERT INTO MovieRating
                                (UserId,MovieId)
                            VALUES
                                (@UserId, @Id)";
                    await connection.ExecuteAsync(sql, new { Id = id, UserId = userid });
                }
                sql = @"SELECT
                            m.Id,
                            ImdbId,
                            Title,
                            Rated,
                            Year,
                            Genres,
                            Director,
                            Runtime,
                            mr.Rating as Rating,
                            DataPopulated
                        FROM Movie m
                        JOIN MovieRating mr
                                        on mr.MovieId = m.Id
                                        and m.Id=@Id
                                        and mr.UserId=@UserId";
                return await connection.QueryFirstOrDefaultAsync<Movie>(sql, new { Id = id, UserId = userid });
            }
        }

        internal async Task SaveRatingAsync(string userid, int? id, int rating)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT COUNT(1) FROM MovieRating mr
                            where mr.MovieId=@Id and mr.UserId=@UserId";
                var exists = await connection.ExecuteScalarAsync<int>(sql, new { Id = id, UserId = userid });
                if (exists == 0)
                {
                    sql = @"INSERT INTO MovieRating
                                (UserId,MovieId, Rating)
                            VALUES
                                (@UserId, @Id, @Rating)";
                    await connection.ExecuteAsync(sql, new { Id = id, UserId = userid, Rating = rating });
                }
                else
                {
                    sql = @"UPDATE MovieRating
                                SET Rating=@Rating
                            WHERE
                                UserId=@UserId
                                AND MovieId=@Id";
                    await connection.ExecuteAsync(sql, new { Id = id, UserId = userid, Rating = rating });
                }
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

        internal async Task UpdateDataAsync(Movie movie)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"UPDATE Movie
                        SET 
                            ImdbId=@ImdbId,
                            Title=@Title,
                            Genres=@Genres,
                            Director=@Director,
                            Runtime=@Runtime,
                            Rated=@Rated,
                            Year=@Year,
                            DataPopulated=1
                        WHERE Id=@Id";
                await connection.ExecuteAsync(sql, movie);
            }
        }
    }
}