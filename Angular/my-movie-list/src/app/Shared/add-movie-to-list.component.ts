import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { MovieListService } from '../MovieList/movielist.service';
import { IMovieList } from '../Models/IMovieList';

@Component({
  selector: 'app-add-movie-to-list',
  templateUrl: './add-movie-to-list.component.html',
  styleUrls: ['./add-movie-to-list.component.css']
})
export class AddMovieToListComponent implements OnInit {
  movieLists: IMovieList[];
  selectedLists: number[];
  movieId: number;
  @Output() messageEvent = new EventEmitter<string>();
  constructor(private movieListService: MovieListService) { }

  ngOnInit(): void {
    //TODO: centralize this so that it doesn't get called everywhere?
    this.movieListService.getMovieLists().subscribe({
      next: movieLists => this.movieLists = movieLists
    });
  }

  saveClick(): void {
    this.movieListService.addMovieToLists(this.movieId, this.selectedLists).subscribe({
      next: ()=> this.messageEvent.emit("Saved!"),
      error: ()=>this.messageEvent.emit("Failed!")
    })
  }

}
