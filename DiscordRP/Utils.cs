using System;

namespace DiscordRP
{
    class Utils
    {
        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static float GetCost(Part part)
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
                    cost += GetCost(child);
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

        public static bool Is64BitProcess()
        {
            return IntPtr.Size == 8;
        }
    }
}
