import { Component, Input, OnInit } from '@angular/core';
import { MovieListService } from './movielist.service';

@Component({
    selector: 'movielist-list',
    templateUrl: './movielist.component.html'
})
export class MovieListComponent implements OnInit {
    constructor(private movieListService: MovieListService) {
    }
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

    ngOnInit(): void {
        this.movieLists = this.movieListService.getMovieLists();
    }
}