SET solution_dir=%1
SET out_dir=%2
SET plugins_dir=%solution_dir%Build\GameData\DiscordRP\Plugins

MKDIR %plugins_dir%
COPY "%out_dir%\DiscordRP.dll" "%plugins_dir%"
COPY "%out_dir%\Libraries\discord-rpc-32.lib" "%plugins_dir%"
COPY "%out_dir%\Libraries\discord-rpc-64.lib" "%plugins_dir%"
