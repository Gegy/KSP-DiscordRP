SET solution_dir=%1
SET out_dir=%2
SET plugins_dir=%solution_dir%Build\GameData\DiscordRP\Plugins

MKDIR %plugins_dir%
MKDIR %plugins_dir%\linux
MKDIR %plugins_dir%\osx
MKDIR %plugins_dir%\win32
MKDIR %plugins_dir%\win64
COPY "%out_dir%\DiscordRP.dll" "%plugins_dir%"
COPY "%out_dir%\Libraries\linux\discord-rpc.bin" "%plugins_dir%\linux"
COPY "%out_dir%\Libraries\osx\discord-rpc.bin" "%plugins_dir%\osx"
COPY "%out_dir%\Libraries\win32\discord-rpc.bin" "%plugins_dir%\win32"
COPY "%out_dir%\Libraries\win64\discord-rpc.bin" "%plugins_dir%\win64"
