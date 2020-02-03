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
    errorMessage: string;
    @Input() name: string;
    create(): void {
        //if (this.name && this.name.length > 0) {
        this.movieListService.createMovieList(this.name).subscribe({
            next: movieList => this.movieLists.push(movieList),
            error: err => this.errorMessage = err
        });
        //}
    }

    ngOnInit(): void {
        this.movieListService.getMovieLists().subscribe({
            next: movieLists => this.movieLists = movieLists,
            error: err => this.errorMessage = err
        });
    }
}