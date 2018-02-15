using System;
using UnityEngine;
using DiscordRP.States;

// Reference: https://github.com/discordapp/discord-rpc
namespace DiscordRP.Discord
{
    class PresenceController
    {
        private const string applicationId = "386261941259337738";

        public void Initialize()
        {
            DiscordRpc.EventHandlers handlers = new DiscordRpc.EventHandlers();
            handlers.readyCallback = ReadyCallback;
            handlers.disconnectedCallback += DisconnectedCallback;
            handlers.errorCallback += ErrorCallback;
            handlers.joinCallback += JoinCallback;
            handlers.spectateCallback += SpectateCallback;
            handlers.requestCallback += RequestCallback;

            DiscordRpc.Initialize(applicationId, ref handlers, true, null);

            Debug.Log("DiscordRP: Discord Initialize");
        }

        public void Disable()
        {
            DiscordRpc.Shutdown();
            Debug.Log("DiscordRP: Discord Shutdown");
        }

        public void UpdatePresence(PresenceState state)
        {
            DiscordRpc.RichPresence presence = state.create();

            Debug.Log(string.Format("DiscordRP: Send presence: {0} ({1})", presence, state));

            DiscordRpc.UpdatePresence(ref presence);
        }

        public void UpdateCallbacks()
        {
            DiscordRpc.RunCallbacks();
        }

        public void ReadyCallback()
        {
            Debug.Log("DiscordRP: Discord Ready");
        }

        public void DisconnectedCallback(int errorCode, string message)
        {
            Debug.Log(string.Format("DiscordRP: Discord Disconnect {0}: {1}", errorCode, message));
        }

        public void ErrorCallback(int errorCode, string message)
        {
            Debug.Log(string.Format("DiscordRP: Discord Error: {0}: {1}", errorCode, message));
        }

        public void JoinCallback(string secret)
        {
        }

        public void SpectateCallback(string secret)
        {
        }

        public void RequestCallback(DiscordRpc.JoinRequest request)
        {
            DiscordRpc.Respond(request.userId, DiscordRpc.Reply.Yes);
        }
    }
}
