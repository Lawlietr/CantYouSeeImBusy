using HarmonyLib;
using Verse;

namespace CantYouSeeImBusy
{
    public class CantYouSeeImBusyMod : Mod
    {
        public static CantYouSeeImBusySettings Settings = null!;

        public CantYouSeeImBusyMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<CantYouSeeImBusySettings>();
            var harmony = new Harmony("sum117.cantyouseeimbusy");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
        }

        public override string SettingsCategory() => "Can't You See I'm Busy";
    }

    public class CantYouSeeImBusySettings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
        }
    }
}
