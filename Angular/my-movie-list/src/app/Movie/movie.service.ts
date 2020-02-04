import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse } from "@angular/common/http"
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IMovie } from '../Models/IMovie';

@Injectable({
    providedIn: 'root'
})
export class MovieService {
    private searchUrl: string = "http://localhost:7071/api/SearchMovies";
    private getDetailUrl: string = "http://localhost:7071/api/GetMovie";
    private saveRatingUrl: string = "http://localhost:7071/api/RateMovie";
    constructor(private http: HttpClient) { }

    searchMovies(searchValue: string): Observable<IMovie[]> {
        return this.http.get<IMovie[]>(this.searchUrl + '/' + searchValue).pipe(catchError(this.handleError));
    }

    getMovie(id: number): Observable<IMovie> {
        return this.http.get<IMovie>(this.getDetailUrl + '/' + id).pipe(catchError(this.handleError));
    }

    saveRating(id: number, rating: number): Observable<object> {
        return this.http.post<object>(this.saveRatingUrl + '/' + id, { rating: rating }).pipe(catchError(this.handleError));
    }


    private handleError(err: HttpErrorResponse) {
        let errorMessage = '';

        if (err.error instanceof ErrorEvent) {
            errorMessage = `An error ocurred: ${err.error.message}`;
        } else {
            errorMessage = `${err.status} - ${err.error ? err.error : ''} ${err.message}`;
        }

        return throwError(errorMessage);
    }
}