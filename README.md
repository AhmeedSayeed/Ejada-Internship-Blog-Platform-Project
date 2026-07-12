# Blog Management Platform
 
A role-based blogging platform built with ASP.NET Core where Authors write and publish posts, Readers browse, comment, and rate content, and Admins moderate the whole platform.
 
## Overview
 
The platform supports three roles — **Author**, **Admin**, and **Reader** — each with a distinct set of permissions. Authors write and manage their own posts through a draft → review → publish workflow, Readers engage through comments, likes, and star ratings, and Admins moderate content, manage users, and monitor platform activity through a dedicated dashboard.
 
## Key Features
 
**Authentication & Users**
- Registration, login, and role-based access via ASP.NET Core Identity
- Editable user profiles (bio, profile image, name, email, password)
- Public author profiles with reader follow functionality
**Post Management**
- Rich text editor with formatting and image embedding
- Categorization and tagging
- Draft saving with auto-save
- Submit → Pending → Approved/Rejected publish workflow
**Discovery**
- Paginated, filterable post listing (category, tag, date, title)
- Sorting by recency or rating
**Engagement**
- Threaded comments with admin moderation (approve/reject/flag)
- Post likes
- 5-star post ratings with public average and vote count
**Admin Tools**
- Dashboard with platform stats and engagement analytics
- User management (suspend, reset password)
- Category/tag management
- Post edit history
**Author Analytics**
- Per-post views, likes, and comment counts
- Views-over-time charts
**Notifications**
- Real-time in-app notifications via SignalR
- Email notifications for approvals, rejections, likes, and comments
## Tech Stack
 
| Layer | Technology |
|---|---|
| Backend | ASP.NET Core Web API (C#) |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Auth | ASP.NET Core Identity |
| Real-time | SignalR |
 
## Architecture
 
The solution follows a layered (Clean Architecture-style) structure to keep the domain model independent of EF Core and other infrastructure concerns:
 
```
BlogPlatform.Domain/          # Entities, enums — no external dependencies
BlogPlatform.Application/     # Interfaces, DTOs, business logic
BlogPlatform.Infrastructure/  # EF Core DbContext, entity configurations, repositories
BlogPlatform.Api/             # Controllers, request/response models
```
