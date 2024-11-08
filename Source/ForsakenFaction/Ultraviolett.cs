using RimWorld;
using System.Text;
using Verse;

namespace ForsakenFaction
{
    public class Ultraviolett : Plant
    {
        public override float GrowthRate => base.GrowthRateFactor_Fertility * base.GrowthRateFactor_Light;

        protected override void CheckMakeLeafless()
        {
        }
        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (def.plant.showGrowthInInspectPane)
            {
                if (LifeStage == PlantLifeStage.Growing)
                {
                    stringBuilder.AppendLine("PercentGrowth".Translate(base.GrowthPercentString));
                    stringBuilder.AppendLine("GrowthRate".Translate() + ": " + GrowthRate.ToStringPercent());
                    if (!base.Blighted)
                    {
                        if (Resting)
                        {
                            stringBuilder.AppendLine("PlantResting".Translate());
                        }
                        if (!HasEnoughLightToGrow)
                        {
                            stringBuilder.AppendLine("PlantNeedsLightLevel".Translate() + ": " + def.plant.growMinGlow.ToStringPercent());
                        }
                    }
                }
                else
                {
                    if (LifeStage == PlantLifeStage.Mature)
                    {
                        if (HarvestableNow)
                        {
                            stringBuilder.AppendLine("ReadyToHarvest".Translate());
                        }
                        else
                        {
                            stringBuilder.AppendLine("Mature".Translate());
                        }
                    }
                }
                if (DyingBecauseExposedToLight)
                {
                    stringBuilder.AppendLine("DyingBecauseExposedToLight".Translate());
                }
                if (base.Blighted)
                {
                    stringBuilder.AppendLine("Blighted".Translate() + " (" + base.Blight.Severity.ToStringPercent() + ")");
                }
            }
            string text = base.InspectStringPartsFromComps();
            if (!text.NullOrEmpty())
            {
                stringBuilder.Append(text);
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }
    }
}
