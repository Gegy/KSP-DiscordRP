using System;
using UnityEngine;

namespace DiscordRP.States
{
    class LandedState : PresenceState
    {
        private readonly CelestialBody body;
        private readonly double latitude;
        private readonly double longitude;
        private readonly long startTimestamp;
        private readonly bool paused;

        public LandedState(CelestialBody body, double latitude, double longitude, long startTimestamp, bool paused)
        {
            this.body = body;
            this.latitude = latitude;
            this.longitude = longitude;
            this.startTimestamp = startTimestamp;
            this.paused = paused;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is LandedState)
            {
                LandedState landedState = (LandedState) obj;

                return landedState.body.Equals(body) && landedState.latitude == latitude && landedState.longitude == longitude && landedState.startTimestamp == startTimestamp && landedState.paused == paused;
            }

            return false;
        }

        public DiscordRpc.RichPresence create()
        {
            string state = state = string.Format("Landed at {0}", body.name);

            return new DiscordRpc.RichPresence()
            {
                state = state,
                details = string.Format("Lat: {0:F3} | Lng: {1:F3}", latitude, longitude),
                largeImageKey = string.Format("body_{0}", body.name.ToLower()),
                largeImageText = body.name,
                startTimestamp = startTimestamp,
                smallImageKey = Utils.GetSmallFlightIcon(paused),
                smallImageText = Utils.GetSmallFlightIconDetails(paused),
            };
        }
    }
}
