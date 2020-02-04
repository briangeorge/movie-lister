import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router'

import { AppComponent } from './app.component';
import { MovieListComponent } from './MovieList/movielist.component';
import { HttpClientModule } from '@angular/common/http';
import { MovielistDetailComponent } from './MovieList/movielist-detail.component';
import { HomeComponent } from './home.component';
import { MovieDetailComponent } from './Movie/movie-detail.component';
import { MovieSearchComponent } from './Movie/movie-search.component';
import { AddMovieToListComponent } from './Shared/add-movie-to-list.component';


@NgModule({
  declarations: [
    AppComponent,
    MovieListComponent,
    MovielistDetailComponent,
    HomeComponent,
    MovieDetailComponent,
    MovieSearchComponent,
    AddMovieToListComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'movielists', component: MovieListComponent },
      { path: 'movielist/:id', component: MovielistDetailComponent },
      { path: 'movie/search', component: MovieSearchComponent },
      { path: 'movie/:id', component: MovieDetailComponent },
      { path: '**', component: HomeComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
