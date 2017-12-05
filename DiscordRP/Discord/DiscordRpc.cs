using DiscordRP;
using System;
using System.Runtime.InteropServices;

public class DiscordRpc
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReadyCallback();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DisconnectedCallback(int errorCode, string message);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ErrorCallback(int errorCode, string message);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void JoinCallback(string secret);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SpectateCallback(string secret);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RequestCallback(JoinRequest request);

    public struct EventHandlers
    {
        public ReadyCallback readyCallback;
        public DisconnectedCallback disconnectedCallback;
        public ErrorCallback errorCallback;
        public JoinCallback joinCallback;
        public SpectateCallback spectateCallback;
        public RequestCallback requestCallback;
    }

    [System.Serializable]
    public struct RichPresence
    {
        public string state; /* max 128 bytes */
        public string details; /* max 128 bytes */
        public long startTimestamp;
        public long endTimestamp;
        public string largeImageKey; /* max 32 bytes */
        public string largeImageText; /* max 128 bytes */
        public string smallImageKey; /* max 32 bytes */
        public string smallImageText; /* max 128 bytes */
        public string partyId; /* max 128 bytes */
        public int partySize;
        public int partyMax;
        public string matchSecret; /* max 128 bytes */
        public string joinSecret; /* max 128 bytes */
        public string spectateSecret; /* max 128 bytes */
        public bool instance;

        public override string ToString()
        {
            return $"RichPresence(state:{state},details:{details},largeImageKey:{largeImageKey},smallImageKey:{smallImageKey},largeImageText:{largeImageText},smallImageText:{smallImageText})";
        }
    }

    [System.Serializable]
    public struct JoinRequest
    {
        public string userId;
        public string username;
        public string avatar;
    }

    public enum Reply
    {
        No = 0,
        Yes = 1,
        Ignore = 2
    }

    public static void Initialize(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId)
    {
        if (Utils.Is64BitProcess())
        {
            Initialize64(applicationId, ref handlers, autoRegister, optionalSteamId);
        }
        else
        {
            Initialize32(applicationId, ref handlers, autoRegister, optionalSteamId);
        }
    }

    public static void Shutdown()
    {
        if (Utils.Is64BitProcess())
        {
            Shutdown64();
        }
        else
        {
            Shutdown32();
        }
    }

    public static void RunCallbacks()
    {
        if (Utils.Is64BitProcess())
        {
            RunCallbacks64();
        }
        else
        {
            RunCallbacks32();
        }
    }

    public static void UpdatePresence(ref RichPresence presence)
    {
        if (Utils.Is64BitProcess())
        {
            UpdatePresence64(ref presence);
        }
        else
        {
            UpdatePresence32(ref presence);
        }
    }

    public static void Respond(string userId, Reply reply)
    {
        if (Utils.Is64BitProcess())
        {
            Respond64(userId, reply);
        }
        else
        {
            Respond32(userId, reply);
        }
    }

    [DllImport("discord-rpc-64.lib", EntryPoint = "Discord_Initialize", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Initialize64(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId);

    [DllImport("discord-rpc-64.lib", EntryPoint = "Discord_Shutdown", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Shutdown64();

    [DllImport("discord-rpc-64.lib", EntryPoint = "Discord_RunCallbacks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void RunCallbacks64();

    [DllImport("discord-rpc-64.lib", EntryPoint = "Discord_UpdatePresence", CallingConvention = CallingConvention.Cdecl)]
    public static extern void UpdatePresence64(ref RichPresence presence);

    [DllImport("discord-rpc-64.lib", EntryPoint = "Discord_Respond", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Respond64(string userId, Reply reply);

    [DllImport("discord-rpc-32.lib", EntryPoint = "Discord_Initialize", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Initialize32(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId);

    [DllImport("discord-rpc-32.lib", EntryPoint = "Discord_Shutdown", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Shutdown32();

    [DllImport("discord-rpc-32.lib", EntryPoint = "Discord_RunCallbacks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void RunCallbacks32();

    [DllImport("discord-rpc-32.lib", EntryPoint = "Discord_UpdatePresence", CallingConvention = CallingConvention.Cdecl)]
    public static extern void UpdatePresence32(ref RichPresence presence);

    [DllImport("discord-rpc-32.lib", EntryPoint = "Discord_Respond", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Respond32(string userId, Reply reply);
}
