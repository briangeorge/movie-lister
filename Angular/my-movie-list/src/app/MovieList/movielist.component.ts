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
        this.movieListService.createMovieList(this.name).subscribe({
            next: movieList => this.movieLists.push(movieList)
        });
    }

    ngOnInit(): void {
        //TODO: add error handling
        this.movieListService.getMovieLists().subscribe({
            next: movieLists => this.movieLists = movieLists
        });
    }
}