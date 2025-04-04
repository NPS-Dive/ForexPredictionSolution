# ForexPrediction

![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-blue)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.4.4-brightgreen)
![ML.NET](https://img.shields.io/badge/ML.NET-3.0.1-orange)

**ForexPrediction** is a .NET Framework-based application designed to predict forex currency pair prices using historical data and machine learning. It leverages Entity Framework 6 for data persistence, ML.NET for time series forecasting with the Singular Spectrum Analysis (SSA) algorithm, and ASP.NET Identity for user management. This project demonstrates a clean architecture with repository and unit of work patterns, making it a robust sample for learning and extension.

---

## Features

- **Data Upload**: Import daily forex data (e.g., from Alpha Vantage) in JSON format and store it in a database.
- **Time Series Forecasting**: Predict future forex prices using ML.NET’s SSA algorithm.
- **Buy/Sell Signals**: Generate trading signals based on predicted price trends.
- **User Management**: Integrated with ASP.NET Identity using `Guid` as the primary key type.
- **Clean Architecture**: Organized into Domain, Infrastructure, and Service layers with dependency injection support.
- **BaseEntity Pattern**: All entities inherit from a common `BaseEntity` class with a `Guid` ID.

---

## Project Structure

```
ForexPrediction/
├── Domain/
│   ├── Entities/
│   │   ├── ApplicationUser.cs       # User entity with ASP.NET Identity
│   │   ├── BaseEntity.cs           # Base class for all entities with Guid ID
│   │   ├── HistoricalData.cs       # Historical forex data entity
│   │   └── Prediction.cs           # Prediction entity with algorithm and signal
│   └── Interfaces/
│       ├── IDataService.cs         # Interface for data upload service
│       ├── IPredictionService.cs   # Interface for prediction service
│       ├── IRepository.cs          # Generic repository interface
│       └── IUnitOfWork.cs          # Unit of work interface
├── Infrastructure/
│   ├── Data/
│   │   └── ForexDbContext.cs       # EF6 DbContext with Identity support
│   └── Data/Repositories/
│       ├── Repository.cs           # Generic repository implementation
│       └── UnitOfWork.cs           # Unit of work implementation
│   └── Services/
│       ├── DataService.cs          # Service for uploading forex data
│       └── SsaPredictionService.cs # Service for SSA-based predictions
├── ForexPrediction.csproj           # Project file
└── README.md                       # This file
```

---

## Technologies Used

- **.NET Framework 4.8**: Core framework for the application.
- **Entity Framework 6.4.4**: ORM for database operations.
- **ASP.NET Identity**: User authentication and management with `Guid` keys.
- **ML.NET 3.0.1**: Machine learning framework for time series forecasting.
- **Newtonsoft.Json 13.0.3**: JSON serialization/deserialization.
- **SQL Server**: Default database (configurable via connection string).

---

## Prerequisites

- **Visual Studio 2019 or later**: With .NET Framework development workload.
- **SQL Server**: LocalDB or a full instance (e.g., SQL Server Express).
- **NuGet Packages**: Installed via the project file (see below).

---

## Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/ForexPrediction.git
cd ForexPrediction
```

### 2. Restore NuGet Packages
Open the solution in Visual Studio and restore the packages:
- Right-click the solution in Solution Explorer > `Restore NuGet Packages`.
- Or run:
  ```bash
  dotnet restore
  ```

Required packages (defined in `ForexPrediction.csproj`):
```xml
<PackageReference Include="EntityFramework" Version="6.4.4" />
<PackageReference Include="Microsoft.AspNet.Identity.EntityFramework" Version="2.2.3" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="Microsoft.ML" Version="3.0.1" />
<PackageReference Include="Microsoft.ML.TimeSeries" Version="3.0.1" />
```

### 3. Configure the Database
Edit `web.config` to set your SQL Server connection string:
```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=(localdb)\mssqllocaldb;Database=ForexDb;Trusted_Connection=True;MultipleActiveResultSets=true" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 4. Initialize the Database
- Open Package Manager Console in Visual Studio.
- Run the following commands:
  ```powershell
  Enable-Migrations
  Add-Migration InitialCreate
  Update-Database
  ```
This creates the `ForexDb` database with tables for `ApplicationUser`, `HistoricalData`, and `Prediction`.

### 5. Build the Solution
In Visual Studio: `Build > Rebuild Solution`.

---

## Usage

### Uploading Historical Data
The `DataService` uploads forex data from a JSON stream (e.g., Alpha Vantage format).

**Example:**
```csharp
using ForexPrediction.Infrastructure.Data;
using ForexPrediction.Infrastructure.Data.Repositories;
using ForexPrediction.Infrastructure.Services;
using System.IO;

class Program
{
    static async Task Main()
    {
        using (var context = new ForexDbContext())
        using (var unitOfWork = new UnitOfWork(context))
        {
            var dataService = new DataService(unitOfWork);
            var json = @"{
                ""Time Series (Daily)"": {
                    ""2025-04-04"": { 
                        ""1. open"": ""1.2345"", 
                        ""2. high"": ""1.2456"", 
                        ""3. low"": ""1.2234"", 
                        ""4. close"": ""1.2400"", 
                        ""5. volume"": ""1234567"" 
                    }
                }
            }";
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            {
                await dataService.UploadDataAsync("EURUSD", stream);
            }
        }
    }
}
```

### Generating Predictions
The `SsaPredictionService` predicts future prices using SSA.

**Example:**
```csharp
using ForexPrediction.Infrastructure.Data;
using ForexPrediction.Infrastructure.Data.Repositories;
using ForexPrediction.Infrastructure.Services;
using System;

class Program
{
    static async Task Main()
    {
        using (var context = new ForexDbContext())
        using (var unitOfWork = new UnitOfWork(context))
        {
            var predictionService = new SsaPredictionService(unitOfWork);
            var predictions = await predictionService.PredictAsync("EURUSD", DateTime.Now, 7);
            foreach (var pred in predictions)
            {
                Console.WriteLine($"Date: {pred.Date}, Predicted: {pred.PredictedValue}, Signal: {pred.Signal}");
            }
        }
    }
}
```

---

## Architecture Overview

- **Domain Layer**: Defines entities (`BaseEntity`, `ApplicationUser`, `HistoricalData`, `Prediction`) and interfaces (`IRepository`, `IUnitOfWork`, etc.).
- **Infrastructure Layer**:
  - **Data**: Contains `ForexDbContext` for EF6 database access.
  - **Repositories**: Implements `Repository<T>` and `UnitOfWork` for data operations.
  - **Services**: Includes `DataService` for data upload and `SsaPredictionService` for ML predictions.
- **BaseEntity**: All entities inherit a `Guid` `Id` for consistency.

---

## Extending the Project

- **Add More Algorithms**: Implement additional prediction methods (e.g., LSTM) in new `IPredictionService` implementations.
- **Web API**: Add an ASP.NET MVC or Web API layer to expose data upload and prediction endpoints.
- **Real-Time Data**: Integrate with a live forex data API (e.g., Alpha Vantage, Yahoo Finance).
- **UI**: Build a frontend (e.g., Razor pages or Angular) to visualize predictions.

---

## Contributing

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/new-feature`).
3. Commit your changes (`git commit -m "Add new feature"`).
4. Push to the branch (`git push origin feature/new-feature`).
5. Open a pull request.

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Contact

For questions or feedback, feel free to open an issue or contact me at [your-email@example.com](mailto:amirmohammadshi@gmail.com).

---

Replace placeholders like `yourusername` and `your-email@example.com` with your actual GitHub username and email. Add a `LICENSE` file to your repository if you choose to include one (e.g., MIT License text).

This `README.md` should effectively showcase your project on GitHub, providing clear instructions and context for potential users or contributors. Let me know if you’d like to tweak it further!
