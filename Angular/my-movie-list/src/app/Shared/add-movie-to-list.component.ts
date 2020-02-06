import { Component, OnInit, Input } from '@angular/core';
import { MovieListService } from '../MovieList/movielist.service';
import { IMovieList } from '../Models/IMovieList';

@Component({
  selector: 'app-add-movie-to-list',
  templateUrl: './add-movie-to-list.component.html',
  styleUrls: ['./add-movie-to-list.component.css']
})
export class AddMovieToListComponent implements OnInit {
  movieLists: IMovieList[];
  selectedLists: number[] = [];
  resultMessage: string;
  @Input() movieId: number;
  constructor(private movieListService: MovieListService) { }

  ngOnInit(): void {
    //TODO: centralize this so that it doesn't get called everywhere?
    this.movieListService.getMovieLists().subscribe({
      next: movieLists => this.movieLists = movieLists
    });
  }

  updateSelected(event: any, id: number): void {
    const index = this.selectedLists.indexOf(id);
    if (event.currentTarget.checked) {
      if (index < 0) {
        this.selectedLists.push(id);
      }
    } else {
      if (index >= 0) {
        this.selectedLists.splice(index, 1);
      }
    }
  }

  saveClick(): void {
    this.movieListService.addMovieToLists(this.movieId, this.selectedLists).subscribe({
      next: () => {
        this.resultMessage = "Saved!";
      },
      error: err => {
        this.resultMessage = err;
      }
    });
  }

}
