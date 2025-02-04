CRUD Operations with Semantic Kernel and OpenAI
This project demonstrates how to perform CRUD (Create, Read, Update, Delete) operations using the Semantic Kernel and OpenAI. The application is built using ASP.NET Core and leverages the power of OpenAI for natural language processing.

Table of Contents
Prerequisites
Installation
Configuration
Usage
API Endpoints
Example Requests
Contributing
License
Prerequisites
.NET 6 SDK
OpenAI API Key
GitHub Codespaces (optional)
Installation
Clone the repository:

git clone https://github.com/your-username/your-repo-name.git
cd your-repo-name
Restore the dependencies:

dotnet restore
Build the project:

dotnet build
Configuration
Set up OpenAI Service:

Obtain your OpenAI API key from the OpenAI website.
Update the ToDoPlugin class with your OpenAI credentials.
Configure Dependency Injection:

Ensure HttpClient and ToDoPlugin are registered in Program.cs or Startup.cs.
Database Configuration:

The project uses an in-memory database for demonstration purposes. You can configure it to use a persistent database by modifying the DbContext setup in Startup.cs.
Usage
Run the Application:

dotnet run
Access the Swagger UI:

Open your browser and navigate to https://localhost:5076/swagger to explore the API endpoints.
API Endpoints
Get All ToDos: GET /todo/GetAll
Get Completed ToDos: GET /todo/GetAllCompleteStatus
Get ToDo by ID: GET /todo/GetOne/{id}
Create New ToDo: POST /todo/Create
Delete ToDo: DELETE /todo/Remove/{id}
Example Requests
Create a New ToDo
POST /todo/Create
Content-Type: application/json

{
  "name": "New Task",
  "isComplete": false
}
Get All ToDos
GET /todo/GetAll
Get ToDo by ID
GET /todo/GetOne/1
Delete a ToDo
DELETE /todo/Remove/1
Contributing
Contributions are welcome! Please fork the repository and create a pull request with your changes.

License
This project is licensed under the MIT License. See the LICENSE file for details.
