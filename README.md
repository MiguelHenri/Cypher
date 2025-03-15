# Cypher

A password manager API built with ASP.NET Core and PostgreSQL.

## Functionalities

Register a new account and login. Then, create, read, update and delete your passwords securely.

## Dependencies

Make sure you have [.NET SDK](https://dotnet.microsoft.com/pt-br/download) installed. This repo uses version 9.0.201.

You must also have a [PostgreSQL](https://www.postgresql.org/download/) connection running locally or remotely.

## Env

Create a `.env` file inside the `/backend` directory and configure the environment variables as in `.env.example`:

```bash
DB_CONN="Host=[host name];Port=[port];Database=[db name];Username=[user name];Password=[password]"
JWT_KEY="[your key here]"
JWT_ISSUER="[issuer]"
JWT_AUDIENCE="[audience]"
```

## Run

To run the API, execute the following command inside the `/backend` directory:

```bash
dotnet run
```