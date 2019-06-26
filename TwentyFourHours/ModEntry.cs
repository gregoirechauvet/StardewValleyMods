using System;
using System.Linq;
using System.Text.RegularExpressions;
using Harmony;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using TwentyFourHours.Framework;

namespace TwentyFourHours
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod, IAssetEditor
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            var harmony = HarmonyInstance.Create(this.ModManifest.UniqueID);
            harmony.Patch(
                original: AccessTools.Method(typeof(DayTimeMoneyBox), nameof(DayTimeMoneyBox.draw)),
                transpiler: new HarmonyMethod(typeof(DayTimeMoneyBoxPatch), nameof(DayTimeMoneyBoxPatch.Transpiler))
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(Game1), nameof(Game1.getTimeOfDayString)),
                prefix: new HarmonyMethod(typeof(GetTimeOfDayPatch), nameof(GetTimeOfDayPatch.Prefix)),
                postfix: new HarmonyMethod(typeof(GetTimeOfDayPatch), nameof(GetTimeOfDayPatch.Postfix))
            );
        }

        public static string ConvertTime(int time)
        {
            var hours = time / 100 % 24;
            var minutes = time % 100;
            var hoursText = (hours < 10 ? "0" : "") + hours;
            var minutesText = (minutes < 10 ? "0" : "") + minutes;
            return hoursText + ":" + minutesText;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Strings/StringsFromCSFiles");
        }

        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals("Strings/StringsFromCSFiles"))
            {
                string pattern = @"([0-9]{1,2}):([0-9]{2})(AM|PM)";

                var data = asset.AsDictionary<string, string>().Data;
                foreach (string key in data.Keys.ToArray())
                {
                    string value = data[key];

                    MatchCollection matches = Regex.Matches(value, pattern);
                    foreach (Match match in matches)
                    {
                        GroupCollection hop = match.Groups;
                        int hours = Int32.Parse(hop[1].ToString());
                        string minutes = hop[2].ToString();
                        string afternoon = hop[3].ToString();

                        if ((afternoon == "AM" && hours == 12) || (afternoon == "PM" && hours < 12))
                        {
                            hours = (hours + 12) % 24;
                        }

                        string hoursText = (hours < 10 ? "0" : "") + hours;
                        string result = hoursText + ":" + minutes;

                        var regex = new Regex(Regex.Escape(hop[0].ToString()));
                        value = regex.Replace(value, result, 1);
                    }

                    data[key] = value;
                }
            }
        }
    }
}
