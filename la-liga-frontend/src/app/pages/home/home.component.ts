import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StandingService } from '../../services/standing.service';
import { Standing } from '../../models/interfaces';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  standingService = inject(StandingService);
  standings$: Observable<Standing[]> | null = null;

  ngOnInit() {
    this.standings$ = this.standingService.getStandings();
  }
}
