import { Component, Input } from '@angular/core';

@Component({
    selector: 'movielist-list',
    templateUrl: './movielist.component.html'
})
export class MovieListComponent {
    movieLists: IMovieList[] = [];
    @Input() name: string;
    create(): void {
        //TODO Call Create movie list api
        this.movieLists.push({
            Id: 1,
            Name: 'test',
            AverageRating: 4.3,
            MovieCount: 7
        })
    }
}