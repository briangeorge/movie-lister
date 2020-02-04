import { Component, OnInit } from '@angular/core';
import { IMovie } from '../Models/IMovie';
import { MovieService } from './movie.service';

@Component({
  selector: 'app-search',
  templateUrl: './movie-search.component.html',
  styleUrls: ['./movie-search.component.css']
})
export class MovieSearchComponent implements OnInit {
  results: IMovie[];
  searchValue: string;
  errorMessage: string;
  constructor(private movieService: MovieService) { }

  search() {
    if (this.searchValue && this.searchValue.length > 0) {
      this.movieService.searchMovies(this.searchValue).subscribe({
        next: movies => this.results = movies,
        error: err => this.errorMessage = err
      });
    }
  }
  ngOnInit() {
  }

}
