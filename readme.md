## Summary

WinWar is a multiplatform (Windows, WindowsStore, OSX, Linux, iOS) port of the original DOS WarCraft: Orcs & Humans PC Game. It is completely rewritten from scratch using only the original art and level data.

It *requires* the original game; no game data is included. Since version 0.2.0 WinWar is compatible with the demo version, which you can find here:
http://wowpedia.org/Warcraft:_Orcs_%26_Humans_%28Demo%29 

## Supported platforms
- Windows
- WindowsStore
- OSX
- Linux
- iOS
- Android (planned)

## Screenshots
![Intro](/../screenshots/Screenshots/Intro.png?raw=true "Intro")
![Orc1](/../screenshots/Screenshots/Orc1.png?raw=true "Orc1")
![iPad](/../screenshots/Screenshots/iPad.jpg?raw=true "iPad")

## Compiling and Running
### Compiling from source
- Check out the source code from the git repo.
- Open the solution (.sln file) for your platform using either Xamarin Studio (*nix or iOS) or Visual Studio 2015.
- Compile the code.

### Running
- Before running the game you must copy over the data files from a Warcraft 1 retail or demo version. To do that, copy everything from the DATA directory of Warcraft 1 (contains 40 .WAR files in retail, but only one .WAR file in the demo version) to a subdirectory named "Data" below the "Assets" directory in the output folder containing the binary (usually bin/Debug, but that differs depending on platform).
- Run the WinWarCS executable either from your IDE or from disk.
- Enjoy

## Brief history
- Started project in 2003
- Originally written in C++
- Switched to C# sometime in 2008
- Later moved to MonoGame as base framework to support multiple platforms
- Note: Due to the age of the project, parts of the codebase are written in a way which I'm not particularly proud of. They will be refactored over time.

## Acknowledgements
WarCraft: Orcs & Humans is a trademark of Blizzard Entertainment.