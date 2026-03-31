import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Match } from '../models/interfaces';

@Injectable({ providedIn: 'root' })
export class MatchService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/Matches`;

  getAll() { return this.http.get<Match[]>(this.url); }
  getById(id: string) { return this.http.get<Match>(`${this.url}/${id}`); }
  create(match: Match) { return this.http.post<Match>(this.url, match); }
  update(id: string, match: Match) { return this.http.put<Match>(`${this.url}/${id}`, match); }
  delete(id: string) { return this.http.delete(`${this.url}/${id}`); }
}
