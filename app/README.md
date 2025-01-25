# My ASP.NET Core App

This is a sample ASP.NET Core application that demonstrates the use of MVC architecture. 

## Project Structure

- **Controllers**: Contains the controllers that handle HTTP requests.
  - `HomeController.cs`: Manages requests related to the home page.

- **Models**: Contains the data models used in the application.
  - `HomeModel.cs`: Represents the data structure for the home page.

- **Views**: Contains the Razor views for rendering HTML.
  - **Home**: Contains views related to the home page.
    - `Index.cshtml`: The main view for the home page.
  - **Shared**: Contains shared layout views.
    - `_Layout.cshtml`: The common layout for all pages.

- **wwwroot**: Contains static files such as CSS, JavaScript, and third-party libraries.
  - **css**: Contains stylesheets.
    - `site.css`: The main stylesheet for the application.
  - **js**: Contains JavaScript files.
    - `site.js`: The main JavaScript file for the application.
  - **lib**: Directory for third-party libraries.

- **Configuration Files**:
  - `appsettings.json`: Contains configuration settings for the application.
  - `Program.cs`: The entry point of the application.
  - `Startup.cs`: Configures services and the application's request pipeline.

## Getting Started

1. Clone the repository.
2. Navigate to the project directory.
3. Run the application using the command `dotnet run`.
4. Open your browser and go to `http://localhost:5000` to view the home page.

## Contributing

Feel free to submit issues or pull requests for improvements or bug fixes.