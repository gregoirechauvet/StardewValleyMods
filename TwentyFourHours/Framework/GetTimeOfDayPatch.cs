using Harmony;
using StardewValley;

namespace TwentyFourHours.Framework
{
    [HarmonyPatch(typeof(Game1))]
    [HarmonyPatch("getTimeOfDayString")]
    internal class GetTimeOfDayPatch
    {
        static bool Prefix()
        {
            return false;
        }

        static void Postfix(int time, ref string __result)
        {
            __result = ModEntry.ConvertTime(time);
        }
    }
}
