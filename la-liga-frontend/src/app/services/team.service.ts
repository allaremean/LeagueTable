import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Team } from '../models/interfaces';

@Injectable({ providedIn: 'root' })
export class TeamService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/Teams`;

  getAll() { return this.http.get<Team[]>(this.url); }
  getById(id: string) { return this.http.get<Team>(`${this.url}/${id}`); }
  create(team: Team) { return this.http.post<Team>(this.url, team); }
  update(id: string, team: Team) { return this.http.put<Team>(`${this.url}/${id}`, team); }
  delete(id: string) { return this.http.delete(`${this.url}/${id}`); }
}
