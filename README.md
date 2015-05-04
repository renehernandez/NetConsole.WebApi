# NetConsole.WebApi

NetConsole.WebApi is a package that it provides a way to access commands by using a sort of RPC-like API interaction.

Though it is possible to use this package as standalone web api, it is designed to be used better as part of another Web API or MVC project.

#### Metadata
The API offers command metadata for each command that can be reached.
The metadata is about:
* Number of actions available for a given command.
* Command overview (inherited from the *command* definition).
* Command name (inherited from the *command* definition).
* For each action it provides the following information:
  * Action name
  * Return type
  * Parameter types

## Basic Usage
To register the API as part of another application, just do:
```csharp
NetConsole.WebApi.WebApiConfig.Register(config); // HttpConfiguration config
```
Usually in Global.asax's Application_Start method or in WebApiConfig's Register method.

#### API Routes

Once the API is running on a server the following urls will accessible from clients:
* *api/commands* `// Return an array of commands object with all their public properties`
* *api/commands/meta* `// Return an array of command metadata`
* *api/commands/{commandName}* `// Return a single command whose name is commandName`
* *api/commands/{commandName}/meta* `// Return command metadata for the command whose name is commandName`
* *api/commands/perform* `// Read the message body and perform the operation stated within.`

## Manual Installation
Install the latest version from [NuGet](https://www.nuget.org/packages/NetConsole.WebApi/).

## Contributing
In lieu of a formal style guide, please take care to follow the existing code style. Add unit tests for any new or changed functionality. Use FAKE and paket to handle automation and dependencies management.

## Release History
Checks [Release Notes](https://github.com/renehernandez/NetConsole.WebApi/blob/master/RELEASE_NOTES.md) to see the changes history.
