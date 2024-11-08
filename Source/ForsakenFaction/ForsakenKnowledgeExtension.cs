using Verse;

namespace ForsakenFaction
{
    public class ForsakenKnowledgeExtension : DefModExtension
    {
        public bool CanDo(Pawn pawn)
        {
            return pawn.HasActiveGene(FOFDefOf.AG_ForsakenKnowledge);
        }
    }
}
