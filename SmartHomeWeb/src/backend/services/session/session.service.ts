import { Injectable } from '@angular/core';
import { SessionApiRepositoryService } from '../../repositories/session-api-repository.service';
import UserCredentialsModel from './models/UserCredentialsModel';
import SessionCreatedModel from './models/SessionCreatedModel';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import UserLoggedModel from './models/UserLoggedModel';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private readonly _userLogged$: BehaviorSubject<UserLoggedModel | null> =
  new BehaviorSubject<UserLoggedModel | null>(null);

  get userLogged(): Observable<UserLoggedModel | null> {
    if (!this._userLogged$.value) {
      const token = localStorage.getItem("token");

      if (token) {
        const permissions = JSON.parse(
          localStorage.getItem("permissions") || "[]"
        );
        this._userLogged$.next({ token, permissions });
      }
    }

    return this._userLogged$.asObservable();
  }

  constructor(private readonly _repository: SessionApiRepositoryService) { }

  public login(
    credentials: UserCredentialsModel
  ): Observable<SessionCreatedModel> {
    return this._repository.login(credentials).pipe(
      tap((response) => {
        localStorage.setItem("token", response.token);
        localStorage.setItem(
          "permissions",
          JSON.stringify(response.permisos)
        );
        this._userLogged$.next({
          token: response.token,
          permissions: response.permisos
        });
      })
    );
  }

  public logout(): Observable<SessionCreatedModel> {
    return this._repository.logout().pipe(
      tap(() => {
        localStorage.removeItem("token");
        localStorage.removeItem("permissions");
        this._userLogged$.next(null);
      })
    );
  }
}
