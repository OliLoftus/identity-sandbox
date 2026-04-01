# Identity Sandbox Reference

This repository is a learning and reference project for modern authentication and authorization patterns using .NET and React. The goals domain is intentionally small so the repo stays useful as a clear reference for OpenID Connect (OIDC), JWT-based API protection, scope-based authorization, validation, and SPA-to-API integration.

## Technology Stack

*   **Backend:** C#, ASP.NET Core 8
*   **Authentication:** Duende IdentityServer
*   **API:** ASP.NET Core Web API
*   **Database:** MongoDB
*   **Frontend:** React, TypeScript, Vite
*   **OIDC Client:** `oidc-client-ts`

## Project Structure

The solution is composed of three distinct projects:

*   `Identity.Oli.Auth`: An ASP.NET Core project hosting Duende IdentityServer. It is responsible for authenticating users and issuing security tokens.
*   `Identity.Oli`: An ASP.NET Core Web API that serves as the resource server. It protects goal and progress endpoints and relies on IdentityServer for authentication and authorization.
*   `Identity.Oli.Spa`: A single-page application (SPA) built with React and TypeScript. This is the reference client for logging in, obtaining tokens, and calling the protected API.

## How to Run Locally

Follow these steps to get the full application running on your machine.

### 1. Prerequisites

*   .NET 8 SDK
*   Node.js (v18 or later)
*   MongoDB running locally on `mongodb://localhost:27017`
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

You can log in with the following credentials (seeded in `Identity.Oli.Auth/Program.cs`):

*   **Username:** `oli`, `alice`, `bob`
*   **Password:** `Passw0rd!` (for all users)

## Key Concepts Implemented

This project serves as a practical example of the following concepts:

*   **Centralized Login:** Using Duende IdentityServer as a standalone identity provider.
*   **OIDC Authorization Code Flow with PKCE:** The current best-practice, most secure authentication flow for SPAs.
*   **JWT-based API Security:** Protecting API endpoints using JWT Bearer authentication.
*   **Scope-based Authorization:** Enforcing read, write, and delete permissions with named policies.
*   **Validation and Query Handling:** Using FluentValidation plus filtering, sorting, and paging on protected endpoints.
*   **CORS:** Correctly configuring Cross-Origin Resource Sharing to allow the SPA to communicate with the API.
*   **Event-Driven Authentication State:** Building a reactive `AuthProvider` in React that responds to login/logout events for a smooth user experience.

## Auth Flow (SPA + IdentityServer)

1. The user clicks **Log in** in the SPA. This triggers an OIDC redirect.
2. The user authenticates in `Identity.Oli.Auth` (ASP.NET Identity + Duende IdentityServer).
3. IdentityServer redirects back to the SPA `/callback`.
4. The SPA exchanges the authorization code for tokens and stores them.
5. The SPA calls the API with `Authorization: Bearer <access_token>`.
6. The API validates the token (issuer/audience) and enforces scope-based policies.

### Inspecting the Access Token (for learning)

1. Log in, then open the browser devtools console and run:
    ```js
    const user = await window.userManager?.getUser?.();
    console.log(user?.access_token);
    ```
2. Paste the token into a JWT decoder (e.g. `https://jwt.io` or `https://jwt.ms`) to inspect claims.
3. Look for the `scope` claim (e.g. `api1.read api1.write api1.delete`).
