CsGo To Discord Activity
============

Reports current game mode, map, and score as discord activity (rich presence)

It makes use of [CSGO Game State Integration](https://developer.valvesoftware.com/wiki/Counter-Strike:_Global_Offensive_Game_State_Integration) .

Usage
=========

Just run the .exe, no install/dependencies needed.

Check that game activity toggle is enabled in discord settings.

Features
=========

- Adds config file so game state integration reports its status
- Watches game process (csgo.exe)
- Listens (http port 2182) for game state changes
- Talks to discord rpc on local machine and reports status
- Tray icon to close it / show simple UI with debug output

Dev
=========

Use Visual Studio 2019

If you want to change the assets you need to create a new app on discord side and change its id in the code.

Thanks
========

[BeepFelix/CSGO---Discord-RichPresence](https://github.com/BeepFelix/CSGO---Discord-RichPresence) for original idea and most assets
