# Cypher

A password manager built with ASP.NET Core, PostgreSQL and React.

## Functionalities

Register a new account and login. Then, create, read, update and delete your passwords securely.

## Dependencies

Make sure you have [.NET SDK](https://dotnet.microsoft.com/pt-br/download) installed. This repo uses version 9.0.201.

You must also have a [PostgreSQL](https://www.postgresql.org/download/) connection running locally or remotely.

Finally, you will need [Node.js](https://nodejs.org/pt/download) installed. This repo uses version 22.14.0.

## Env

Create a `.env` file inside the `/backend` directory and configure the environment variables as in `.env.example`:

```bash
DB_CONN="Host=[host name];Port=[port];Database=[db name];Username=[user name];Password=[password]"
JWT_KEY="[your key here]"
JWT_ISSUER="[issuer]"
JWT_AUDIENCE="[audience]"
FRONTEND_URL="[front end url here]"
```

Also, create a `.env` file inside the `/frontend` directory and do the same:

```bash
VITE_BACKEND_URL="[backend url here]"
```

## Run

To run the API, execute the following command inside the `/backend` directory:

```bash
dotnet run
```

To run the interface, execute the following commands inside the `/frontend` directory:

```bash
npm install
```
- For installing all dependencies.

```bash
npm run dev
```

- This will execute the development environment.