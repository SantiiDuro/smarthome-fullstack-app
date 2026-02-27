import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import environments from '../../environments';
import UserCredentialsModel from '../services/session/models/UserCredentialsModel';
import SessionCreatedModel from '../services/session/models/SessionCreatedModel';
import ApiRepository from './api-repository';

@Injectable({
  providedIn: 'root'
})
export class SessionApiRepositoryService extends ApiRepository {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, http);
  }

  public login(
    credentials: UserCredentialsModel
  ): Observable<SessionCreatedModel>{
      return this.post(credentials, "sesiones");
    }

  public logout(): Observable<SessionCreatedModel>{
    return this.delete("sesiones");
  }
}
