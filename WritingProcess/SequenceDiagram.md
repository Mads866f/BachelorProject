# Start Messages
kbem@pop-os:~/Desktop/BachelorProjekt/BachelorProject$ docker compose -f compose.yaml up
[+] Running 4/0
 ✔ Container Postgres_db                 Created                                                                                                                                         0.0s
 ✔ Container bachelorproject-pbengine-1  Created                                                                                                                                         0.0s
 ✔ Container bachelorproject-backend-1   Created                                                                                                                                         0.0s
 ✔ Container bachelorproject-frontend-1  Created                                                                                                                                         0.0s
Attaching to Postgres_db, backend-1, frontend-1, pbengine-1
Postgres_db  |
Postgres_db  | PostgreSQL Database directory appears to contain a database; Skipping initialization
Postgres_db  |
Postgres_db  | 2025-05-13 13:41:50.841 UTC [1] LOG:  starting PostgreSQL 17.4 (Debian 17.4-1.pgdg120+2) on x86_64-pc-linux-gnu, compiled by gcc (Debian 12.2.0-14) 12.2.0, 64-bit
Postgres_db  | 2025-05-13 13:41:50.842 UTC [1] LOG:  listening on IPv4 address "0.0.0.0", port 5432
Postgres_db  | 2025-05-13 13:41:50.842 UTC [1] LOG:  listening on IPv6 address "::", port 5432
Postgres_db  | 2025-05-13 13:41:50.851 UTC [1] LOG:  listening on Unix socket "/var/run/postgresql/.s.PGSQL.5432"
Postgres_db  | 2025-05-13 13:41:50.856 UTC [29] LOG:  database system was shut down at 2025-05-13 13:41:48 UTC
Postgres_db  | 2025-05-13 13:41:50.863 UTC [1] LOG:  database system is ready to accept connections
pbengine-1   | INFO:     Started server process [1]
pbengine-1   | INFO:     Waiting for application startup.
pbengine-1   | INFO:     Application startup complete.
pbengine-1   | INFO:     Uvicorn running on http://0.0.0.0:8000 (Press CTRL+C to quit)
frontend-1   | warn: Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository[60]
frontend-1   |       Storing keys in a directory '/home/app/.aspnet/DataProtection-Keys' that may not be persisted outside of the container. Protected data will be unavailable when container is destroyed. For more information go to https://aka.ms/aspnet/dataprotectionwarning
backend-1    | info: Microsoft.Hosting.Lifetime[14]
backend-1    |       Now listening on: http://[::]:8080
backend-1    | info: Microsoft.Hosting.Lifetime[0]
backend-1    |       Application started. Press Ctrl+C to shut down.
backend-1    | info: Microsoft.Hosting.Lifetime[0]
backend-1    |       Hosting environment: Production
backend-1    | info: Microsoft.Hosting.Lifetime[0]
backend-1    |       Content root path: /app
frontend-1   | info: Microsoft.Hosting.Lifetime[14]
frontend-1   |       Now listening on: http://[::]:8080
frontend-1   | info: Microsoft.Hosting.Lifetime[0]
frontend-1   |       Application started. Press Ctrl+C to shut down.
frontend-1   | info: Microsoft.Hosting.Lifetime[0]
frontend-1   |       Hosting environment: Production
frontend-1   | info: Microsoft.Hosting.Lifetime[0]
frontend-1   |       Content root path: /app
frontend-1   | warn: Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware[3]
frontend-1   |       Failed to determine the https port for redirect.

# Creating Election:

frontend-1   | info: Front.Services.ApiService.ElectionsApiService[0]
frontend-1   |       Creating Election with id: 00000000-0000-0000-0000-000000000000
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request POST http://backend:8080/api/Election
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/api/Election
backend-1    | warn: Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware[3]
backend-1    |       Failed to determine the https port for redirect.
backend-1    | info: Backend.Controllers.DataControllers.ElectionController[0]
backend-1    |       Creating Election with name: HAHAAH
backend-1    | info: Backend.Services.DataServices.ElectionService[0]
backend-1    |       Creating election
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 193.9909ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 201.673ms - 200

## Navigates to elections page
frontend-1   | info: Front.Services.ApiService.ElectionsApiService[0]
frontend-1   |       Getting Elections
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/Election
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/Election
frontend-1   | info: Front.Services.ApiService.ElectionsApiService[0]
frontend-1   |       Getting Elections
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/Election?status=Open
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/Election?status=Open
backend-1    | info: Backend.Controllers.DataControllers.ElectionController[0]
backend-1    |       Getting Elections with status: all
backend-1    | info: Backend.Services.DataServices.ElectionService[0]
backend-1    |       Getting all elections
backend-1    | info: Backend.Controllers.DataControllers.ElectionController[0]
backend-1    |       Getting Elections with status: Open
backend-1    | info: Backend.Services.DataServices.ElectionService[0]
backend-1    |       Getting open elections
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 44.5858ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 51.3059ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 44.8787ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 51.4712ms - 200
frontend-1   | info: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Getting voters by election id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/voters/election/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/voters/election/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Controllers.DataControllers.VotersController[0]
backend-1    |       Fetching all voters from election with id 68095015-d9f8-41f7-9de7-1baef771f135.
backend-1    | info: Backend.Services.DataServices.VoterService[0]
backend-1    |       Getting voters for election with id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | warn: Backend.Controllers.DataControllers.VotersController[0]
backend-1    |       No voters found.
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 49.6286ms - 404
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 49.8641ms - 404
frontend-1   | fail: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Internal server error - GetVotersByElectionId
frontend-1   |       Front.Utilities.Errors.InternalServerErrorException: Internal server error - GetVotersByElectionId
frontend-1   | fail: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Error getting voters by election id
frontend-1   |       Front.Utilities.Errors.InternalServerErrorException: Internal server error - GetVotersByElectionId
frontend-1   |          at Front.Services.ApiService.VotersApiService.GetVotersByElectionId(Guid electionId) in /src/Front/Services/ApiService/VotersApiService.cs:line 89
frontend-1   | info: Front.Services.ApiService.ElectionsApiService[0]
frontend-1   |       Getting Elections
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/Election
backend-1    | info: Backend.Controllers.DataControllers.ElectionController[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
backend-1    |       Getting Elections with status: all
frontend-1   |       Sending HTTP request GET http://backend:8080/api/Election
frontend-1   | info: Front.Services.ApiService.ElectionsApiService[0]
frontend-1   |       Getting Elections
backend-1    | info: Backend.Services.DataServices.ElectionService[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
backend-1    |       Getting all elections
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/Election?status=Open
backend-1    | info: Backend.Controllers.DataControllers.ElectionController[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
backend-1    |       Getting Elections with status: Open
frontend-1   |       Sending HTTP request GET http://backend:8080/api/Election?status=Open
backend-1    | info: Backend.Services.DataServices.ElectionService[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
backend-1    |       Getting open elections
frontend-1   |       Received HTTP response headers after 14.522ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 14.8383ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 16.5348ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 16.7046ms - 200
frontend-1   | info: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Getting voters by election id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/voters/election/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
backend-1    | info: Backend.Controllers.DataControllers.VotersController[0]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/voters/election/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    |       Fetching all voters from election with id 68095015-d9f8-41f7-9de7-1baef771f135.
backend-1    | info: Backend.Services.DataServices.VoterService[0]
backend-1    |       Getting voters for election with id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | warn: Backend.Controllers.DataControllers.VotersController[0]
backend-1    |       No voters found.
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 13.9416ms - 404
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 14.1725ms - 404
frontend-1   | fail: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Internal server error - GetVotersByElectionId
frontend-1   |       Front.Utilities.Errors.InternalServerErrorException: Internal server error - GetVotersByElectionId
frontend-1   | fail: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Error getting voters by election id
frontend-1   |       Front.Utilities.Errors.InternalServerErrorException: Internal server error - GetVotersByElectionId
frontend-1   |          at Front.Services.ApiService.VotersApiService.GetVotersByElectionId(Guid electionId) in /src/Front/Services/ApiService/VotersApiService.cs:line 89

## Adding a Voter
frontend-1   | info: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Creating voter by id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request POST http://backend:8080/api/voters
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/api/voters
backend-1    | info: Backend.Controllers.DataControllers.VotersController[0]
backend-1    |       Creating voter with electionId 68095015-d9f8-41f7-9de7-1baef771f135.
backend-1    | info: Backend.Services.DataServices.VoterService[0]
backend-1    |       Creating new voter
backend-1    | info: Backend.Repositories.VotersRepository[0]
backend-1    |       Creating voter for election: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Controllers.DataControllers.VotersController[0]
backend-1    |       Voter created successfully with ID f9c9ffc7-e332-4b49-80b6-1d5e6509b732.
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 40.7144ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 40.9357ms - 200
frontend-1   | info: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Getting voters by election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Controllers.DataControllers.VotersController[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
backend-1    |       Fetching all voters from election with id 68095015-d9f8-41f7-9de7-1baef771f135.
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/voters/election/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.DataServices.VoterService[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
backend-1    |       Getting voters for election with id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   |       Sending HTTP request GET http://backend:8080/api/voters/election/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 8.3239ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 8.5302ms - 200

## Adding a Project
frontend-1   | info: Front.Services.ApiService.Elections.ProjectsApiService[0]
frontend-1   |       Create Project with name: P1
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request POST http://backend:8080/api/Project/
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/api/Project/
backend-1    | info: Backend.Controllers.DataControllers.ProjectController[0]
backend-1    |       Creating project with name P1
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Creating Project
backend-1    | info: Backend.Controllers.DataControllers.ProjectController[0]
backend-1    |       Project created successfully with name P1
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 31.66ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 31.8088ms - 200
frontend-1   | info: Front.Services.ApiService.Elections.ProjectsApiService[0]
frontend-1   |       Created Project with name: P1successfully
frontend-1   | info: Front.Services.ApiService.Elections.ProjectsApiService[0]
frontend-1   |       Get Projects from election with Id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/Project/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/Project/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Controllers.DataControllers.ProjectController[0]
backend-1    |       Getting projects for election 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 20.832ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 20.9996ms - 200

## A Voter Voting (both loading and submitting):
frontend-1   | info: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Getting voter by id f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/voters/f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/voters/f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Controllers.DataControllers.VotersController[0]
backend-1    |       Fetching voter with ID f9c9ffc7-e332-4b49-80b6-1d5e6509b732.
backend-1    | info: Backend.Services.DataServices.VoterService[0]
backend-1    |       Getting voter with id f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Repositories.VotersRepository[0]
backend-1    |       Getting Voter with id:f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Repositories.VotersRepository[0]
backend-1    |       Found a result with id:f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 15.6957ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 16.0052ms - 200
frontend-1   | info: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Getting voter by id f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/voters/f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/voters/f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Controllers.DataControllers.VotersController[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
backend-1    |       Fetching voter with ID f9c9ffc7-e332-4b49-80b6-1d5e6509b732.
frontend-1   |       Received HTTP response headers after 3.7603ms - 200
backend-1    | info: Backend.Services.DataServices.VoterService[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
backend-1    |       Getting voter with id f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   |       End processing HTTP request after 3.9256ms - 200
backend-1    | info: Backend.Repositories.VotersRepository[0]
frontend-1   | info: Front.Services.ApiService.Elections.ProjectsApiService[0]
frontend-1   |       Get Projects from election with Id: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    |       Getting Voter with id:f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Repositories.VotersRepository[0]
backend-1    |       Found a result with id:f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
backend-1    | info: Backend.Controllers.DataControllers.ProjectController[0]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/Project/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    |       Getting projects for election 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/Project/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 10.0685ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 10.2476ms - 200
frontend-1   | info: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Getting voter by id f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/voters/f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/voters/f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Controllers.DataControllers.VotersController[0]
backend-1    |       Fetching voter with ID f9c9ffc7-e332-4b49-80b6-1d5e6509b732.
backend-1    | info: Backend.Services.DataServices.VoterService[0]
backend-1    |       Getting voter with id f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Repositories.VotersRepository[0]
backend-1    |       Getting Voter with id:f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Repositories.VotersRepository[0]
backend-1    |       Found a result with id:f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 13.3962ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 13.5596ms - 200
frontend-1   | info: Front.Services.ApiService.Elections.ProjectsApiService[0]
frontend-1   |       Get Projects from election with Id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/Project/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/Project/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Controllers.DataControllers.ProjectController[0]
backend-1    |       Getting projects for election 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 10.1707ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 10.4694ms - 200
frontend-1   | info: Front.Services.ApiService.ScoresApiService[0]
frontend-1   |       UpdateScores for voter: f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request POST http://backend:8080/api/scores/f9c9ffc7-e332-4b49-80b6-1d5e6509b732
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/api/scores/f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Controllers.DataControllers.ScoresController[0]
backend-1    |       Updating Scores for voter with id: f9c9ffc7-e332-4b49-80b6-1d5e6509b732)
backend-1    | info: Backend.Services.DataServices.ScoresService[0]
backend-1    |       Getting scores for voter with id f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Repositories.VotersRepository[0]
backend-1    |       Getting Voter with id:f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Repositories.VotersRepository[0]
backend-1    |       Found a result with id:f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Services.DataServices.ScoresService[0]
backend-1    |       Create Score
backend-1    | info: Backend.Repositories.VotersRepository[0]
backend-1    |       Getting Voter with id:f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Repositories.VotersRepository[0]
backend-1    |       Found a result with id:f9c9ffc7-e332-4b49-80b6-1d5e6509b732
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 61.2927ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 61.4376ms - 200

# Ending Election
frontend-1   | info: Front.Services.ApiService.PbEngineApiService[0]
frontend-1   |       Calculating election with id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/pbengine/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/pbengine/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Controllers.PbEngineController[0]
backend-1    |       Calculating election with id:68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.DataServices.ElectionService[0]
backend-1    |       Getting election with id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.DataServices.ElectionService[0]
backend-1    |       Ending election with id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.DataServices.VoterService[0]
backend-1    |       Getting voters for election with id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.ApiServices.PbEngine.PbEngineService[0]
backend-1    |       Calculating election
backend-1    | info: System.Net.Http.HttpClient.PbEngine.LogicalHandler[100]
backend-1    |       Start processing HTTP request POST http://pbengine:8000/getResult/?method=1&ballot_type=1
backend-1    | info: System.Net.Http.HttpClient.PbEngine.ClientHandler[100]
backend-1    |       Sending HTTP request POST http://pbengine:8000/getResult/?method=1&ballot_type=1
pbengine-1   | INFO:     172.18.0.4:56140 - "POST /getResult/?method=1&ballot_type=1 HTTP/1.1" 200 OK
backend-1    | info: System.Net.Http.HttpClient.PbEngine.ClientHandler[101]
backend-1    |       Received HTTP response headers after 22.3062ms - 200
backend-1    | info: System.Net.Http.HttpClient.PbEngine.LogicalHandler[101]
backend-1    |       End processing HTTP request after 31.6072ms - 200
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
backend-1    |       Adding new election result with ElectionId: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Adding Election Result to database with Id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 84.8092ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 84.8987ms - 200
frontend-1   | info: Front.Services.ApiService.ElectionsApiService[0]
frontend-1   |       Getting Election Called with id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/Election/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/Election/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Controllers.DataControllers.ElectionController[0]
backend-1    |       Getting Election by id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.DataServices.ElectionService[0]
backend-1    |       Getting election with id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 12.265ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 12.4552ms - 200
frontend-1   | info: Front.Services.ApiService.ElectionResultsApiService[0]
frontend-1   |       Fetching list of election results for election with ID: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/ElectionResult/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/ElectionResult/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Controllers.DataControllers.ElectionResultController[0]
backend-1    |       Get results for electionId 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
backend-1    |       Getting election results
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting ElectionResults from database with election Id: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting Elected Projects from database with Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 31.6335ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 31.7485ms - 200
frontend-1   | info: Front.Services.ApiService.ElectionResultsApiService[0]
frontend-1   |       Got 1 election results
frontend-1   | info: Front.Services.ApiService.ElectionResultsApiService[0]
frontend-1   |       Id:9c58eb20-0237-436c-84ba-44104354e522
frontend-1   |        ElectionId: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   |        SubmittedProjects: 1
frontend-1   |        ElectedProjects: 1
frontend-1   |        UsedMethod: mes
frontend-1   |        UsedBallot: 1
frontend-1   |        TotalBudget: 120
frontend-1   | info: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Getting voters by election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Controllers.DataControllers.VotersController[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
backend-1    |       Fetching all voters from election with id 68095015-d9f8-41f7-9de7-1baef771f135.
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/voters/election/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.DataServices.VoterService[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
backend-1    |       Getting voters for election with id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   |       Sending HTTP request GET http://backend:8080/api/voters/election/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 5.181ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 5.2888ms - 200
frontend-1   | info: Front.Services.ApiService.PbEngineApiService[0]
frontend-1   |       Getting Avg Satisfaction
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request POST http://backend:8080/analyze/avgSatisfaction/9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/analyze/avgSatisfaction/9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: Front.Services.ApiService.PbEngineApiService[0]
frontend-1   |       Getting Avg Satisfaction - For Coherent Groups
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request POST http://backend:8080/api/pbengine/analyze/CoherentGroups/9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/api/pbengine/analyze/CoherentGroups/9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: Front.Services.ApiService.PbEngineApiService[0]
frontend-1   |       Getting Avg Satisfaction
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request POST http://backend:8080/analyze/avgSatisfaction/9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/analyze/avgSatisfaction/9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: Front.Services.ApiService.PbEngineApiService[0]
frontend-1   |       Getting Avg Satisfaction - For Coherent Groups
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request POST http://backend:8080/api/pbengine/analyze/CoherentGroups/9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Controllers.PbEngineController[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
backend-1    |       Getting avg satisfaction
frontend-1   |       Sending HTTP request POST http://backend:8080/api/pbengine/analyze/CoherentGroups/9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
backend-1    |       Getting election result with id 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting ElectionResults from database with Result Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Controllers.PbEngineController[0]
backend-1    |       Getting avg satisfaction
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
backend-1    |       Getting election result with id 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting ElectionResults from database with Result Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting Elected Projects from database with Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting Elected Projects from database with Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
backend-1    | info: Backend.Controllers.PbEngineController[0]
backend-1    |       Getting groups avg sat
backend-1    | info: Backend.Controllers.PbEngineController[0]
backend-1    |       Getting groups avg sat
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
backend-1    |       Getting election result with id 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
backend-1    |       Getting election result with id 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting ElectionResults from database with Result Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting ElectionResults from database with Result Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.DataServices.VoterService[0]
backend-1    |       Getting voters for election with id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting Elected Projects from database with Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting Elected Projects from database with Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.DataServices.VoterService[0]
backend-1    |       Getting voters for election with id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.ApiServices.PbEngine.PbEngineService[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 51.5942ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
backend-1    |       Getting analysis numbers
frontend-1   |       End processing HTTP request after 51.8952ms - 200
backend-1    | info: Backend.Services.ApiServices.PbEngine.PbEngineService[0]
backend-1    |       Getting analysis numbers
backend-1    | info: System.Net.Http.HttpClient.PbEngine.LogicalHandler[100]
backend-1    |       Start processing HTTP request POST http://pbengine:8000/analyze/avgSatisfaction
backend-1    | info: System.Net.Http.HttpClient.PbEngine.LogicalHandler[100]
backend-1    |       Start processing HTTP request POST http://pbengine:8000/analyze/avgSatisfaction
backend-1    | info: System.Net.Http.HttpClient.PbEngine.ClientHandler[100]
backend-1    |       Sending HTTP request POST http://pbengine:8000/analyze/avgSatisfaction
backend-1    | info: System.Net.Http.HttpClient.PbEngine.ClientHandler[100]
backend-1    |       Sending HTTP request POST http://pbengine:8000/analyze/avgSatisfaction
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 60.0751ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 60.1395ms - 200
pbengine-1   | INFO:     172.18.0.4:56140 - "POST /analyze/avgSatisfaction HTTP/1.1" 200 OK
backend-1    | info: System.Net.Http.HttpClient.PbEngine.ClientHandler[101]
backend-1    |       Received HTTP response headers after 795.3593ms - 200
backend-1    | info: System.Net.Http.HttpClient.PbEngine.LogicalHandler[101]
backend-1    |       End processing HTTP request after 795.5515ms - 200
pbengine-1   | INFO:     172.18.0.4:56148 - "POST /analyze/avgSatisfaction HTTP/1.1" 200 OK
backend-1    | info: System.Net.Http.HttpClient.PbEngine.ClientHandler[101]
backend-1    |       Received HTTP response headers after 797.3307ms - 200
backend-1    | info: System.Net.Http.HttpClient.PbEngine.LogicalHandler[101]
backend-1    |       End processing HTTP request after 797.4527ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 860.7905ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 868.2553ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 868.3685ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 860.8981ms - 200
frontend-1   | info: Front.Services.ApiService.ElectionsApiService[0]
backend-1    | info: Backend.Controllers.DataControllers.ElectionController[0]
frontend-1   |       Getting Election Called with id: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    |       Getting Election by id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/Election/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/Election/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
backend-1    | info: Backend.Services.DataServices.ElectionService[0]
frontend-1   |       Received HTTP response headers after 5.0622ms - 200
backend-1    |       Getting election with id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
backend-1    | info: Backend.Controllers.DataControllers.ElectionResultController[0]
backend-1    |       Get results for electionId 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
backend-1    |       Getting election results
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
frontend-1   |       End processing HTTP request after 5.3481ms - 200
backend-1    |       Getting ElectionResults from database with election Id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: Front.Services.ApiService.ElectionResultsApiService[0]
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
frontend-1   |       Fetching list of election results for election with ID: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    |       Getting Elected Projects from database with Id: 9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
backend-1    | info: Backend.Repositories.ProjectRepository[0]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/ElectionResult/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/ElectionResult/68095015-d9f8-41f7-9de7-1baef771f135
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 21.6539ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 21.9263ms - 200
frontend-1   | info: Front.Services.ApiService.ElectionResultsApiService[0]
frontend-1   |       Got 1 election results
frontend-1   | info: Front.Services.ApiService.ElectionResultsApiService[0]
frontend-1   |       Id:9c58eb20-0237-436c-84ba-44104354e522
frontend-1   |        ElectionId: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   |        SubmittedProjects: 1
backend-1    | info: Backend.Controllers.DataControllers.VotersController[0]
frontend-1   |        ElectedProjects: 1
backend-1    |       Fetching all voters from election with id 68095015-d9f8-41f7-9de7-1baef771f135.
frontend-1   |        UsedMethod: mes
backend-1    | info: Backend.Services.DataServices.VoterService[0]
frontend-1   |        UsedBallot: 1
backend-1    |       Getting voters for election with id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   |        TotalBudget: 120
frontend-1   | info: Front.Services.ApiService.VotersApiService[0]
frontend-1   |       Getting voters by election id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request GET http://backend:8080/api/voters/election/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request GET http://backend:8080/api/voters/election/68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 12.7418ms - 200
backend-1    | info: Backend.Controllers.PbEngineController[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
backend-1    |       Getting avg satisfaction
frontend-1   |       End processing HTTP request after 12.9832ms - 200
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
frontend-1   | info: Front.Services.ApiService.PbEngineApiService[0]
backend-1    |       Getting election result with id 9c58eb20-0237-436c-84ba-44104354e522
frontend-1   |       Getting Avg Satisfaction
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
backend-1    |       Getting ElectionResults from database with Result Id: 9c58eb20-0237-436c-84ba-44104354e522
frontend-1   |       Start processing HTTP request POST http://backend:8080/analyze/avgSatisfaction/9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/analyze/avgSatisfaction/9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: Front.Services.ApiService.PbEngineApiService[0]
frontend-1   |       Getting Avg Satisfaction - For Coherent Groups
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
frontend-1   |       Start processing HTTP request POST http://backend:8080/api/pbengine/analyze/CoherentGroups/9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/api/pbengine/analyze/CoherentGroups/9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Controllers.PbEngineController[0]
frontend-1   | info: Front.Services.ApiService.PbEngineApiService[0]
backend-1    |       Getting groups avg sat
frontend-1   |       Getting Avg Satisfaction
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
backend-1    |       Getting election result with id 9c58eb20-0237-436c-84ba-44104354e522
frontend-1   |       Start processing HTTP request POST http://backend:8080/analyze/avgSatisfaction/9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/analyze/avgSatisfaction/9c58eb20-0237-436c-84ba-44104354e522
frontend-1   | info: Front.Services.ApiService.PbEngineApiService[0]
backend-1    |       Getting ElectionResults from database with Result Id: 9c58eb20-0237-436c-84ba-44104354e522
frontend-1   |       Getting Avg Satisfaction - For Coherent Groups
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[100]
backend-1    | info: Backend.Controllers.PbEngineController[0]
frontend-1   |       Start processing HTTP request POST http://backend:8080/api/pbengine/analyze/CoherentGroups/9c58eb20-0237-436c-84ba-44104354e522
backend-1    |       Getting avg satisfaction
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[100]
frontend-1   |       Sending HTTP request POST http://backend:8080/api/pbengine/analyze/CoherentGroups/9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
backend-1    |       Getting election result with id 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting ElectionResults from database with Result Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting Elected Projects from database with Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting Elected Projects from database with Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting Elected Projects from database with Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Controllers.PbEngineController[0]
backend-1    |       Getting groups avg sat
backend-1    | info: Backend.Services.DataServices.ElectionResultService[0]
backend-1    |       Getting election result with id 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting ElectionResults from database with Result Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
backend-1    | info: Backend.Repositories.ElectionResultRepository[0]
backend-1    |       Getting Elected Projects from database with Id: 9c58eb20-0237-436c-84ba-44104354e522
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
pbengine-1   | INFO:     172.18.0.4:56148 - "POST /analyze/avgSatisfaction HTTP/1.1" 200 OK
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   |       Received HTTP response headers after 38.6625ms - 200
backend-1    | info: Backend.Services.DataServices.VoterService[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
backend-1    |       Getting voters for election with id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   |       End processing HTTP request after 39.0902ms - 200
backend-1    | info: Backend.Services.DataServices.VoterService[0]
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
backend-1    |       Getting voters for election with id 68095015-d9f8-41f7-9de7-1baef771f135
frontend-1   |       Received HTTP response headers after 45.6933ms - 200
backend-1    | info: Backend.Services.ApiServices.PbEngine.PbEngineService[0]
backend-1    |       Getting analysis numbers
backend-1    | info: System.Net.Http.HttpClient.PbEngine.LogicalHandler[100]
backend-1    |       Start processing HTTP request POST http://pbengine:8000/analyze/avgSatisfaction
backend-1    | info: System.Net.Http.HttpClient.PbEngine.ClientHandler[100]
pbengine-1   | INFO:     172.18.0.4:56140 - "POST /analyze/avgSatisfaction HTTP/1.1" 200 OK
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
backend-1    |       Sending HTTP request POST http://pbengine:8000/analyze/avgSatisfaction
frontend-1   |       End processing HTTP request after 45.9235ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 51.8912ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
frontend-1   |       End processing HTTP request after 52.0078ms - 200
backend-1    | info: Backend.Services.ApiServices.PbEngine.PbEngineService[0]
backend-1    |       Getting analysis numbers
backend-1    | info: System.Net.Http.HttpClient.PbEngine.LogicalHandler[100]
backend-1    |       Start processing HTTP request POST http://pbengine:8000/analyze/avgSatisfaction
backend-1    | info: System.Net.Http.HttpClient.PbEngine.ClientHandler[100]
backend-1    |       Sending HTTP request POST http://pbengine:8000/analyze/avgSatisfaction
backend-1    | info: System.Net.Http.HttpClient.PbEngine.ClientHandler[101]
backend-1    |       Received HTTP response headers after 8.7929ms - 200
backend-1    | info: System.Net.Http.HttpClient.PbEngine.LogicalHandler[101]
backend-1    |       End processing HTTP request after 8.9123ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.ClientHandler[101]
frontend-1   |       Received HTTP response headers after 62.8925ms - 200
frontend-1   | info: System.Net.Http.HttpClient.Backend.LogicalHandler[101]
backend-1    | info: System.Net.Http.HttpClient.PbEngine.ClientHandler[101]
frontend-1   |       End processing HTTP request after 62.9812ms - 200
backend-1    |       Received HTTP response headers after 12.467ms - 200
backend-1    | info: System.Net.Http.HttpClient.PbEngine.LogicalHandler[101]
backend-1    |       End processing HTTP request after 12.6509ms - 200
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting Project with projectId: d6456ff5-4464-4093-af8c-06e9af41059b
backend-1    | info: Backend.Services.Interfaces.IProjectService[0]
backend-1    |       Getting project with Election id 68095015-d9f8-41f7-9de7-1baef771f135
backend-1    | info: Backend.Repositories.ProjectRepository[0]
backend-1    |       Getting projects from database for election with id: 68095015-d9f8-41f7-9de7-1baef771f135