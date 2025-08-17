# BookStore.Api

## Description

This project is a RESTful API for a Book Store, built using ASP.NET Core. It implements the Repository and Unit of Work patterns for data access, promoting separation of concerns and testability. The API provides endpoints for managing categories and products, including features like pagination, caching, and versioning.

## Features and Functionality

*   **Category Management:**
    *   Create, read, update, and delete categories.
    *   Retrieval of categories with pagination.
    *   In-memory caching for frequently accessed category data.
*   **Product Management:**
    *   Create, read, update (full and partial using JSON Patch), and delete products.
    *   API Versioning to support multiple versions of the product endpoints.
    *   Response caching to improve API performance.
*   **Data Access:**
    *   Implements the Repository pattern for interacting with the database.
    *   Uses the Unit of Work pattern to manage transactions and ensure data consistency.
*   **API Versioning:** Supports API versioning using URL segment.
*   **Caching:** Implements Memory caching for Category data to enhance response times.
*   **Response Caching:** Implements response caching at Controller level with Cache Profiles.
*   **Validation:** Data validation using Data Annotations in DTOs.
*   **Testing:** Unit tests for Category and Product controllers.
*   **Seeding:** Seed data for Products and Categories to ensure the application has data to display
*   **Indexing:** Unique Indexing For Category CatName

## Technology Stack

*   ASP.NET Core 8.0
*   Entity Framework Core
*   SQL Server
*   AutoMapper
*   Asp.Versioning.Mvc
*   Microsoft.AspNetCore.JsonPatch
*   Microsoft.Extensions.Caching.Memory
*   Moq (for testing)
*   xUnit (for testing)

## Prerequisites

*   .NET SDK 8.0 or later
*   SQL Server instance
*   An IDE such as Visual Studio or Visual Studio Code

## Installation Instructions

1.  Clone the repository:

    ```bash
    git clone https://github.com/muhammadabdelgawad/BookStore.Api-
    cd BookStore.Api-
    ```

2.  Update the database connection string:

    *   Open `Data Access/Data/ApplicationDbContext.cs`.
    *   Locate the `options.UseSqlServer()` method in the `Program.cs` file.
    *   Modify the connection string to point to your SQL Server instance. Example:

        ```csharp
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        ```

        Ensure the "DefaultConnection" string exists in your `appsettings.json` or user secrets.

3.  Apply database migrations:

    ```bash
    cd Data Access
    dotnet ef database update -p Data Access -s ..\BookStore.Api
    ```

4.  Build and run the API:

    ```bash
    cd BookStore.Api
    dotnet build
    dotnet run
    ```

## Usage Guide

The API will be accessible at `https://localhost:<port>/api/v{version}/[controller]`, where `<port>` is the port number configured in your `launchSettings.json` file (typically 5001 for HTTPS).

### Category Endpoints

*   `GET /api/v1/Category`: Retrieves all categories (paginated). Accepts optional query parameters `pageNumber` and `pageSize`. Example: `GET /api/v1/Category?pageNumber=2&pageSize=10`.
*   `GET /api/v1/Category/{id}`: Retrieves a category by its ID.
*   `POST /api/v1/Category`: Creates a new category. Requires a JSON payload in the request body matching the `CreateCategoryDto` structure (e.g., `{"CatName": "Fiction", "CatOrder": 1}`).
*   `PUT /api/v1/Category/{id}`: Updates an existing category. Requires a JSON payload in the request body matching the `UpdateCategoryDto` structure (e.g., `{"CatName": "Updated Fiction", "CatOrder": 2}`).
*   `DELETE /api/v1/Category/{id}`: Deletes a category.

### Product Endpoints

*   `GET /api/v1/Product`: Retrieves all products (Version 1).
*   `GET /api/v2/Product`: Retrieves all products (Version 2).
*   `GET /api/v1/Product/{id}`: Retrieves a product by its ID.
*   `POST /api/v1/Product`: Creates a new product. Requires a JSON payload in the request body matching the `CreateProductDto` structure.
    Example: `{"Title": "New Book", "Author": "Author Name", "Price": 25.00, "CategoryId": 1}`.
*   `PUT /api/v1/Product/{id}`: Updates an existing product. Requires a JSON payload in the request body matching the `UpdateProductDto` structure.
*   `PATCH /api/v1/Product/{id}`: Partially updates an existing product using JSON Patch.  Example: `[{"op": "replace", "path": "/Title", "value": "Updated Title"}]`.
*   `DELETE /api/v1/Product/{id}`: Deletes a product.

## API Documentation

The API uses Swagger for documentation. Once the application is running, you can access the Swagger UI by navigating to `https://localhost:<port>/swagger` in your browser.  This provides an interactive interface for exploring the API endpoints, request parameters, and response schemas.

## Contributing Guidelines

Contributions are welcome! To contribute to this project, please follow these steps:

1.  Fork the repository.
2.  Create a new branch for your feature or bug fix.
3.  Implement your changes, ensuring code quality and adherence to the project's coding standards.
4.  Write unit tests for your changes.
5.  Submit a pull request with a clear description of your changes.

## License Information

This project is licensed under the MIT License. See the `LICENSE` file in the repository for more information.

```
MIT License

Copyright (c) 2024 Muhammad Abdelgawad

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

