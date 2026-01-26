using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace ForsakenFaction
{
    public class FactionGoodwillExtension : DefModExtension
    {
        public List<FactionDef> ignoreNaturalEnemy = new List<FactionDef>();
    }
}
