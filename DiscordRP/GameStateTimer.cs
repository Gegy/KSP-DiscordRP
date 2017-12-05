using System;

namespace DiscordRP
{
    class GameStateTimer
    {
        private readonly Predicate<GameScenes> checkActive;

        public bool CurrentActive { get; private set; }
        public long Timestamp { get; private set; }

        public GameStateTimer(Predicate<GameScenes> checkActive)
        {
            this.checkActive = checkActive;
            this.Timestamp = Utils.GetEpochTime();
        }

        public void Update()
        {
            bool newActive = checkActive.Invoke(HighLogic.LoadedScene);

            if (newActive != CurrentActive)
            {
                CurrentActive = newActive;

                if (newActive)
                {
                    Timestamp = Utils.GetEpochTime();
                }
            }
        }
    }
}
