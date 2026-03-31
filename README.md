# LaLiga Table

A full-stack application simulating a football league system. This project features an **ASP.NET Core Web API** back-end powered by Entity Framework,  integrated with a  **Angular** front-end dashboard.

## 🚀 Features

### Frontend (Angular 17+)
- **Dynamic Standings Table**: Real-time league table calculating wins, losses, goals, and points dynamically.
- **Matches Directory**: Publicly view all historical games featuring full team names, acronyms, and scorelines.
- **Admin Command Dashboard**: Secure workspace exclusively for `Admin` and `SuperAdmin` users to visually manage teams, update matches, and administer user identities.
- **JWT Interceptor**: Automated state management securely attaching JWT authorization headers to protected network requests.

### Backend (ASP.NET Core 8)
- **Code-First Entity Framework Architecture**: Relational SQL Server database mapping utilizing foreign keys and navigational entities.
- **Role-Based Authentication**: Custom JWT-based Identity matrix handling logic for registering, promoting, and demoting users globally.
- **Scalar API Interface**: Modern, interactive REST API testing environment mounted at `/scalar/v1`.
- **Dynamic Math Generators**: The league points and goal-difference analytics are completely calculated on-the-fly via the Match logic matrix.

## ⚽ Unique Design Decisions
- **Acronyms as Primary Keys**: Instead of using non-descriptive numerics (like `Id=1`), Teams utilize their natural acronym as the SQL Primary Key (e.g. `RMA`). Match Primary keys are structurally concatenated hashes (e.g. `RMABAR` represents Real Madrid vs Barcelona) providing immediate referential context!
- **Data Hydration via DbContext**: Pre-seeded database environments for immediate testing upon spinning up Entity Framework `dotnet run`.

## ⚙️ How to Run

### 1. The Backend (ASP.NET)
1. Open a terminal and navigate into the `LaLiga` folder.
2. Ensure your `appsettings.json` connection string maps to your SQL Server environment.
3. Run `dotnet ef database update` to orchestrate your database migrations.
4. Launch the application via `dotnet run` (runs natively on `http://localhost:5077`).
5. Open `http://localhost:5077/scalar/v1` to inspect the backend endpoints directly.

### 2. The Frontend (Angular)
1. Open a new terminal and navigate into the `la-liga-frontend` directory.
2. Install node modules via `npm install`.
3. Start the application and watcher by running `npx ng serve`.
4. Open your browser to `http://localhost:4200` to dive into the UI!

## 🔐 Accounts & Permissions
Users must register an account and be promoted by a SuperAdmin via the API to gain  CRUD operational access to the `/admin` UI dashboard. SuperAdmins possess global deletion and role-manipulation rights!. There is only one SuperAdmin.

---
*Developed to showcase .NET knowledge depth and Angular.*
