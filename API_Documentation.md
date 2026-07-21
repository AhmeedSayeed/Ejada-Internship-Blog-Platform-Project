# Blog Management Platform — API Documentation

**Base URL:** `/api`
**Auth scheme:** Bearer JWT (`Authorization: Bearer <accessToken>`), issued by `/api/auth/login`, renewed via `/api/auth/refresh-token`.
**Roles:** `Admin`, `Author`, `Reader` (see `AppRoles`).

This document mirrors the original requirements doc: each section number matches the feature number in `Blog_Project.pdf`. Endpoints under **Core APIs** correspond to features with no `[Optional]` tag; endpoints under **Optional APIs** correspond to features explicitly marked `[Optional]`.

---

## Core APIs

### 1. Authentication (core portion)

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/auth/register` | none | Create account, default role `Reader` |
| POST | `/auth/login` | none | Validate credentials, returns access + refresh token |
| POST | `/auth/refresh-token` | none (refresh token in body) | Rotate refresh token, issue new access token |
| POST | `/auth/logout` | Authorize | Revoke the supplied refresh token |

**Register — request**
```json
{ "email": "user@example.com", "userName": "user1", "password": "..." }
```
**Login — response**
```json
{ "accessToken": "...", "refreshToken": "..." }
```

### 2. User Profiles

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/users/me` | Authorize | Own profile (name, email, bio, image) |
| PUT | `/users/me` | Authorize | Edit name, email, bio, password (text fields only — JSON body) |
| POST | `/users/me/profile-image` | Authorize | Upload/replace profile image (`multipart/form-data`), returns new image URL |
| GET | `/users/{id}` | none | Public author profile — name, image, published posts |
| POST | `/users/{id}/follow` | Authorize (`Reader`) | Follow an author |
| DELETE | `/users/{id}/follow` | Authorize | Unfollow |
| GET | `/users/me/following` | Authorize | Authors I follow |
| GET | `/users/{id}/followers` | none | Who follows this author |

### 3. Post Management (core portion)

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/posts` | Authorize (`Author`) | Create post |
| PUT | `/posts/{id}` | Authorize (`Author`, owner) | Edit own post; writes a `PostEditHistory` snapshot |
| DELETE | `/posts/{id}` | Authorize (`Author`, owner) | Soft-delete own post |
| GET | `/posts/{id}` | none | Post detail |
| POST | `/posts/{id}/images` | Authorize (`Author`, owner) | Upload an image (`multipart/form-data`), returns URL to embed in rich-text content |
| DELETE | `/posts/{postId}/images/{imageId}` | Authorize (`Author`, owner) | Remove an uploaded image and delete its file |

### 4. Post Listing & Filtering

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/posts` | none | Paginated list. Query params: `page`, `pageSize`, `title`, `date`, `sortBy=recent\|rating` |

### 5. Commenting & Liking

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/posts/{postId}/comments` | Authorize (`Reader`) | Add comment or threaded reply (`parentCommentId` optional) |
| GET | `/posts/{postId}/comments` | none | List comments for a post, threaded |
| DELETE | `/comments/{id}` | Authorize (owner or `Admin`) | Soft-delete a comment |
| POST | `/comments/{id}/flag` | Authorize (`Reader`) | Flag a comment → `Status = Flagged` |
| PUT | `/comments/{id}/status` | Authorize (`Admin`) | Approve or reject a flagged comment |
| POST | `/posts/{postId}/like` | Authorize (`Reader`) | Like a post |
| DELETE | `/posts/{postId}/like` | Authorize | Unlike |

### 6. Rating Management

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/posts/{postId}/rating` | Authorize (`Reader`) | Rate 1–5; upsert if already rated |
| GET | `/posts/{postId}/rating` | none | Average rating + vote count |
| GET | `/authors/me/ratings` | Authorize (`Author`) | Ratings across all of my posts, for dashboard |

---

## Optional APIs

### 1b. Password Recovery & Email Verification `[Optional]`

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/auth/forgot-password` | none | Generate Identity reset token, email a reset link |
| POST | `/auth/reset-password` | none | Consume token, set new password |
| POST | `/auth/confirm-email` | none | Confirm email via Identity token |
| POST | `/auth/resend-confirmation` | none | Resend confirmation email |

### 3b. Tags & Categories `[Optional]`

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/categories` | none | List categories |
| POST | `/categories` | Authorize (`Admin`) | Create category |
| PUT | `/categories/{id}` | Authorize (`Admin`) | Edit category |
| DELETE | `/categories/{id}` | Authorize (`Admin`) | Delete category |
| GET | `/tags` | none | List tags |
| POST | `/tags` | Authorize (`Author`, `Admin`) | Create tag |
| PUT | `/posts/{id}/tags` | Authorize (`Author`, owner) | Assign tags to a post |

### 7. Draft Saving & Auto-Save `[Optional]`

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/posts/draft` | Authorize (`Author`) | Create a draft |
| PUT | `/posts/draft/{id}` | Authorize (`Author`, owner) | Auto-save / manual save (client calls every 30s) |
| GET | `/authors/me/drafts` | Authorize (`Author`) | List own drafts |

### 8. Publish Workflow `[Optional]`

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/posts/{id}/submit` | Authorize (`Author`, owner) | `Draft → PendingApproval`, sets `SubmittedAt` |
| PUT | `/posts/{id}/approve` | Authorize (`Admin`) | `PendingApproval → Approved`, sets `PublishedAt`, `ReviewedByAdminId` |
| PUT | `/posts/{id}/reject` | Authorize (`Admin`) | `PendingApproval → Rejected`, sets `ReviewedByAdminId` |
| GET | `/admin/posts/pending` | Authorize (`Admin`) | Posts awaiting review |

### 9. Admin Dashboard & Management `[Optional]`

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/admin/dashboard/stats` | Authorize (`Admin`) | Totals: posts, pending, rejected, top 10 recent |
| GET | `/admin/users` | Authorize (`Admin`) | List/search users |
| PUT | `/admin/users/{id}/suspend` | Authorize (`Admin`) | Toggle `IsSuspended` |
| POST | `/admin/users/{id}/reset-password` | Authorize (`Admin`) | Force-generate a reset token for a user |
| GET | `/admin/posts/{id}/edit-history` | Authorize (`Admin`) | `PostEditHistory` timeline for a post |

### 10. Analytics for Authors `[Optional]`

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/authors/me/analytics` | Authorize (`Author`) | Views, likes, comments per post |
| GET | `/authors/me/analytics/views-over-time` | Authorize (`Author`) | Time-series `PostView` data for charts |

### 11. Real-Time Notifications `[Optional]`

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/notifications` | Authorize | List own notifications |
| PUT | `/notifications/{id}/read` | Authorize | Mark as read |
| GET | `/notifications/unread-count` | Authorize | Count for the bell icon |
| WS | `/hubs/notifications` | Authorize | SignalR hub — push new notifications live |

---

## Notes

- Full request/response schemas aren't enumerated here — once controllers exist, Swagger/OpenAPI generates those automatically from the DTOs.
- Endpoints marked "owner" require checking `AuthorId`/`UserId` against the authenticated user's ID, not just role membership — role alone doesn't prove ownership of a specific resource.
- Delete-type endpoints assume the soft-delete/`IsDeleted` pattern already built into the generic repository, not a hard `DELETE FROM`.
- Image upload endpoints (`/posts/{id}/images`, `/users/me/profile-image`) accept `multipart/form-data`, not JSON, and are saved locally via `IFileStorageService` under `wwwroot/uploads`. Response shape:
  ```json
  { "url": "/uploads/posts/a1b2c3d4.jpg" }
  ```
  The returned `url` is what gets stored in `PostImage.ImageUrl` / `ApplicationUser.ProfileImageUrl` — never a server file path.
