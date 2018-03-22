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
        private readonly bool paused;

        public SplashedState(CelestialBody body, double latitude, double longitude, long startTimestamp, bool paused)
        {
            this.body = body;
            this.latitude = latitude;
            this.longitude = longitude;
            this.startTimestamp = startTimestamp;
            this.paused = paused;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is SplashedState)
            {
                SplashedState splashedState = (SplashedState) obj;

                return splashedState.body.Equals(body) && splashedState.latitude == latitude && splashedState.longitude == longitude && splashedState.startTimestamp == startTimestamp && splashedState.paused == paused;
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
                largeImageKey = string.Format("body_{0}", body.displayName.ToLower()),
                largeImageText = body.displayName,
                startTimestamp = startTimestamp,
                smallImageKey = Utils.GetSmallFlightIcon(paused),
                smallImageText = Utils.GetSmallFlightIconDetails(paused),
            };
        }
    }
}
