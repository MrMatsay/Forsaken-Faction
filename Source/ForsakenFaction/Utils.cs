using Verse;

namespace ForsakenFaction
{
    public static class Utils
    {
        public static bool HasActiveGene(this Pawn pawn, GeneDef geneDef)
        {
            if (pawn.genes != null)
            {
                Gene gene = pawn.genes.GetGene(geneDef);
                return gene != null && gene.Active;
            }
            return false;
        }
    }
}
