using System;
using UnityEngine;
using DarkMultiPlayerCommon;
using System.Collections.Generic;

namespace DiscordRP.States
{
    class OnlineState : PresenceState, IEquatable<OnlineState>
    {
        private readonly Enum joinable;

        public OnlineState(Enum joinable)
        {
            this.joinable = ClientState.CONNECTED;
        }

        public override bool Equals( object online)
        {

            if (joinable != null && online is OnlineState)
            {
                OnlineState OnlineState = (OnlineState) online;
            }
            return false;

        }

        public DiscordRpc.RichPresence create()
        {
            return new DiscordRpc.RichPresence()
            {
                state = "Playing Online",
                details = string.Format("Connect: {0}",  joinable),
                largeImageKey = "playing_online",
                largeImageText = "Playing Online",
                startTimestamp = 0,
                smallImageKey = "default",
                smallImageText = "Kerbal Space Program",
            };
        }

        public bool Equals(OnlineState other)
        {
            return other != null &&
                   EqualityComparer<Enum>.Default.Equals(joinable, other.joinable);
        }

        public override int GetHashCode()
        {
            var hashCode = -981442036;
            hashCode = hashCode * -1521134295 + EqualityComparer<Enum>.Default.GetHashCode(joinable);
            return hashCode;
        }
    }
}
