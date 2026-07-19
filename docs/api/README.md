# API Usage Guide

## Base URL

Run the HTTP profile locally:

```bash
```

The base URL is `http://localhost:5185`. Swagger UI is available at
`http://localhost:5185/swagger` and the OpenAPI document is available at
`http://localhost:5185/swagger/v1/swagger.json`.

## Posts

### List Posts

```http
GET /api/posts
```

Response `200`:

```json
[
  {
    "id": 1,
    "title": "A title",
    "commentCount": 1
  }
]
```

An empty database returns `200` with `[]`.

### Create Post

```http
POST /api/posts
Content-Type: application/json

{
  "title": "A title",
  "content": "Post content"
}
```

Response `201`:

```json
{
  "id": 1,
  "title": "A title",
  "commentCount": 0
}
```

Missing, empty, whitespace-only, or oversized values return `400`.

## Comments

### Get Post Detail

```http
GET /api/posts/1
```

Response `200`:

```json
{
  "id": 1,
  "title": "A title",
  "content": "Post content",
  "comments": [
    {
      "id": 1,
      "postId": 1,
      "content": "A comment"
    }
  ]
}
```

Missing posts return `404`. Invalid identifiers return `400`.

### Create Comment

```http
POST /api/posts/1/comments
Content-Type: application/json

{
  "content": "A comment"
}
```

Response `201`:

```json
{
  "id": 1,
  "postId": 1,
  "content": "A comment"
}
```

Missing posts return `404`; invalid identifiers or content return `400`.

## Search Posts

```http
GET /api/posts/search?title=api&hasComments=true&page=1&pageSize=20&sortBy=-title&sortBy=commentCount
```

All filters are optional and are combined with `AND`:

- `title`: case-insensitive title text filter.
- `content`: case-insensitive content text filter.
- `hasComments`: filters posts with or without comments.
- `minCommentCount` and `maxCommentCount`: comment count range.
- `page` and `pageSize`: one-based pagination; page size is limited to 100.
- `sortBy`: repeatable fields `id`, `title`, `content`, or `commentCount`; prefix a
  field with `-` for descending order. Fields are applied in the supplied order.

Response `200`:

```json
{
  "items": [
    {
      "id": 1,
      "title": "API post",
      "commentCount": 2
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalCount": 1
}
```

Invalid ranges, pagination values, or sort fields return `400`.


## Error Contract

Expected validation and resource errors use:

```json
{
  "error": "A descriptive error message."
}
```

Unexpected failures return `500` without stack traces, SQL, secrets, or user input.

## Database and Migrations

The API uses `Microsoft.EntityFrameworkCore.Sqlite`. The local file is configured
through `BlogDatabase:ConnectionString` and ignored by Git. Startup checks
`GetPendingMigrationsAsync` and calls `MigrateAsync` only when the schema is behind
the model. Tests use isolated SQLite databases.

## Validation Commands

```bash
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=cobertura \
  /p:Threshold=85 \
  /p:ThresholdType=line%2cbranch \
  /p:ThresholdStat=total
```
