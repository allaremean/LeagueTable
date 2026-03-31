import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { TeamsComponent } from './pages/teams/teams.component';
import { MatchesComponent } from './pages/matches/matches.component';
import { AdminComponent } from './pages/admin/admin.component';
import { authGuard } from './core/auth.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'teams', component: TeamsComponent },
  { path: 'matches', component: MatchesComponent },
  { path: 'admin', component: AdminComponent, canActivate: [authGuard], data: { roles: ['Admin', 'SuperAdmin'] } },
  { path: '**', redirectTo: '' }
];
