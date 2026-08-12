# Medias Delight
 
A web app to help users better understand their media preferences using AI. Users can login and manage a list of media items that will get analyzed.

## Flow
- logs in or registers
- Go to media Items dashboard (can manage media Items from here)
- add media item (including information about them)
- Visit one of the AI analysis pages
- Enter info (slightly different per AI page) and submit

## AI anaysis pages
There are 3 core AI pages that each use media items by the User to produce a response.
1) Analyze Me: A general analysis about the users preference
2) Assess work: User provides the name of a work and Ai will judge if they will like it based on a rating out of 10, and a description on why
3) Recommend Works: A recommendation of 3-5 works

## Tools
- ASP.NET CORE MVC
- ASP.NET CORE Identity
- C#
- Micosoft SQL Server
- Gemini API

## How to run
1) Create ".env" file with db connection string and Gemini api key. There is an example file called "ExampleEnv.txt".
2) make sure you have .NET runtime and SQL Server installed
3) Run with commands `dotnet restore` and then `dotnet run`
