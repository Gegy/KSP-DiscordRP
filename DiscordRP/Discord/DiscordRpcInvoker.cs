namespace DiscordRP.Discord
{
    interface DiscordRpcInvoker
    {
        void Initialize(string applicationId, ref DiscordRpc.EventHandlers handlers, bool autoRegister, string optionalSteamId);

        void Shutdown();

        void RunCallbacks();

        void UpdatePresence(ref DiscordRpc.RichPresence presence);

        void Respond(string userId, DiscordRpc.Reply reply);
    }
}
