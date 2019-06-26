using System;
using System.Linq;
using System.Text.RegularExpressions;
using Harmony;
using StardewModdingAPI;

namespace TwentyFourHours
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod, IAssetEditor
    {
        // internal static IMonitor Logger;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // ModEntry.Logger = this.Monitor;
            var harmony = HarmonyInstance.Create("com.github.princeleto.twentyfourhours");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
        }

        public static string ConvertTime(int time)
        {
            var hours = time / 100 % 24;
            var minutes = time % 100;
            var hoursText = (hours < 10 ? "0" : "") + hours.ToString();
            var minutesText = (minutes < 10 ? "0" : "") + minutes.ToString();
            return hoursText + ":" + minutesText;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            if (asset.AssetNameEquals("Strings/StringsFromCSFiles"))
            {
                return true;
            }

            return false;
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
