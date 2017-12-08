using System;
using UnityEngine;

namespace DiscordRP.States
{
    class OrbitingState : PresenceState
    {
        private readonly CelestialBody body;
        private readonly double semiMajorAxis;
        private readonly double eccentricity;
        private readonly long startTimestamp;

        public OrbitingState(CelestialBody body, double semiMajorAxis, double eccentricity, long startTimestamp)
        {
            this.body = body;
            this.semiMajorAxis = semiMajorAxis;
            this.eccentricity = eccentricity;
            this.startTimestamp = startTimestamp;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is OrbitingState)
            {
                OrbitingState orbitingState = (OrbitingState) obj;

                return orbitingState.body.Equals(body) && orbitingState.semiMajorAxis == semiMajorAxis && orbitingState.eccentricity == eccentricity && orbitingState.startTimestamp == startTimestamp;
            }

            return false;
        }

        public DiscordRpc.RichPresence create()
        {
            string state = state = string.Format("Orbiting around {0}", body.name);

            return new DiscordRpc.RichPresence()
            {
                state = state,
                details = string.Format("SMA: {0} | Ec: {1:F2}", Utils.FormatDistance(semiMajorAxis), eccentricity),
                largeImageKey = string.Format("body_{0}", body.name.ToLower()),
                largeImageText = body.name,
                startTimestamp = startTimestamp,
                smallImageKey = "default",
                smallImageText = "Kerbal Space Program",
            };
        }
    }
}
