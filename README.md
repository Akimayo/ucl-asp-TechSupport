# TechSupport and TechSupport.Backend
Michal Ciesla @ Unicorn College, 2020
## Running
React 'frontend' (**TechSupport**) can be run using the following commands:
```ps
npm install --save
npm start
```
After the `npm` packages have been installed, you can omit the first line.

ASP .NET Core 'backend' (**TechSupport.Backend**) can be run using the following commands:
```ps
cd backend/
dotnet run
```
'Backend' needs to be running in order for 'frontend' to function. Also make sure that your browser will allow access to the 'backend' portion through HTTPS.

## URLs
- **TechSupport** can be found at [http://localhost:3000](http://localhost:3000/) after running `npm start`

- **TechSupport.Backend** can be found at [http://localhost:5000](http://localhost:5000/) after running `dotnet run`

You can access one from the other using the link at the top right of both pages.
