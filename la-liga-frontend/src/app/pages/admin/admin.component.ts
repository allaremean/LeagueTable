import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TeamService } from '../../services/team.service';
import { MatchService } from '../../services/match.service';
import { AuthService } from '../../services/auth.service';
import { Team, Match, User } from '../../models/interfaces';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent implements OnInit {
  teamService = inject(TeamService);
  matchService = inject(MatchService);
  authService = inject(AuthService);

  teams: Team[] = [];
  matches: Match[] = [];
  users: User[] = [];

  // Team Form
  newTeam: Team = { teamID: '', name: '', city: '', stadium: '' };
  teamMsg = '';
  editingTeamId: string | null = null;

  // Match Form
  newMatch: Match = { matchID: '', homeTeamID: '', homeScore: 0, awayTeamID: '', awayScore: 0 };
  matchMsg = '';
  editingMatchId: string | null = null;

  userMsg = '';
  
  isSuperAdmin = false;

  ngOnInit() {
    this.authService.currentUser$.subscribe(u => {
      if (u) this.isSuperAdmin = u.role === 'SuperAdmin';
    });
    this.loadTeams();
    this.loadMatches();
    if (this.isSuperAdmin) {
      this.loadUsers();
    }
  }

  loadTeams() {
    this.teamService.getAll().subscribe({
      next: (data) => this.teams = data || [],
      error: () => this.teams = []
    });
  }

  loadMatches() {
    this.matchService.getAll().subscribe({
      next: (data) => this.matches = data || [],
      error: () => this.matches = []
    });
  }

  loadUsers() {
    this.authService.getAllUsers().subscribe({
      next: (data) => this.users = data || [],
      error: () => this.users = []
    });
  }

  editTeam(team: Team) {
    this.editingTeamId = team.teamID;
    this.newTeam = { ...team };
  }

  cancelEditTeam() {
    this.editingTeamId = null;
    this.newTeam = { teamID: '', name: '', city: '', stadium: '' };
  }

  saveTeam() {
    if (this.editingTeamId) {
      this.teamService.update(this.editingTeamId, this.newTeam).subscribe({
        next: () => {
          this.teamMsg = 'Team updated successfully!';
          this.loadTeams();
          this.cancelEditTeam();
          setTimeout(() => this.teamMsg = '', 3000);
        },
        error: () => this.teamMsg = 'Error updating team.'
      });
    } else {
      this.teamService.create(this.newTeam).subscribe({
        next: () => {
          this.teamMsg = 'Team created successfully!';
          this.loadTeams();
          this.cancelEditTeam();
          setTimeout(() => this.teamMsg = '', 3000);
        },
        error: () => this.teamMsg = 'Error creating team.'
      });
    }
  }

  editMatch(match: Match) {
    this.editingMatchId = match.matchID;
    this.newMatch = { ...match };
  }

  cancelEditMatch() {
    this.editingMatchId = null;
    this.newMatch = { matchID: '', homeTeamID: '', homeScore: 0, awayTeamID: '', awayScore: 0 };
  }

  saveMatch() {
    if (this.editingMatchId) {
      this.newMatch.matchID = this.editingMatchId; // Enforce PK stays same during edit
      this.matchService.update(this.editingMatchId, this.newMatch).subscribe({
        next: () => {
          this.matchMsg = 'Match updated successfully!';
          this.loadMatches();
          this.cancelEditMatch();
          setTimeout(() => this.matchMsg = '', 3000);
        },
        error: () => this.matchMsg = 'Error updating match.'
      });
    } else {
      this.newMatch.matchID = this.newMatch.homeTeamID + this.newMatch.awayTeamID;
      this.matchService.create(this.newMatch).subscribe({
        next: () => {
          this.matchMsg = 'Match recorded successfully!';
          this.loadMatches();
          this.cancelEditMatch();
          setTimeout(() => this.matchMsg = '', 3000);
        },
        error: () => this.matchMsg = 'Error creating match.'
      });
    }
  }

  deleteTeam(id: string) {
    if(confirm(`Are you sure you want to delete team ${id}?`)) {
       this.teamService.delete(id).subscribe({
         next: () => this.loadTeams(),
         error: () => alert('Cannot delete team. It may be part of an existing match.')
       });
    }
  }

  deleteMatch(id: string) {
    if(confirm(`Are you sure you want to delete match ${id}?`)) {
       this.matchService.delete(id).subscribe({
         next: () => this.loadMatches(),
         error: () => alert('Error deleting match.')
       });
    }
  }

  getTeamName(teamID: string): string {
    const team = this.teams.find(t => t.teamID === teamID);
    return team && team.name ? team.name : teamID;
  }

  promoteUser(username: string) {
    this.authService.promoteUser(username).subscribe({
      next: () => this.loadUsers(),
      error: () => this.userMsg = 'Error promoting user.'
    });
  }

  demoteUser(username: string) {
    this.authService.demoteUser(username).subscribe({
      next: () => this.loadUsers(),
      error: () => this.userMsg = 'Error demoting user.'
    });
  }

  deleteUser(username: string) {
    if (confirm(`Are you sure you want to delete user ${username}?`)) {
      this.authService.deleteUser(username).subscribe({
        next: () => this.loadUsers(),
        error: () => {
           this.userMsg = 'Error deleting user.';
           setTimeout(() => this.userMsg = '', 3000);
        }
      });
    }
  }
}
