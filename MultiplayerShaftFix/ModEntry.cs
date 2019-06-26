using Harmony;
using StardewModdingAPI;

namespace MultiplayerShaftFix
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            var harmony = HarmonyInstance.Create("com.github.princeleto.twentyfourhours");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}
