# User Management .NET Core Solution

This repository contains a .NET Core solution for User Management. The solution includes a Docker Web service for executing user management through swagger.

## Getting Started

These instructions will help you get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

To run the application, you need to have the following software installed on your machine:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop)
- [Visual Studio IDE](https://visualstudio.microsoft.com/vs/)

### Installation

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/AnzeS/mikrocop.git
   ```

2. Navigate to the root directory of the project:

   ```bash
   cd mikrocop/Mikrocop.ManagingUsers
   ```

3. Build the Docker image for the user management service:

   ```bash
   docker-compose build
   ```

### Develop

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/AnzeS/mikrocop.git
   ```

2. Navigate to the root directory of the project:

   ```bash
   cd mikrocop/Mikrocop.ManagingUsers
   ```

3. Open solution `Mikrocop.ManagingUsers.sln` in Visual Studio IDE

### Usage

#### Docker Service

1. Navigate to the `Mikrocop.ManagingUsers` directory:

   ```bash
   cd mikrocop/Mikrocop.ManagingUsers
   ```

2. Run the Docker container for the exchange service:

   ```bash
   docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
   ```

3. Access the exchange swagger through your web browser at `http://localhost:8088/swagger/index.html`.

4. Client API-Keys can be found in file 'mikrocop/Mikrocop.ManagingUsers/appsettings.Development.json' under 'ClientKeys' section.