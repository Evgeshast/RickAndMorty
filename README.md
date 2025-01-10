
# C# Test Runner with Allure Reporting

This project includes a setup for automated testing with Allure reporting using .NET and GitHub Actions.

---

## Dependency Installation

To install all necessary dependencies, run the following commands:

1. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

2. **Install Required NuGet Packages** (if not already added in the project):
   ```bash
   dotnet add package NUnit
   dotnet add package NUnit3TestAdapter
   dotnet add package Allure.NUnit
   dotnet add package FluentAssertions
   ```

3. **Install Allure CLI**:
   - **For macOS**:
     ```bash
     brew install allure
     ```
   - **For Ubuntu**:
     ```bash
     sudo apt update
     sudo apt install -y allure
     ```
   - **For Windows**:
     1. Download the ZIP file from [Allure Releases](https://github.com/allure-framework/allure2/releases).
     2. Extract the archive and add the `bin` folder to your system `PATH` variable.

---

## Project Build

To prepare the project for running tests, use the following command:

```bash
dotnet build --configuration Release
```

This command will build the project in `Release` mode.

---

## Running Tests

To run automated tests, use the following command:

```bash
dotnet test --logger "trx;LogFileName=test_results.trx" --results-directory allure-results
```

- **Optional**: To run specific tests, add a filter:
  ```bash
  dotnet test --filter FullyQualifiedName=Tests.CharacterTest.GetAllCharacters_ShouldReturnSuccess --logger "trx;LogFileName=test_results.trx" --results-directory allure-results
  ```

Test results will be saved in the `allure-results` directory.

---

## Generating Reports

After running tests, use the following commands to generate an Allure report:

1. **Generate Report**:
   ```bash
   allure generate allure-results --clean -o allure-report
   ```

2. **View Report**:
   ```bash
   allure open allure-report
   ```

The report will open in your default browser.

---

## CI/CD (GitHub Actions)

The project includes a GitHub Actions workflow to automate testing and report generation. The workflow performs the following steps:

1. Builds the project.
2. Runs all tests or filters them based on input.
3. Generates an Allure report.
4. Saves the report as an artifact in GitHub Actions.
5. (Optional) Publishes the report to GitHub Pages.

To manually trigger the workflow:
1. Go to the **Actions** tab in your repository.
2. Select the workflow "C# Test Runner with Allure."
3. Click **Run workflow** and optionally specify a test filter.

---

## Project Structure

```
.
├── Common/                    # Shared utilities and helpers
├── Api/                       # Main API source code
├── Tests/                     # Test files
├── allure-results/            # Test results directory
├── allure-report/             # Generated Allure report directory
├── .github/workflows/         # GitHub Actions configuration
│   └── ci.yml                 # CI/CD workflow file
├── README.md                  # Project documentation
```

---

## Local Setup

To test the project locally, follow these steps:

1. Ensure all dependencies are installed (see the "Dependency Installation" section).
2. Build the project (see the "Project Build" section).
3. Run the tests (see the "Running Tests" section).
4. Generate and open the report (see the "Generating Reports" section).
