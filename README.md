# Library Management System

A library management system project using 3-layer architecture (.NET 10).

## Architecture

This project follows the **3-Layer Architecture** pattern:

### 1. **Presentation Layer (Library.PL)**
- Console application
- User interface and interaction
- Only communicates with BLL

### 2. **Business Logic Layer (Library.BLL)**
- Business rules and validation
- Communicates with DAL
- Returns user-friendly messages

### 3. **Data Access Layer (Library.DAL)**
- CRUD operations
- File/Database access
- No business logic

### 4. **Core Layer (Library.Core)**
- Entities (Book)
- Interfaces (IRepository)
- Constants (Messages, AppSettings)
- Helpers (PathHelper)

## Features

- ✅ View all books
- ✅ Search books (by title or author)
- ✅ Add new book
- ✅ Update book information
- ✅ Delete book

## Requirements

- .NET 10 SDK
- macOS/Windows/Linux

## How to Run

```bash
# Clone the repository
git clone https://github.com/Ming1309/LibraryManagementSystem.git

# Navigate to project directory
cd LibraryManagementSystem

# Run the application
dotnet run --project Library.PL
```

## Project Structure

```
LibraryManagementSystem/
├── Library.Core/           # Domain models, interfaces, constants
│   ├── Book.cs
│   ├── Interfaces/
│   │   └── IRepository.cs
│   ├── Constants/
│   │   ├── Messages.cs
│   │   └── AppSettings.cs
│   └── Helpers/
│       └── PathHelper.cs
├── Library.DAL/            # Data access layer
│   ├── BookRepository.cs
│   └── data.json
├── Library.BLL/            # Business logic layer
│   └── BookService.cs
└── Library.PL/             # Presentation layer
    └── Program.cs
```

## Data Storage

- **Development**: Data stored in `Library.DAL/data.json`
- **Production**: Data stored in application directory

The application automatically detects the environment using `#if DEBUG` directive.

## Validation Rules

- Book title cannot be empty
- Book title must be at least 2 characters
- Publish year must be greater than 0
- Publish year cannot exceed current year

## Author

Ming1309
