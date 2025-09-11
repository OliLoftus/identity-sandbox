# Identity Sandbox

This is a learning project designed to explore modern authentication and authorization patterns using .NET and React. The goal is to build a secure application from the ground up, implementing key concepts like OpenID Connect (OIDC), JWTs, and role-based access control.

## Technology Stack

*   **Backend:** C#, ASP.NET Core 8
*   **Authentication:** Duende IdentityServer
*   **API:** ASP.NET Core Web API
*   **Database:** In-Memory (for development)
*   **Frontend:** React, TypeScript, Vite
*   **OIDC Client:** `oidc-client-ts`

## Project Structure

The solution is composed of three distinct projects:

*   `Identity.Oli.Auth`: An ASP.NET Core project hosting Duende IdentityServer. It is responsible for authenticating users and issuing security tokens.
*   `Identity.Oli`: An ASP.NET Core Web API that serves as the resource server. It protects its endpoints and relies on the IdentityServer for authentication.
*   `Identity.Oli.Spa`: A single-page application (SPA) built with React and TypeScript. This is the user-facing client that communicates with the IdentityServer to log users in and with the API to fetch data.

## How to Run Locally

Follow these steps to get the full application running on your machine.

### 1. Prerequisites

*   .NET 8 SDK
*   Node.js (v18 or later)
*   An IDE like JetBrains Rider or Visual Studio.

### 2. Run the Backend Services

1.  Open the `Identity.Oli.sln` file in Rider.
2.  Create a **Compound** run configuration named `Backend Services` that starts both the `Identity.Oli.Auth` and `Identity.Oli` projects.
3.  Run the `Backend Services` configuration. This will start:
    *   The IdentityServer on `https://localhost:7040`
    *   The API on `https://localhost:7108`

### 3. Run the Frontend SPA

1.  Open a terminal and navigate to the SPA's directory:
    ```bash
    cd Identity.Oli.Spa
    ```
2.  Install the dependencies:
    ```bash
    npm install
    ```
3.  Start the development server:
    ```bash
    npm run dev
    ```
4.  Open your browser and navigate to `http://localhost:5173`.

### 4. Test Users

You can log in with the following credentials (defined in `Identity.Oli.Auth/Utils/TestUsers.cs`):

*   **Username:** `oli`, `alice`, `bob`
*   **Password:** `password` (for all users)

## Key Concepts Implemented

This project serves as a practical example of the following concepts:

*   **Centralized Login:** Using Duende IdentityServer as a standalone identity provider.
*   **OIDC Authorization Code Flow with PKCE:** The current best-practice, most secure authentication flow for SPAs.
*   **JWT-based API Security:** Protecting API endpoints using JWT Bearer authentication.
*   **CORS:** Correctly configuring Cross-Origin Resource Sharing to allow the SPA to communicate with the API.
*   **Event-Driven Authentication State:** Building a reactive `AuthProvider` in React that responds to login/logout events for a smooth user experience.
