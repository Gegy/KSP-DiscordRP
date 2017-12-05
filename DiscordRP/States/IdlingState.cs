using System;

namespace DiscordRP.States
{
    class IdlingState : PresenceState
    {
        private readonly long startTimestamp;

        public IdlingState(long startTimestamp)
        {
            this.startTimestamp = startTimestamp;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is IdlingState)
            {
                IdlingState idleState = (IdlingState) obj;

                return idleState.startTimestamp == startTimestamp;
            }

            return false;
        }

        public DiscordRpc.RichPresence create()
        {
            return new DiscordRpc.RichPresence()
            {
                state = "Idle",
                details = "",
                largeImageKey = "default",
                largeImageText = "Idling",
                startTimestamp = startTimestamp,
            };
        }
    }
}
