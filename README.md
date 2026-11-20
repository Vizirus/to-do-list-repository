# To-Do List Application

A multi-tier task management application built with an **ASP.NET Core Web API** backend and an **ASP.NET MVC** frontend, following **Layered Architecture** principles. Developed as the final project for the **EPAM Systems .NET Fundamentals** course.

## Architecture

- **`DataLayer`** — Entity Framework Core data access, models, and database configuration (SQLite with optional SQL Server support)
- **`BusinessLayer`** — Business logic, services, and validation
- **`WebApi`** — Standalone RESTful Web API (can also be used independently via the `step-3/api` branch)
- **`WebMvc`** — ASP.NET Core MVC + Razor frontend consuming the Web API
- **`BusinessTest` & `DataTest`** — Automated unit tests for business logic and data access
- **Code Quality & Docs** — Enforced via `stylecop.json` and `.editorconfig`, with design docs in [`software-architecture.md`](./software-architecture.md) and [`functional-requirements.md`](./functional-requirements.md)

## Features

- Create and manage multiple to-do lists
- Add, edit, and delete tasks
- Assign tasks to specific users
- Organize and filter tasks using tags

## Tech Stack

- **Language & Framework:** C# / .NET, ASP.NET Core Web API, ASP.NET MVC, Razor
- **Data Access:** Entity Framework Core, SQLite / MS SQL Server
- **Quality & Testing:** xUnit/NUnit tests (`BusinessTest`, `DataTest`), StyleCop

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/Vizirus/to-do-list-repository.git
   cd to-do-list-repository
   ```
2. Run the **Web API** backend:
   ```bash
   cd WebApi
   dotnet run
   ```
3. In a separate terminal, run the **MVC** frontend:
   ```bash
   cd WebMvc
   dotnet run
   ```
4. Run tests:
   ```bash
   dotnet test
   ```

## Usage Showcase

After logging in, you can view existing lists, edit them, or create a new one:

<img width="956" height="370" alt="Create a new to-do list" src="https://github.com/user-attachments/assets/c949f535-050d-42a2-85ef-c074cceb9d41" />

Adding a new task to a list:

<img width="961" height="581" alt="Add a new task" src="https://github.com/user-attachments/assets/b38ed188-090f-42c6-aa02-3aa7c604feec" />

Viewing and managing all tasks in a list:

<img width="958" height="440" alt="View all tasks" src="https://github.com/user-attachments/assets/ffb5fc96-0b68-48e0-b053-bd5e8f1eeb94" />
