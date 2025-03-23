# 👥 User Management API

A robust User Management API built with **.NET 8**, following **Clean Architecture** principles. It integrates **Azure AD B2C** for authentication and **Microsoft Graph API** for user registration and management.

## 🔧 Tech Stack

- **.NET 8 Web API**
- **Azure AD B2C** (OpenID Connect)
- **Microsoft Graph API**
- **Entity Framework Core + SQL Server**
- **Clean Architecture**
---

## ✨ Features

- 🔐 **Azure AD B2C Authentication**
    - Supports ROPC flow or standard user flows via OpenID Connect.
    - JWT validation and token-based authentication.

- 👤 **Admin-Based User Registration**
    - Admins can create Azure AD B2C users via Microsoft Graph API.
    - Secure handling of secrets and access tokens.

- 🔒 **Role-Based Access Control (RBAC)**
    - `[Authorize(Roles = "...")]` for securing endpoints.
    - Claims-based identity management.

- 🧱 **Clean Architecture Implementation**
    - Domain-driven design with clear separation of concerns.
    - Testable, scalable, and maintainable.

---