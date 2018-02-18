using System;
using System.Runtime.InteropServices;

namespace DiscordRP.Discord
{
    class Win64Invoker : DiscordRpcInvoker
    {
        private const string DISCORD_RPC_LOCATION = "GameData/DiscordRP/Plugins/win64/discord-rpc.bin";

        public void Initialize(string applicationId, ref DiscordRpc.EventHandlers handlers, bool autoRegister, string optionalSteamId)
        {
            ExternInitialize(applicationId, ref handlers, autoRegister, optionalSteamId);
        }

        public void Respond(string userId, DiscordRpc.Reply reply)
        {
            ExternRespond(userId, reply);
        }

        public void RunCallbacks()
        {
            ExternRunCallbacks();
        }

        public void Shutdown()
        {
            ExternShutdown();
        }

        public void UpdatePresence(ref DiscordRpc.RichPresence presence)
        {
            ExternUpdatePresence(ref presence);
        }

        [DllImport(DISCORD_RPC_LOCATION, EntryPoint = "Discord_Initialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ExternInitialize(string applicationId, ref DiscordRpc.EventHandlers handlers, bool autoRegister, string optionalSteamId);

        [DllImport(DISCORD_RPC_LOCATION, EntryPoint = "Discord_Shutdown", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ExternShutdown();

        [DllImport(DISCORD_RPC_LOCATION, EntryPoint = "Discord_RunCallbacks", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ExternRunCallbacks();

        [DllImport(DISCORD_RPC_LOCATION, EntryPoint = "Discord_UpdatePresence", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ExternUpdatePresence(ref DiscordRpc.RichPresence presence);

        [DllImport(DISCORD_RPC_LOCATION, EntryPoint = "Discord_Respond", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ExternRespond(string userId, DiscordRpc.Reply reply);
    }
}
