# API Documentation

## 1. Users

### Authenticate User
**GET** `/api/users/auth`  
Checks the authentication status of the current user.

**Response:**
- `200 OK`: User is authenticated.
- `401 Unauthorized`: User is not authenticated.

---

### Register User
**POST** `/api/users/register`  
Registers a new user.

**Request Body:**
```json
{
  "name": "string",
  "email": "string",
  "hashedPassword": "string"
}
```

**Response:**
- `200 OK`: User successfully registered.
- `400 Bad Request`: Validation errors.

---

### Login User
**POST** `/api/users/login`  
Logs in a user.

**Request Body:**
```json
{
  "email": "string",
  "password": "string"
}
```

**Response:**
- `200 OK`: User is successfully logged in.
- `401 Unauthorized`: Invalid credentials.

---

## 2. Passwords

### Get All Passwords
**GET** `/api/passwords`
Retrieves all passwords for the authenticated user.

**Response:**
- `200 OK`: Returns a list of passwords.

### Create Password
**POST** `/api/passwords`
Creates a new password entry.

**Request Body:**
```json
{
  "serviceName": "string",
  "password": "string"
}

```
**Response:**
- `201 Created`: Password successfully created.
- `400 Bad Request`: Validation errors.

### Update Password
**PUT** `/api/passwords/{id}`
Updates an existing password entry.

**Request Body:**
```json
{
  "serviceName": "string",
  "password": "string"
}
```

**Response:**
- `200 OK`: Password successfully updated.
- `404 Not Found`: Password entry not found.

### Delete Password
**DELETE** `/api/passwords/{id}`
Deletes an existing password entry.

**Response:**
- `200 OK`: Password successfully deleted.
- `404 Not Found`: Password entry not found.