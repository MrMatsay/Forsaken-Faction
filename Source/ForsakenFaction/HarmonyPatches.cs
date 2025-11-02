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
        }
    }

    //---------- PLANT UTILITY PATCHES ----------//
    [HarmonyPatch(typeof(PlantUtility), "GrowthSeasonNow", new Type[] { typeof(IntVec3), typeof(Map), typeof(ThingDef) })]
    public static class FOF_GrowthSeasonNow_Postfix
    {
        [HarmonyPostfix]
        public static void Postfix(IntVec3 c, Map map, ThingDef plantDef, ref bool __result)
        {
            string str = plantDef?.defName;
            if (str == "FOF_PlantUltraviolett")
            {
                __result = true;
            }
            else if ((c.GetZone(map) is Zone_Growing zone_Growing) && zone_Growing.GetPlantDefToGrow().defName == "FOF_PlantUltraviolett")
            {
                __result = true;
            }
        }
    }

    //---------- GENE LOCKING PATCHES ----------//
    [HarmonyPatch(typeof(EquipmentUtility), "CanEquip", new Type[] { typeof(Thing), typeof(Pawn), typeof(string), typeof(bool) }, new ArgumentType[] { 0, 0, ArgumentType.Out, 0 })]
    public static class FOF_CanEquip_Postfix
    {
        [HarmonyPostfix]
        public static void PostFix(ref bool __result, Thing thing, Pawn pawn, ref string cantReason)
        {
            if (__result)
            {
                ForsakenKnowledgeExtension extension = thing.def.GetModExtension<ForsakenKnowledgeExtension>();
                if (extension != null && !extension.CanDo(pawn))
                {
                    cantReason = "AG_NeedsForsakenKnowledgeToWield".Translate();
                    __result = false;
                }
            }
        }
    }
    [HarmonyPatch(typeof(Bill), "PawnAllowedToStartAnew")]
    public class FOF_PawnAllowedToStartBill_Postfix
    {
        [HarmonyPostfix]
        public static void Postfix(ref bool __result, Bill __instance, Pawn p)
        {
            if (__result)
            {
                ForsakenKnowledgeExtension extension = __instance.recipe.GetModExtension<ForsakenKnowledgeExtension>() ?? __instance.recipe.ProducedThingDef?.GetModExtension<ForsakenKnowledgeExtension>();
                if (extension != null && !extension.CanDo(p))
                {
                    JobFailReason.Is("AG_NeedsForsakenKnowledge".Translate());
                    __result = false;
                }
            }
        }
    }
    [HarmonyPatch(typeof(GenConstruct), nameof(GenConstruct.CanConstruct), new Type[] { typeof(Thing), typeof(Pawn), typeof(bool), typeof(bool), typeof(JobDef) })]
    public class FOF_CanConstruct_Postfix
    {
        [HarmonyPostfix]
        public static bool Postfix(bool __result, Thing t, Pawn p, JobDef jobForReservation)
        {
            if (__result)
            {
                ForsakenKnowledgeExtension extension = t?.def?.entityDefToBuild?.GetModExtension<ForsakenKnowledgeExtension>();
                if (extension != null && !extension.CanDo(p))
                {
                    if (jobForReservation != null)
                    {
                        JobFailReason.Is("AG_NeedsForsakenKnowledge".Translate());
                    }
                    __result = false;
                }
            }
            return __result;
        }
    }
    [HarmonyPatch(typeof(WorkGiver_GrowerSow), "ExtraRequirements")]
    public static class FOF_GrowerSow_ExtraRequirements_Postfix
    {
        [HarmonyPostfix]
        public static void PostFix(ref bool __result, IPlantToGrowSettable settable, Pawn pawn)
        {
            IntVec3 c;
            if (settable is Zone_Growing zone_Growing)
            {
                c = zone_Growing.Cells[0];
            }
            else
            {
                c = ((Thing)settable).Position;
            }

            ThingDef wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, pawn.Map);
            ForsakenKnowledgeExtension extension = wantedPlantDef.GetModExtension<ForsakenKnowledgeExtension>();

            if (extension != null && !extension.CanDo(pawn))
            {
                __result = false;
            }
        }
    }
}
