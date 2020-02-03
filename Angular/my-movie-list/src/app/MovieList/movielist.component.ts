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
        this.movieLists.push(this.movieListService.createMovieList());
    }

    ngOnInit(): void {
        this.movieLists = this.movieListService.getMovieLists();
    }
}