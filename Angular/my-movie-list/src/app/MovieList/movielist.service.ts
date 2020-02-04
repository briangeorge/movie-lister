import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse } from "@angular/common/http"
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IMovieList } from '../Models/IMovieList';

@Injectable({
    providedIn: 'root'
})
export class MovieListService {
    private getUrl: string = "http://localhost:7071/api/GetMovieLists";
    private getSingleUrl: string = "http://localhost:7071/api/GetMovieList";
    private createUrl: string = "http://localhost:7071/api/CreateMovieList";
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