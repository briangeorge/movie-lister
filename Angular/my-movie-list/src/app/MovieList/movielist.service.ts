import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http"
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class MovieListService {
    private getUrl: string = "http://localhost:7071/api/GetMovieLists";
    private createUrl: string = "http://localhost:7071/api/CreateMovieList";
    constructor(private http: HttpClient) { }
    getMovieLists(): Observable<IMovieList[]> {
        //todo: add exception handling
        return this.http.get<IMovieList[]>(this.getUrl);
    }

    createMovieList(name: string): Observable<IMovieList> {
        //TODO Call Create movie list api
        return this.http.post<IMovieList>(this.createUrl, {
            Name: name
        });
    }
}