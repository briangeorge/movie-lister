import { IMovie } from './IMovie';

export interface IMovieList {
    id: number;
    name: string;
    averageRating: number;
    movieCount: number;
    movies: IMovie[]
}