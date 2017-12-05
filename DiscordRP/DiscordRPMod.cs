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

        private readonly GameStateTimer launchStateTimer = new GameStateTimer(scene => HighLogic.LoadedSceneIsGame && FlightGlobals.ActiveVessel != null && !FlightGlobals.ActiveVessel.Landed);
        private readonly GameStateTimer idleStateTimer = new GameStateTimer(scene => scene != GameScenes.EDITOR && scene != GameScenes.FLIGHT);

        private PresenceController presenceController;
        private PresenceState state;

        private float lastUpdate = 0.0F;
        private float updateInterval = 10.0F;

        private bool initialized;

        void Awake()
        {
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + PLUGIN_DIRECTORY);

            presenceController = new PresenceController();

            lastUpdate = Time.time;
            state = new IdlingState(launchStateTimer.Timestamp);
            idleStateTimer.Update();
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

            launchStateTimer.Update();
            idleStateTimer.Update();

            float currentTime = Time.time;

            if (currentTime - lastUpdate > updateInterval || !initialized)
            {
                lastUpdate = currentTime;

                PresenceState previousState = state;
                
                state = UpdateState();

                if (!state.Equals(previousState) || !initialized)
                {
                    presenceController.UpdatePresence(state);
                }

                initialized = true;
            }
        }

        private PresenceState UpdateState()
        {
            if (HighLogic.LoadedSceneIsGame)
            {
                Vessel activeVessel = FlightGlobals.ActiveVessel;
                Part rootEditorPart = EditorLogic.RootPart;

                if (activeVessel != null)
                {
                    double periapsis = activeVessel.orbit.semiMajorAxis * (1.0 - activeVessel.orbit.eccentricity);
                    double apoapsis = activeVessel.orbit.semiMajorAxis * (1.0 + activeVessel.orbit.eccentricity);

                    if (activeVessel.Landed)
                    {
                        return new LandedState(activeVessel.mainBody, activeVessel.latitude, activeVessel.longitude);
                    }
                    else if (apoapsis > activeVessel.mainBody.sphereOfInfluence || (apoapsis < 0.0 && periapsis > apoapsis))
                    {
                        return new EscapingState(activeVessel.mainBody, launchStateTimer.Timestamp);
                    }
                    else if (activeVessel.mainBody.atmosphereDepth > activeVessel.altitude || periapsis < activeVessel.mainBody.Radius)
                    {
                        return new FlyingState(activeVessel.mainBody, activeVessel.altitude, activeVessel.srfSpeed, launchStateTimer.Timestamp);
                    }
                    else
                    {
                        return new OrbitingState(activeVessel.mainBody, activeVessel.orbit.semiMajorAxis, activeVessel.orbit.eccentricity, launchStateTimer.Timestamp);
                    }
                }
                else if (rootEditorPart != null)
                {
                    return new BuildingState(Utils.GetTotalParts(rootEditorPart), Utils.GetCost(rootEditorPart));
                }
            }

            return new IdlingState(idleStateTimer.Timestamp);
        }
    }
}
