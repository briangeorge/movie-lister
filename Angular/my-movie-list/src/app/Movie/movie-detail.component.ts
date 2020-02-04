import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MovieService } from './movie.service';
import { IMovie } from '../Models/IMovie';

@Component({
  selector: 'app-movie-detail',
  templateUrl: './movie-detail.component.html',
  styleUrls: ['./movie-detail.component.css']
})
export class MovieDetailComponent implements OnInit {
  private movie: IMovie;
  constructor(private route: ActivatedRoute, private router: Router, private movieService: MovieService) { }

  ngOnInit() {
    let id = +this.route.snapshot.paramMap.get('id');
    this.movieService.getMovie(id).subscribe({
      next: movie => this.movie = movie,
      error: err => this.router.navigate(['/'], { queryParams: { errorMessage: err } })
    });
  }

}
