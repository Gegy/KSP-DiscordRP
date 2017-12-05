using System;
using UnityEngine;

namespace DiscordRP.States
{
    class EscapingState : PresenceState
    {
        private readonly CelestialBody body;
        private readonly long startTimestamp;

        public EscapingState(CelestialBody body, long startTimestamp)
        {
            this.body = body;
            this.startTimestamp = startTimestamp;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is EscapingState)
            {
                EscapingState escapingState = (EscapingState) obj;

                return escapingState.body.Equals(body) && escapingState.startTimestamp == startTimestamp;
            }

            return false;
        }

        public DiscordRpc.RichPresence create()
        {
            string state = state = string.Format("Escaping {0}", body.name);

            return new DiscordRpc.RichPresence()
            {
                state = state,
                details = "At escape velocity",
                largeImageKey = string.Format("body_{0}", body.name.ToLower()),
                largeImageText = body.name,
                startTimestamp = startTimestamp,
                smallImageKey = "default",
                smallImageText = "Kerbal Space Program",
            };
        }
    }
}
