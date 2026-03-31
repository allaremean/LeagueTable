import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Standing } from '../models/interfaces';

@Injectable({ providedIn: 'root' })
export class StandingService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/Standings`;

  getStandings() { return this.http.get<Standing[]>(this.url); }
  getTeamStanding(teamId: string) { return this.http.get<Standing>(`${this.url}/${teamId}`); }
}
