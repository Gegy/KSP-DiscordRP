using DiscordRP;
using DiscordRP.Discord;
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

    private static DiscordRpcInvoker invoker;

    static DiscordRpc()
    {
        if (Utils.IsWindows())
        {
            if (Utils.Is64BitProcess())
            {
                invoker = new Win64Invoker();
            }
            else
            {
                invoker = new Win32Invoker();
            }
        }
        else if (Utils.IsOSX())
        {
            invoker = new OSXInvoker();
        }
        else
        {
            invoker = new LinuxInvoker();
        }
    }

    public static void Initialize(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId)
    {
        invoker.Initialize(applicationId, ref handlers, autoRegister, optionalSteamId);
    }

    public static void Shutdown()
    {
        invoker.Shutdown();
    }

    public static void RunCallbacks()
    {
        invoker.RunCallbacks();
    }

    public static void UpdatePresence(ref RichPresence presence)
    {
        invoker.UpdatePresence(ref presence);
    }

    public static void Respond(string userId, Reply reply)
    {
        invoker.Respond(userId, reply);
    }
}
