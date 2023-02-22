# dotnet-rpg-battle

dotnet-rpg-battle is the backend of RPG-Battle, a game simulator inspired by Dungeons & Dragons

[Frontend](https://github.com/zizzhangJohn/dotnet-battle-client) is built with ReactTS.

[Deployment](https://cozy-cucurucho-9cc339.netlify.app): https://cozy-cucurucho-9cc339.netlify.app
## Installation
The backend is built with .net 6 web api, MSSQL with entity framework, using **google jwt** for authentication.

For details on how to obtain **google jwt**, check [here](https://developers.google.com/identity/gsi/web/guides/overview), don't read the deprecated one.

To run it, you must first replace the `"MSSQLConnection"` string in `appsettings.json` to match you database, add your frontend domain to `AddCors` policy part in `Program.cs`

After setting `"MSSQLConnection"`, you must run ef database update to create the tables
```bash
dotnet ef database update
```
## Future improvement
Adding a system to blacklist unexpired jwt upon user signout