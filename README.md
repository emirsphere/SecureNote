🔒 SecureNote API (WIP)

Status: 🚧 Work in progress - Currently focusing on writing Unit Tests for the business logic.

📌 About The Project

This is a RESTful API built for a secure note-taking application. Since my main goal is to become a backend engineer who actually understands system design rather than just writing code that "works", I am building this using Clean Architecture principles.

There is no frontend for this project right now, and that is intentional. My entire focus is on the backend architecture, database relations, and security.

⚙️ Tech Stack

Language: C#

Framework: .NET (Web API)

Database: MS SQL Server / Entity Framework Core

Testing: xUnit & Moq (Currently implementing)

🏗️ What I'm Focusing On

Instead of rushing to finish the app, I am taking my time to learn enterprise-level patterns. My main technical goals are:

Clean Architecture: Separating the Domain, Application, Infrastructure, and API layers so the project is modular and easy to maintain.

Dependency Injection: Keeping services loosely coupled.

Unit Testing (Current Focus): Writing tests for my services using xUnit and Moq. I want to make sure my core logic doesn't break when I add new features later.

Security: Planning to implement JWT authentication and data encryption so notes aren't saved as plain text in the database.

🚀 To-Do / Progress

[x] Project structure and layer setup (Clean Architecture).

[x] Database connection and basic CRUD endpoints.

[ ] Writing unit tests for the core services (I am working on this right now).

[x ] Implementing JWT Authentication.

[x] Encrypting notes before saving them to MS SQL Server.

💡 Developer Note

I started this project to get hands-on experience with how real-world APIs are built. Anyone can write a basic CRUD app, but I want to know exactly what happens behind the scenes. Writing unit tests is slowing me down a bit, but it's teaching me how to write better, more reliable code.
