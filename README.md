BlogManagementAPI_RunInstructions:
  Title: "Blog Management API - Run Instructions"
  Prerequisites:
    - ".NET 6 SDK or later - [Download .NET SDK](https://dotnet.microsoft.com/download/dotnet)"
    - "SQL Server (if you are using a database other than the in-memory database)"
    - "Git (for cloning the repository) - [Download Git](https://git-scm.com/)"
    - "Postman or any API testing tool (optional, for testing API endpoints)"
  StepsToRunAPI:
    - Step1:
        Title: "Clone the Repository"
        Description: |
          First, clone the repository from GitHub:
          ```bash
          git clone https://github.com/pk95955/BlogManagementAPI.git
          ```
          Navigate into the project directory:
          ```bash
          cd BlogManagementAPI
          ```
    - Step2:
        Title: "Set Up the Database (Optional)"
        Description: |
          If you are using a database other than the in-memory JSON database (like SQL Server or PostgreSQL), ensure you have the connection string set up in the `appsettings.json` file.
          ```json
          {
            "ConnectionStrings": {
              "DefaultConnection": "Server=YOUR_SERVER;Database=BlogManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true"
            }
          }
          ```
    - Step3:
        Title: "Run Database Migrations (Optional)"
        Description: |
          If you're using a database like SQL Server, run the following command to apply migrations and create the necessary tables:
          ```bash
          dotnet ef database update
          ```
    - Step4:
        Title: "Restore NuGet Packages"
        Description: |
          Ensure all the required packages are installed. You can do this by running the following command in the project directory:
          ```bash
          dotnet restore
          ```
    - Step5:
        Title: "Build the Solution"
        Description: |
          Build the project to ensure there are no errors:
          ```bash
          dotnet build
          ```
    - Step6:
        Title: "Run the Application"
        Description: |
          Now you can run the API:
          ```bash
          dotnet run
          ```
          By default, the API will run on `http://localhost:5000` and `https://localhost:5001`.
    - Step7:
        Title: "Test the API Endpoints"
        Description: |
          You can test the API using Postman or any other API testing tool. For example, to retrieve all blog posts, make a `GET` request to:
          ```bash
          GET https://localhost:5001/api/BlogPost
          ```
          Ensure that if you have authorization enabled, you pass the generated JWT token in the `Authorization` header as a `Bearer` token.
    - Step8:
        Title: "Swagger UI (Optional)"
        Description: |
          If Swagger is enabled, you can access the Swagger UI for API documentation and testing by visiting:
          ```bash
          https://localhost:5001/swagger
          ```

ConfigurationNotes:
  Title: "Configuration Notes"
  Notes:
    - "The `appsettings.json` file is used to configure database connections and other settings."
    - "For JWT authentication, ensure that your token configuration in `appsettings.json` matches what you're using in the frontend."

CommonIssues:
  Title: "Common Issues"
  Issues:
    - "401 Unauthorized: Ensure you are passing the JWT token correctly in the `Authorization` header."
    - "500 Internal Server Error: Check the logs to identify the issue, especially if it's related to database connectivity or missing migrations."

