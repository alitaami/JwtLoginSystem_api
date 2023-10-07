**<h1>JwtLoginSystem API Documentation</h1>**
**<h3>Overview</h3>**

Welcome to the JwtLoginSystem API documentation. This API provides JWT authentication using access and refresh tokens. The API is equipped with Swagger for interactive documentation and testing.

**<h3>API Endpoints**</h3>
Use Swagger for test Endpoints.
**<h4>SignUp**</h4>

    Use this endpoint to register new users.
    Provide the necessary details for registration.

**<h4>Login</h4>**

    Authenticate using your username and password.
    Ensure the grantType is set to "password".
    Upon successful authentication, you'll receive an access token and a refresh token.

**<h4>RefreshToken**</h4>

If your access token expires, use the refresh token obtained during login to request a new access token:

    Set the Authorization header as: Bearer <Your-refreshToken>
    Provide your username in the Username field. (This step updates the refresh token of the user in the DB.)

**<h3>Docker Images**</h3>

The Docker images for this project are available as .tar files in my docker hub.
To use them write these in your command line:

```bash
 docker pull alitaami/jwtloginsystem_api-webapp:latest 
 docker pull alitaami/mssql_server:2019-GA-ubuntu-16.04 
 
