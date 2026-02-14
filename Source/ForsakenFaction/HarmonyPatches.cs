using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using Verse;
using Verse.AI;

namespace ForsakenFaction
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("ForsakenFaction");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            var original = AccessTools.Method(typeof(WorkGiver_GrowerSow), "ExtraRequirements");
            harmony.Unpatch(original, HarmonyPatchType.Postfix, "com.alphagenes");
        }
    }

    //---------- PLANT UTILITY PATCHES ----------//
    [HarmonyPatch(typeof(PlantUtility), "GrowthSeasonNow", new Type[] { typeof(IntVec3), typeof(Map), typeof(ThingDef) })]
    public static class FOF_GrowthSeasonNow_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(IntVec3 c, Map map, ThingDef plantDef, ref bool __result)
        {
            string str = plantDef?.defName;
            if (str != null && str == "FOF_PlantUltraviolett")
            {
                __result = true;
            }
            else if ((c.GetZone(map) is Zone_Growing zone_Growing) && zone_Growing.GetPlantDefToGrow()?.defName == "FOF_PlantUltraviolett")
            {
                __result = true;
            }
        }
    }

    //---------- GENE LOCKING PATCHES ----------//
    [HarmonyPatch(typeof(Bill), "PawnAllowedToStartAnew")]
    public static class FOF_PawnAllowedToStartBill_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref bool __result, Bill __instance, Pawn p)
        {
            if (__result)
            {
                ForsakenKnowledgeExtension extension = __instance.recipe.GetModExtension<ForsakenKnowledgeExtension>() ?? __instance.recipe.ProducedThingDef?.GetModExtension<ForsakenKnowledgeExtension>();
                if (extension != null && !extension.CanDo(p))
                {
                    JobFailReason.Is("FOF_NeedsForsakenKnowledge".Translate());
                    __result = false;
                }
            }
        }
    }

    //---------- FACTION GOODWILL PATCHES ----------//
    [HarmonyPatch(typeof(GoodwillSituationWorker_NaturalEnemy), "GetNaturalGoodwillOffset")]
    public static class FOF_GetNaturalGoodwill_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result, Faction other)
        {
            Faction ofPlayer = Find.FactionManager.OfPlayer;
            FactionGoodwillExtension extension = ofPlayer.def.GetModExtension<FactionGoodwillExtension>();
            if (extension != null && extension.ignoreNaturalEnemy.Contains(other.def))
                __result = 0;
        }
    }
}
