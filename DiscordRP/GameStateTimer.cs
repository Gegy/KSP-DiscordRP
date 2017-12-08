using System;

namespace DiscordRP
{
    class GameStateTimer
    {
        private readonly Predicate<GameScenes> preconditions;
        private readonly Predicate<GameScenes> checkActive;

        public bool CurrentActive { get; private set; }
        public long Timestamp { get; private set; }

        public GameStateTimer(Predicate<GameScenes> preconditions, Predicate<GameScenes> checkActive)
        {
            this.preconditions = preconditions;
            this.checkActive = checkActive;
            this.Timestamp = Utils.GetEpochTime();
        }

        public static GameStateTimer Inverse(GameStateTimer timer)
        {
            return new GameStateTimer(timer.preconditions, scene => !timer.checkActive.Invoke(scene));
        }

        public void Update()
        {
            bool newActive = preconditions.Invoke(HighLogic.LoadedScene) && checkActive.Invoke(HighLogic.LoadedScene);

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
