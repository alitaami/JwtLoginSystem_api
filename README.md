**<h1>JwtLoginSystem API Documentation</h1>**
**<h3>Overview</h3>**

Welcome to the JwtLoginSystem API documentation. This API provides JWT authentication using access and refresh tokens. The API is equipped with Swagger for interactive documentation and testing.

**<h3>API Endpoints**</h3>
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

The Docker images for this project are available as .tar files in this repository.

To use them:

    1-Download the .tar files.
    2-Load them into Docker using the following commands:

    docker load -i /path/where/you/saved/jwtloginsystem_api-webapp.tar
    docker load -i /path/where/you/saved/mssql_server.tar
 
