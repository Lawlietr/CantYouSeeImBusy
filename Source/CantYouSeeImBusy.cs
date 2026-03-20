using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
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
        public bool ModEnabled = true;
        public Dictionary<string, float> NeedDecayRates = new Dictionary<string, float>();

        public float GetDecayRate(NeedDef def)
        {
            if (def == null) return 0f;
            return NeedDecayRates.TryGetValue(def.defName, out float rate) ? rate : 0f;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ModEnabled, "ModEnabled", true);
            Scribe_Collections.Look(ref NeedDecayRates, "NeedDecayRates", LookMode.Value, LookMode.Value);
            NeedDecayRates ??= new Dictionary<string, float>();
        }
    }
}
