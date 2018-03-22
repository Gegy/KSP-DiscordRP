using System;
using UnityEngine;

namespace DiscordRP.States
{
    class FlyingState : PresenceState
    {
        private readonly CelestialBody body;
        private readonly double altitude;
        private readonly double velocity;
        private readonly long startTimestamp;
        private readonly bool paused;

        public FlyingState(CelestialBody body, double altitude, double velocity, long startTimestamp, bool paused)
        {
            this.body = body;
            this.altitude = altitude;
            this.velocity = velocity;
            this.startTimestamp = startTimestamp;
            this.paused = paused;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is FlyingState)
            {
                FlyingState flyingState = (FlyingState) obj;

                return flyingState.body.Equals(body) && flyingState.altitude == altitude && flyingState.velocity == velocity && flyingState.startTimestamp == startTimestamp && flyingState.paused == paused;
            }

            return false;
        }

        public DiscordRpc.RichPresence create()
        {
            string state = state = string.Format("Flying over {0}", body.displayName);

            return new DiscordRpc.RichPresence()
            {
                state = state,
                details = string.Format("Alt: {0:F0}m | Vel: {1:F0}m/s", altitude, velocity),
                largeImageKey = string.Format("body_{0}", body.displayName.ToLower()),
                largeImageText = body.name,
                startTimestamp = startTimestamp,
                smallImageKey = Utils.GetSmallFlightIcon(paused),
                smallImageText = Utils.GetSmallFlightIconDetails(paused),
            };
        }
    }
}
