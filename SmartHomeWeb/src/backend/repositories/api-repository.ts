import {
    HttpClient,
    HttpErrorResponse,
    HttpHeaders,
  } from '@angular/common/http';
  import { Observable, catchError, retry, throwError } from 'rxjs';
  
  export default abstract class ApiRepository {
    protected get fullEndpoint(): string {
        return this._apiOrigin;
    }
  
    protected get headers() {
      return {
        headers: new HttpHeaders({
          accept: 'application/json',
          Authorization: localStorage.getItem('token') ?? '',
        }),
      };
    }
  
    constructor(
      protected readonly _apiOrigin: string,
      protected readonly _http: HttpClient
    ) {}
  
    protected get<T>(extraResource = '', query = ''): Observable<T> {
      query = query ? `?${query}` : '';
      extraResource = extraResource ? `/${extraResource}` : '';
  
      return this._http
        .get<T>(`${this.fullEndpoint}${extraResource}${query}`, this.headers)
        .pipe(retry(3), catchError(this.handleError));
    }
  
    protected post<T>(body: any, extraResource = ''): Observable<T> {
      extraResource = extraResource ? `/${extraResource}` : '';
  
      return this._http
        .post<T>(`${this.fullEndpoint}${extraResource}`, body, this.headers)
        .pipe(retry(3), catchError(this.handleError));
    }
  
    protected patch<T>(body: any, extraResource = ''): Observable<T> {
      extraResource = extraResource ? `/${extraResource}` : '';
    
      return this._http
        .patch<T>(`${this.fullEndpoint}${extraResource}`, body, this.headers)
        .pipe(retry(3), catchError(this.handleError));
    }
    

    protected putById<T>(
      id: string,
      body: any = null,
      extraResource = ''
    ): Observable<T> {
      extraResource = extraResource ? `/${extraResource}` : '';
  
      return this._http
        .put<T>(`${this.fullEndpoint}/${id}${extraResource}`, body, this.headers)
        .pipe(retry(3), catchError(this.handleError));
    }
  
    protected delete<T>(extraResource = '', query = ''): Observable<T> {
      query = query ? `?${query}` : '';
  
      extraResource = extraResource ? `/${extraResource}` : '';
  
      return this._http
        .delete<T>(`${this.fullEndpoint}${extraResource}${query}`, this.headers)
        .pipe(retry(3), catchError(this.handleError));
    }
  
    protected handleError(error: HttpErrorResponse) {
      let errorMessage = 'Algo salió mal; inténtalo de nuevo más tarde.';
      
      if (error.error instanceof ErrorEvent) {
        // Error del lado del cliente o de red
        console.error('Ocurrió un error:', error.error.message);
        errorMessage = error.error?.message;
      } else {
        // Error del lado del servidor
        console.error(`Backend retornó código ${error.status}, cuerpo del error: ${error.error}`);
    
        // Si el backend proporciona un mensaje, úsalo; si no, usa un mensaje genérico.
        errorMessage = error.error?.mensaje || errorMessage;
      }
    
      // Devolver un observable con el mensaje de error personalizado
      return throwError(errorMessage);
    }
  }
  