import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MovieListService } from './movielist.service';
import { IMovieList } from '../Models/IMovieList';

@Component({
  selector: 'app-movielist-detail',
  templateUrl: './movielist-detail.component.html',
  styleUrls: ['./movielist-detail.component.css']
})
export class MovielistDetailComponent implements OnInit {
  movieList: IMovieList;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private movieListService: MovieListService) { }

  ngOnInit() {
    let id = +this.route.snapshot.paramMap.get('id');
    this.movieListService.getMovieList(id).subscribe({
      next: movieList => this.movieList = movieList,
      error: err => this.router.navigate(['/movielists'], { queryParams: { errorMessage: err } })
    })
  }

}
