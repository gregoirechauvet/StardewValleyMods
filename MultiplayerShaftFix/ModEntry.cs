using Harmony;
using MultiplayerShaftFix.Framework;
using StardewModdingAPI;
using StardewValley.Locations;

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
            var harmony = HarmonyInstance.Create(this.ModManifest.UniqueID);
            harmony.Patch(
                original: AccessTools.Method(typeof(MineShaft), nameof(MineShaft.enterMineShaft)),
                transpiler: new HarmonyMethod(typeof(PatchEnterMineShaft), nameof(PatchEnterMineShaft.Transpiler))
            );
        }
    }
}
