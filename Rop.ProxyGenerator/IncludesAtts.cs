using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Rop.Generators.Shared;

namespace Rop.ProxyGenerator
{
    public class IncludesAtts
    {
        public List<string> AttsToInclude { get; } = new List<string>();
        public IncludesAtts(ISymbol namedTypeSymbol)
        {
            var nextatts = namedTypeSymbol.GetAttributes().SkipWhile(a => SymbolHelperAtts.GetShortName(a) != "IncludeNextAttributes")
                .ToList();
            if (nextatts.Any())
            {
                AttsToInclude.AddRange(nextatts.Skip(1).Select(a=>a.ToString()));
            }
        }

        public void Render(StringBuilder sb, int tabs)
        {
            foreach (var att in AttsToInclude)
            {
                sb.AppendLines(tabs,$"[{att}]");
            }
        }
        public bool IsEmpty => AttsToInclude.Count == 0;
    }
}