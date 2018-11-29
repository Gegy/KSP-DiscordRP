using System;
using UnityEngine;

namespace DiscordRP.States
{
    class EscapingState : PresenceState
    {
        private readonly CelestialBody body;
        private readonly long startTimestamp;
        private readonly bool paused;

        public EscapingState(CelestialBody body, long startTimestamp, bool paused)
        {
            this.body = body;
            this.startTimestamp = startTimestamp;
            this.paused = paused;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is EscapingState)
            {
                EscapingState escapingState = (EscapingState) obj;

                return escapingState.body.Equals(body) && escapingState.startTimestamp == startTimestamp && escapingState.paused == paused;
            }

            return false;
        }

        public DiscordRpc.RichPresence create()
        {
            string state = state = string.Format("Escaping {0}", body.name ?? body.displayName);

            return new DiscordRpc.RichPresence()
            {
                state = state,
                details = "At escape velocity",
                largeImageKey = string.Format("body_{0}", body.displayName.ToLower() ?? body.name.ToLower()),
                largeImageText = body.displayName ?? body.name,
                startTimestamp = startTimestamp,
                smallImageKey = Utils.GetSmallFlightIcon(paused),
                smallImageText = Utils.GetSmallFlightIconDetails(paused),
            };
        }
    }
}
