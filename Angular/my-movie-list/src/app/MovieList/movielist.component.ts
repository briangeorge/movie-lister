import { Component, Input, OnInit } from '@angular/core';
import { MovieListService } from './movielist.service';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'movielist-list',
    templateUrl: './movielist.component.html'
})
export class MovieListComponent implements OnInit {
    constructor(private movieListService: MovieListService,
        private route: ActivatedRoute) {
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
        this.errorMessage = this.route.snapshot.queryParamMap.get('errorMessage');
    }
}