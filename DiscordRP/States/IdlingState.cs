using System;

namespace DiscordRP.States
{
    class IdlingState : PresenceState
    {
        private readonly long startTimestamp;
        private readonly GameScenes scene;

        public IdlingState(long startTimestamp, GameScenes scene)
        {
            this.startTimestamp = startTimestamp;
            this.scene = scene;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is IdlingState)
            {
                IdlingState idleState = (IdlingState) obj;

                return idleState.startTimestamp == startTimestamp && idleState.scene == scene;
            }

            return false;
        }

        public DiscordRpc.RichPresence create()
        {
            return new DiscordRpc.RichPresence()
            {
                state = "Idle",
                details = GetSceneDescription(),
                largeImageKey = "default",
                largeImageText = "Idling",
                startTimestamp = startTimestamp,
            };
        }

        private String GetSceneDescription()
        {
            switch (scene)
            {
                case GameScenes.LOADING:
                case GameScenes.LOADINGBUFFER:
                    return "Loading Game";
                case GameScenes.MAINMENU:
                    return "In the Main Menu";
                case GameScenes.SETTINGS:
                    return "Configuring their game";
                case GameScenes.SPACECENTER:
                    return "At the KSC";
                case GameScenes.TRACKSTATION:
                    return "In the Tracking Station";
                case GameScenes.CREDITS:
                    return "Watching the Credits";
            }

            return scene.ToString();
        }
    }
}
