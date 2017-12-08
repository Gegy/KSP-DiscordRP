using System;
using UnityEngine;

namespace DiscordRP.States
{
    class SplashedState : PresenceState
    {
        private readonly CelestialBody body;
        private readonly double latitude;
        private readonly double longitude;
        private readonly long startTimestamp;

        public SplashedState(CelestialBody body, double latitude, double longitude, long startTimestamp)
        {
            this.body = body;
            this.latitude = latitude;
            this.longitude = longitude;
            this.startTimestamp = startTimestamp;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is SplashedState)
            {
                SplashedState splashedState = (SplashedState) obj;

                return splashedState.body.Equals(body) && splashedState.latitude == latitude && splashedState.longitude == longitude && splashedState.startTimestamp == startTimestamp;
            }

            return false;
        }

        public DiscordRpc.RichPresence create()
        {
            string state = state = string.Format("Splashed down at {0}", body.name);

            return new DiscordRpc.RichPresence()
            {
                state = state,
                details = string.Format("Lat: {0:F3} | Lng: {1:F3}", latitude, longitude),
                largeImageKey = string.Format("body_{0}", body.name.ToLower()),
                largeImageText = body.name,
                startTimestamp = startTimestamp,
                smallImageKey = "default",
                smallImageText = "Kerbal Space Program",
            };
        }
    }
}
