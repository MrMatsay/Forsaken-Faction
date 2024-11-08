using RimWorld;
using Verse;

namespace ForsakenFaction
{
    [DefOf]
    public static class FOFDefOf
    {
        static FOFDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FOFDefOf));
        }

        [MayRequire("sarg.alphagenes")]
        [MayRequireBiotech]
        public static GeneDef AG_ForsakenKnowledge;
    }
}
