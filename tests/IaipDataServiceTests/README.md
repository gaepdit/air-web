# Database Integration Tests

The tests in the "DbTests" folder require a connection to a working database populated with data. The values in the
tests were selected using data from the current `airbranch` dev database.

*Important:* Because the tests in this project access a working database, it's possible that ordinary changes to the
data may causes tests to begin failing.

## Setup

Create a *testsettings.json* file in the "DbTests" folder with a valid connection string:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=***"
    }
}
```
