import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse } from "@angular/common/http"
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IMovieList } from '../Models/IMovieList';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class MovieListService {
    private getUrl: string = environment.baseUrl + "/api/GetMovieLists";
    private getSingleUrl: string = environment.baseUrl + "/api/GetMovieList";
    private createUrl: string = environment.baseUrl + "/api/CreateMovieList";
    private addMovieToListsUrl: string = environment.baseUrl + "/api/AddMovieToLists";
    constructor(private http: HttpClient) { }

    getMovieLists(): Observable<IMovieList[]> {
        return this.http.get<IMovieList[]>(this.getUrl).pipe(catchError(this.handleError));
    }
    getMovieList(id: number): Observable<IMovieList> {
        return this.http.get<IMovieList>(this.getSingleUrl + '/' + id).pipe(catchError(this.handleError));
    }

    createMovieList(name: string): Observable<IMovieList> {
        return this.http.post<IMovieList>(this.createUrl, {
            Name: name
        }).pipe(catchError(this.handleError));
    }

    addMovieToLists(movieId: number, selectedLists: number[]): Observable<object> {
        return this.http.post<object>(this.addMovieToListsUrl, {
            movieId: movieId,
            lists: selectedLists
        }).pipe(catchError(this.handleError));
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