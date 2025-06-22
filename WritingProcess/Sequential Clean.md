
# Creating Election:

User Clicks create new election.

Frontend - Election API service - creating Election
Backend - Election Controller - creating Election

Backend - Election Service - creating Election

Backend - Election Repository - requests to create election

Postgres Db - Creates election in database

%% Responds all the way  back to frontend.

%% Navigates to the elections page

Frontend - Election Api Service - Requests all elections

Backend - Election controller - Requests all elections from Service.

Backend - Election Service - Requests all elections from database.

Backend - Election Repository - Requests elections from database.

Database - retrieves all elections from database.

%% Responds back.

frontend - voterApiService - Fetches all voters for election.

Backend - VoterController - requests all voters for election.

Backend - VoterService - requests voters for election

Backend - Voter repository - requests voters from database.

Database - responds with voters (empty)

%% Responds back


## Adding a Voter
User clicks "Add Voter button"

Frontend - VoterApiService - requests to create voter for election

Backend - VoterController - requests to create voter for election

Backend - VoterService - requests to create voter for election

Backend - Voter Repository - requsts to create voter

Database - creates voter for election

%% Responds all the way back.

## Creating project to election

User - Click on create project button.

Frontend - ProjectApiService - requests to create project for election

Backend - ProjectController - requests to create project for election

Backend - ProjectService - requests to create project for election

Backend - Project Repository - requests to create project for election

Database - creates project for election

%% Responds back to frontend.

## A Voter Voting

User inserts joincode

Frontend - VoterApiService - request to retrieve voter with id.

Backend - Voters Controller - requests to retriever voter with Id.

Backend - Voter Service - requests to retrieve voter with ID.

Backend - Voter Repository - requests to retrieve voter with ID-+.

Database - Retrieves voter with id.

%%Responds back

Frontend - ProjectApiService - requests projects for election

Backend - ProjectController - requests projects for election

Backend - Project Service - requests projects for election

Backend - Project Repository - requests projects for election

Database - retrieves projects for election

%% Responds back.

User clicks "submit" 

Frontend - ScoreApiServer - Requests to update Scores for voter

Backend - ScoreController - requests to update scores for voter

Backend - ScoreService - Requests to update score for voter

Backend - Scores repository - Requests to update scores in database

database - updates scores in database

%% Responds all the way back.


# Ending Election

User clicks end election

Frontend - PbEngineService - requests to end election

Backend - PbEngineController - Prepares to end election

Backend - ElectionService - Requests election from database

Backend - Election repository - requests election from database

Database - fetches election from database

% Responds back to backend

Backend - Project Service - requests projects for election

Backend - Project Repository - requests projects for election

Database - retrieves projects for election

% Responds back to backend

Backend - PbEngineService - requests to get election calculated

PbEngine - calculates election and responds back to backend

Backend - ElectionResultService - requests to add electionresult to database

Backend - ElectionResultRepository - requests to add elctionresult to database

Database - Creates new electionresult

%% Responds back to frontend with electionresultid


## Loading the result page



%Frontend - navigates to result page

Frontend - ElectionApiService - requests electionresult from database

Backend - ElectionResultController - requests all electionresults with election id

Backend - ElectionResultService - requests all electionresults with electionid.

Backend - ElectionResultRepository - requests ElectionResults from database with election id

Database - returns electionresults

%% responds back to backend

Backend - ProjectRepository - fetch projects for election

Database - retrieves projects for election

% responds back with projects

Frontend - VoterApiService - Requests voters for election

Backend - VoterController - Requests voters for election

Backend - VoterService - requests voters for election

Backend - Voter Repository - Requests voters for election

Database - returns voters for election

% Returns voters all the way back to the frontend.

Frontend - PbEngineApiService - Requests avg satisfaction numbers

Backend - PbEngineController - Requests avg satsifaction numbers

Backend - ElectionResultService - requests ElectionResult

Backend - ElectionResultRepositorues - requests ElectionResult

Database - responds with electionResult
% responds back to backend

Backend - ProjectService - requests projects for elections

Backend - ProjectRepository - Requests Project for election

Database - Responds with projects

%responds back to backend

Backend - VoterService - Requests voters for election

Backend - VoterRepositories - requests voters for election

Database - fetches voters for election 

%responds back with voters.

Backend - PbEngineService - requests avg sat numbers

PbEngine - calculates and responds

% Responds all the way back to frontend.