namespace TwentyFourHours.Framework
{
    internal class GetTimeOfDayPatch
    {
        public static bool Prefix()
        {
            return false;
        }

        public static void Postfix(int time, ref string __result)
        {
            __result = ModEntry.ConvertTime(time);
        }
    }
}
