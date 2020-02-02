import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class MovieListService {
    getMovieLists(): IMovieList[] {
        return [{
            Id: 1,
            Name: 'List 1',
            MovieCount: 6,
            AverageRating: 4.3
        }, {
            Id: 2,
            Name: 'List 2',
            MovieCount: 3,
            AverageRating: 2
        }, {
            Id: 3,
            Name: 'List 3',
            MovieCount: 9,
            AverageRating: 3.7
        }];
    }
}