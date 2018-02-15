using System;
using UnityEngine;

namespace DiscordRP
{
    static class Utils
    {
        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static float GetTotalCost(Part part)
        {
            AvailablePart availablePart = PartLoader.getPartInfoByName(part.name);

            if (availablePart != null && availablePart.costsFunds)
            {
                float cost = availablePart.cost;

                foreach (PartResource resource in part.Resources)
                {
                    double unusedAmount = resource.maxAmount - resource.amount;
                    cost -= (float) (unusedAmount * PartResourceLibrary.Instance.GetDefinition(resource.resourceName).unitCost);
                }

                foreach (PartModule module in part.Modules)
                {
                    if (module is IPartCostModifier)
                    {
                        IPartCostModifier costModifier = module as IPartCostModifier;
                        cost = costModifier.GetModuleCost(cost, ModifierStagingSituation.CURRENT);
                    }
                }

                foreach (Part child in part.children)
                {
                    cost += GetTotalCost(child);
                }

                return cost;
            }
            else
            {
                return 0.0F;
            }
        }

        public static int GetTotalParts(Part part)
        {
            int parts = 1;

            foreach (Part child in part.children)
            {
                parts += GetTotalParts(child);
            }

            return parts;
        }

        public static long GetEpochTime()
        {
            return (long) (DateTime.UtcNow - EPOCH).TotalSeconds;
        }

        public static bool IsWindows()
        {
            return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
        }

        public static bool IsOSX()
        {
            Debug.Log(string.Format("DiscordRP: Detected OS: {0}", Application.platform));
            return Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor;
        }

        public static bool Is64BitProcess()
        {
            return IntPtr.Size == 8;
        }

        public static double GetApoapsis(this Orbit orbit)
        {
            return orbit.semiMajorAxis * (1.0 + orbit.eccentricity);
        }

        public static double GetPeriapsis(this Orbit orbit)
        {
            return orbit.semiMajorAxis * (1.0 - orbit.eccentricity);
        }

        public static string FormatDistance(double distance)
        {
            if (distance > 2000000)
            {
                return string.Format("{0:F0}Mm", distance / 1000000.0);
            }
            else if (distance > 2000)
            {
                return string.Format("{0:F0}km", distance / 1000.0);
            }
            else
            {
                return string.Format("{0:F0}m", distance);
            }
        }

        public static string GetSmallFlightIcon(bool paused)
        {
            if (paused)
            {
                return "paused";
            }
            else
            {
                return "default";
            }
        }

        public static string GetSmallFlightIconDetails(bool paused)
        {
            if (paused)
            {
                return "Paused game";
            }
            else
            {
                return "Kerbal Space Program";
            }
        }
    }
}
