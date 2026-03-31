import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatchService } from '../../services/match.service';
import { TeamService } from '../../services/team.service';
import { Match, Team } from '../../models/interfaces';

@Component({
  selector: 'app-matches',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './matches.component.html',
  styleUrl: './matches.component.scss'
})
export class MatchesComponent implements OnInit {
  matchService = inject(MatchService);
  teamService = inject(TeamService);
  
  matches: Match[] = [];
  teams: Team[] = [];

  ngOnInit() {
    this.teamService.getAll().subscribe(t => {
      this.teams = t || [];
      this.matchService.getAll().subscribe(m => this.matches = m || []);
    });
  }

  getTeamName(teamID: string): string {
    const team = this.teams.find(t => t.teamID === teamID);
    return team && team.name ? team.name : teamID;
  }
}
