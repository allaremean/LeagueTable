import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TeamService } from '../../services/team.service';
import { Team } from '../../models/interfaces';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './teams.component.html',
  styleUrl: './teams.component.scss'
})
export class TeamsComponent implements OnInit {
  teamService = inject(TeamService);
  teams$: Observable<Team[]> | null = null;
  
  ngOnInit() {
    this.teams$ = this.teamService.getAll();
  }
}
