# Search Rank Checker

A small tool that allows users to check where a given URL appears in Google's top 100 search results for a specific keyword.

---

## Tech Stack

- **Backend**: ASP.NET Core 8 Web API
- **Frontend**: Vue 3 + Vite
- **Tests**: xUnit

---

## Features

- Accepts a search keyword and a URL from the user
- Performs a Google search (without using 3rd-party libraries or APIs)
- Parses the first 100 results and returns the positions where the URL appears (e.g. `1, 10, 33` or `0`)
- Displays results or errors in a simple web interface
- Includes unit tests that use real saved HTML responses from Google

---

## Project Structure

```
SearchRankChecker.Api/       - .NET backend API
SearchRankChecker.Tests/     - xUnit test project for backend parser
search-rank-checker-frontend/ - Vue 3 frontend (SPA using Vite)
```

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js & npm](https://nodejs.org/) (for frontend)
- A modern browser

---

## Getting Started

### 1. Run the Backend

```bash
cd SearchRankChecker.Api
dotnet run
```

By default, the API will run on:

```
https://localhost:7038
```

### 2. Run the Frontend

In a new terminal:

```bash
cd search-rank-checker-frontend
npm install
npm run dev
```

The frontend should be available at:

```
http://localhost:5173
```

---

## Example Usage

Search for:

- Keyword: `land registry search`  
- URL: `www.infotrack.co.uk`

If found in Google's top 100 results, the app will return positions like:

```
Found at positions 1, 16, 33
```

If not found:

```
URL not found in top 100 search results.
```

---

## Notes

- The project scrapes HTML directly from Google Search using a browser-like user-agent.
- It uses minimal dependencies to meet the no third-party scraping library requirement.
- Google may temporarily block repeated requests. The app detects this and shows a friendly error.

---

## Tests

The test project includes real-world HTML samples saved from Google search result pages.

To run tests:

```bash
cd SearchRankChecker.Tests
dotnet test
```

---

## Submission

To run the app locally:

1. Clone the repository
2. Run the backend (`dotnet run`)
3. Run the frontend (`npm install && npm run dev`)
4. Open http://localhost:5173 in your browser