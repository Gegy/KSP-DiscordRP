using System;
using UnityEngine;
using DiscordRP.Discord;
using DiscordRP.States;

namespace DiscordRP
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class DiscordRPMod : MonoBehaviour
    {
        private static readonly string PLUGIN_DIRECTORY = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private PresenceController presenceController;
        private StateTracker stateTracker;

        private PresenceState state;

        private float lastUpdate = 0.0F;
        private float updateInterval = 10.0F;

        private bool initialized;

        void Awake()
        {
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + PLUGIN_DIRECTORY);

            presenceController = new PresenceController();

            lastUpdate = Time.time;
            state = new IdlingState(Utils.GetEpochTime(), GameScenes.LOADING);
            stateTracker = new StateTracker();
        }

        void Start()
        {
            Debug.Log("DiscordRP: Plugin startup");
            presenceController.Initialize();

            DontDestroyOnLoad(this);
        }

        void OnDisable()
        {
            Debug.Log("DiscordRP: Plugin disable");
            presenceController.Disable();

            initialized = false;
        }

        void Update()
        {
            presenceController.UpdateCallbacks();

            stateTracker.UpdateTimers();

            float currentTime = Time.time;

            if (currentTime - lastUpdate > updateInterval || !initialized)
            {
                lastUpdate = currentTime;

                PresenceState previousState = state;
                
                state = stateTracker.UpdateState();

                if (!state.Equals(previousState) || !initialized)
                {
                    presenceController.UpdatePresence(state);
                }

                initialized = true;
            }
        }
    }
}
