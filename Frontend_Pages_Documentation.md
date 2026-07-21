# Blog Management Platform — Frontend Pages Documentation (ASP.NET MVC)

Section numbers match `API_Documentation.md` and the original requirements doc, so each page can be cross-referenced to the endpoints it calls.

---

## Core Pages

### 1. Authentication & Account

| Page | Route | Roles | Calls |
|---|---|---|---|
| Login | `/Account/Login` | anonymous | `POST /auth/login` |
| Register | `/Account/Register` | anonymous | `POST /auth/register` (role choice: Reader or Author only) |
| Access Denied | `/Account/AccessDenied` | anonymous | — |

### 2. User Profile

| Page | Route | Roles | Calls |
|---|---|---|---|
| My Profile / Edit | `/Profile/Manage` | Authorize | `GET`/`PUT /users/me` |
| Public Author Profile | `/Profile/{id}` | anonymous | `GET /users/{id}` |
| Following List | `/Profile/Following` | Authorize | `GET /users/me/following` |
| Followers List | `/Profile/{id}/Followers` | anonymous | `GET /users/{id}/followers` |

### 3. Post Management

| Page | Route | Roles | Calls |
|---|---|---|---|
| Create Post | `/Posts/Create` | Author | `POST /posts`, `POST /posts/{id}/images` |
| Edit Post | `/Posts/Edit/{id}` | Author (owner) | `PUT /posts/{id}` |
| My Posts | `/Posts/Mine` | Author | `GET /posts?authorId=me` |
| Post Details | `/Posts/Details/{id}` | anonymous | `GET /posts/{id}`, `GET /posts/{id}/comments`, `GET /posts/{id}/rating` |

### 4. Post Listing & Filtering

| Page | Route | Roles | Calls |
|---|---|---|---|
| Home / Feed | `/` or `/Posts` | anonymous | `GET /posts` (filters: title, date; sort: recent/rating) |

### 5. Commenting & Liking

Comment form, thread, like button, and flag action render **inside** the Post Details page — not separate pages. Only the admin queue needs its own page:

| Page | Route | Roles | Calls |
|---|---|---|---|
| Comment Moderation Queue | `/Admin/Comments` | Admin | `GET` flagged comments, `PUT /comments/{id}/status` |

### 6. Rating Management

Rating widget renders inside Post Details. Author-facing summary gets its own page:

| Page | Route | Roles | Calls |
|---|---|---|---|
| My Ratings Dashboard | `/Author/Ratings` | Author | `GET /authors/me/ratings` |

---

## Optional Pages

### 1b. Password Recovery `[Optional]`

| Page | Route | Roles | Calls |
|---|---|---|---|
| Forgot Password | `/Account/ForgotPassword` | anonymous | `POST /auth/forgot-password` |
| Reset Password | `/Account/ResetPassword` | anonymous | `POST /auth/reset-password` |

### 3b. Tags & Categories `[Optional]`

| Page | Route | Roles | Calls |
|---|---|---|---|
| Manage Categories | `/Admin/Categories` | Admin | `GET`/`POST`/`PUT`/`DELETE /categories` |
| Manage Tags | `/Admin/Tags` | Admin | `GET`/`POST /tags` |
| Browse by Category | `/Posts/Category/{id}` | anonymous | `GET /posts?categoryId=` |
| Browse by Tag | `/Posts/Tag/{id}` | anonymous | `GET /posts?tagId=` |

### 7. Draft Saving & Auto-Save `[Optional]`

| Page | Route | Roles | Calls |
|---|---|---|---|
| My Drafts | `/Posts/Drafts` | Author | `GET /authors/me/drafts` |

Auto-save runs from JS on the Create/Edit page itself (periodic `PUT /posts/draft/{id}`) — not a separate page.

### 8. Publish Workflow `[Optional]`

| Page | Route | Roles | Calls |
|---|---|---|---|
| Pending Review Queue | `/Admin/PendingPosts` | Admin | `GET /admin/posts/pending`, `PUT /posts/{id}/approve`, `PUT /posts/{id}/reject` |

"Submit for review" is a button on the Edit Post page, not a separate page.

### 9. Admin Dashboard & Management `[Optional]`

| Page | Route | Roles | Calls |
|---|---|---|---|
| Admin Dashboard | `/Admin` | Admin | `GET /admin/dashboard/stats` |
| Manage Users | `/Admin/Users` | Admin | `GET /admin/users`, `PUT /admin/users/{id}/suspend`, `PUT /admin/users/{id}/role` |
| Post Edit History | `/Admin/Posts/{id}/History` | Admin | `GET /admin/posts/{id}/edit-history` |

### 10. Analytics for Authors `[Optional]`

| Page | Route | Roles | Calls |
|---|---|---|---|
| Author Analytics | `/Author/Analytics` | Author | `GET /authors/me/analytics`, `GET /authors/me/analytics/views-over-time` |

### 11. Real-Time Notifications `[Optional]`

| Page | Route | Roles | Calls |
|---|---|---|---|
| Notifications | `/Notifications` (+ navbar dropdown partial) | Authorize | `GET /notifications`, `PUT /notifications/{id}/read`, SignalR hub `/hubs/notifications` |

---

## Shared / Utility Pages

| Page | Route | Notes |
|---|---|---|
| 404 Not Found | `/Error/404` | Configured via `app.UseStatusCodePagesWithReExecute` |
| 500 Server Error | `/Error/500` | Configured via `app.UseExceptionHandler` |
| Shared Layout | `_Layout.cshtml` | Navbar changes by role: guest / Reader / Author / Admin links |
