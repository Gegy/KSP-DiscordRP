using System;
using UnityEngine;
using DiscordRP.States;

namespace DiscordRP
{
    class StateTracker
    {
        private readonly GameStateTimer launchStateTimer;
        private readonly GameStateTimer landedStateTimer;

        private readonly GameStateTimer buildingStateTimer;

        private readonly GameStateTimer idleStateTimer;

        public StateTracker()
        {
            this.launchStateTimer = new GameStateTimer(scene => HighLogic.LoadedSceneIsGame && FlightGlobals.ActiveVessel != null, scene => !FlightGlobals.ActiveVessel.Landed && !FlightGlobals.ActiveVessel.Splashed);
            this.landedStateTimer = GameStateTimer.Inverse(launchStateTimer);

            this.buildingStateTimer = new GameStateTimer(scene => true, scene => scene == GameScenes.EDITOR);

            this.idleStateTimer = new GameStateTimer(scene => true, scene => scene != GameScenes.EDITOR && scene != GameScenes.FLIGHT);

            UpdateTimers();
        }

        public void UpdateTimers()
        {
            launchStateTimer.Update();
            landedStateTimer.Update();

            buildingStateTimer.Update();

            idleStateTimer.Update();
        }

        public PresenceState UpdateState()
        {
            if (HighLogic.LoadedSceneIsGame)
            {
                Vessel activeVessel = FlightGlobals.ActiveVessel;
                Part rootEditorPart = EditorLogic.RootPart;

                if (activeVessel != null)
                {
                    return GetFlightState(activeVessel);
                }
                else if (rootEditorPart != null)
                {
                    return new BuildingState(Utils.GetTotalParts(rootEditorPart), Utils.GetTotalCost(rootEditorPart), buildingStateTimer.Timestamp);
                }
            }

            return new IdlingState(idleStateTimer.Timestamp, HighLogic.LoadedScene);
        }

        private PresenceState GetFlightState(Vessel activeVessel)
        {
            double periapsis = activeVessel.orbit.GetPeriapsis();
            double apoapsis = activeVessel.orbit.GetApoapsis();

            if (activeVessel.Landed)
            {
                return new LandedState(activeVessel.mainBody, activeVessel.latitude, activeVessel.longitude, landedStateTimer.Timestamp);
            }
            else if (activeVessel.Splashed)
            {
                return new SplashedState(activeVessel.mainBody, activeVessel.latitude, activeVessel.longitude, landedStateTimer.Timestamp);
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
    }
}
