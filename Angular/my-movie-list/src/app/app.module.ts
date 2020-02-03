import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { MovieListComponent } from './MovieList/movielist.component';
import { HttpClientModule } from '@angular/common/http';
import { MovielistDetailComponent } from './MovieList/movielist-detail.component';


@NgModule({
  declarations: [
    AppComponent,
    MovieListComponent,
    MovielistDetailComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
