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
  movie: IMovie;
  saveMessage: string;
  constructor(private route: ActivatedRoute, private router: Router, private movieService: MovieService) { }

  saveRating(): void {
    if (this.movie.rating) {
      this.movieService.saveRating(this.movie.id, this.movie.rating).subscribe({
        next: msg => this.saveMessage = msg['message'],
        error: err => this.saveMessage = err
      })
    }
  }
  ngOnInit(): void {
    let id = +this.route.snapshot.paramMap.get('id');
    this.movieService.getMovie(id).subscribe({
      next: movie => this.movie = movie,
      error: err => this.router.navigate(['/'], { queryParams: { errorMessage: err } })
    });
  }

}
