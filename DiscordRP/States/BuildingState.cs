using System;
using UnityEngine;

namespace DiscordRP.States
{
    class BuildingState : PresenceState
    {
        private readonly int partCount;
        private readonly double cost;
        private readonly long startTimestamp;

        public BuildingState(int partCount, double cost, long startTimestamp)
        {
            this.partCount = partCount;
            this.cost = cost;
            this.startTimestamp = startTimestamp;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is BuildingState)
            {
                BuildingState buildingState = (BuildingState) obj;

                return buildingState.cost == cost && buildingState.partCount == partCount && buildingState.startTimestamp == startTimestamp; ;
            }

            return false;
        }

        public DiscordRpc.RichPresence create()
        {
            return new DiscordRpc.RichPresence()
            {
                state = "Building a craft",
                details = string.Format("Cst: ${1:F0} | Prts: {0}", partCount, cost),
                largeImageKey = "building_craft",
                largeImageText = "Building a craft",
                startTimestamp = startTimestamp,
                smallImageKey = "default",
                smallImageText = "Kerbal Space Program",
            };
        }
    }
}
