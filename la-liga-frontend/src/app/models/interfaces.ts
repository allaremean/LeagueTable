export interface Team {
  teamID: string;
  name: string | null;
  city: string | null;
  stadium: string | null;
}

export interface Match {
  matchID: string;
  homeTeamID: string;
  awayTeamID: string;
  homeScore: number;
  awayScore: number;
  homeTeam?: Team;
  awayTeam?: Team;
}

export interface Standing {
  position: number;
  team: string;
  played: number;
  wins: number;
  draws: number;
  losses: number;
  goalsFor: number;
  goalsAgainst: number;
  goalDifference: number;
  points: number;
}

export interface User {
  id: number;
  username: string;
  role: string;
  token?: string; // We'll append this from the auth service login
}

export interface UserDto {
  username?: string;
  password?: string;
}
