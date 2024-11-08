using RimWorld.Planet;
using Verse;

namespace ForsakenFaction
{
    public class GenStep_DownedForsaken : GenStep_Scatterer
    {
        public override int SeedPart => 931842770;
        protected override bool CanScatterAt(IntVec3 c, Map map)
        {
            return base.CanScatterAt(c, map) && c.Standable(map) && DownedForsakenQuestUtility.GetForsakenFaction() != null;
        }
        protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
        {
            Pawn pawn;
            if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
            {
                pawn = (Pawn)parms.sitePart.things.Take(parms.sitePart.things[0]);
            }
            else
            {
                DownedRefugeeComp component = map.Parent.GetComponent<DownedRefugeeComp>();
                pawn = (component == null || !component.pawn.Any) ? DownedForsakenQuestUtility.GenerateRefugee() : component.pawn.Take(component.pawn[0]);
            }
            HealthUtility.DamageUntilDowned(pawn, false, null, null, null);
            HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
            GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
            pawn.mindState.WillJoinColonyIfRescued = true;
            MapGenerator.rootsToUnfog.Add(loc);
            MapGenerator.SetVar<CellRect>("RectOfInterest", CellRect.CenteredOn(loc, 1, 1));
        }
    }
}
