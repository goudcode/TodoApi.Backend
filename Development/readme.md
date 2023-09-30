# Development docker compose
Use this to aid development. Contains:
- PostgresSQL database
- PgAdmin to manage it

## Usage
```sh
docker compose up -d
```

Administration interface can be found on: http:

## Connection string
Connection string for in the ``appsettings.json`` looks like this
``` json
"ConnectionStrings": {
    "TodoDatabase": "Server=localhost;User Id=root;Password=root;Database=test_db"
}
```