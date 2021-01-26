# VL-Minecraft

A cross-platform Minecraft launcher for Minecraft Vast-Light

### Copyright

Copyright &copy; 2021 TURX

This project is an open source software under GNU GPL v3 license with the following exception.

Properties of Minecraft Vast-Light (including its name, websites, icons, etc.) in this project or any modified part of affiliated projects are not under the license above and prohibited to use without permission.

### Libraries

- .NET 5.0
- BMCLAPI
- Bootstrap
- CmlLib.Core
- ElectronNET.API
- ICSharpCode.SharpZipLib
- jQuery
- jQuery Validation
- jQuery Validation Unobtrusive
- LZMA-SDK
- Newtonsoft.Json

### Building

Prior: install the Electron.NET CLI, namely ```electronize```

Working Directory: ```/VL-Launcher```

Command: ```electronize build /target <win|osx|linux>```

macOS binaries cannot be built on Windows

Try to add ```/PublishReadyToRun false``` parameter if a relevant error terminates the compilation process
