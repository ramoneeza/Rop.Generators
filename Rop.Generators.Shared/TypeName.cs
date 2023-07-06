using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Rop.Generators.Shared
{
    public struct TypeName
    {
        public readonly string Name;
        public readonly string FullName;
        public readonly bool IsGeneric;
        public readonly TypeName[] GenericNames;

        public TypeName(string tname)
        {
            if (string.IsNullOrEmpty(tname)) throw new ArgumentException("Type name is empty");
            if (!tname.Contains('<'))
            {
                Name = tname;
                FullName=tname;
                IsGeneric = false;
                GenericNames = Array.Empty<TypeName>();
            }
            else
            {
                FullName = tname;
                IsGeneric = true;
                var p1 = tname.IndexOf("<");
                var p2=tname.LastIndexOf('>');
                var inner=tname.Substring(p1+1,p2-p1-1);
                Name = tname.Substring(0, p1);
                var sp = inner.Split(',');
                var all=sp.Select(x =>new TypeName(x.Trim())).ToArray();
                GenericNames = all;
            }
        }

        public string ReplaceGeneric(params string[] generics)
        {
            if (GenericNames.Length != generics.Length) throw new ArgumentException("Number of generics mismatch");
            if (!IsGeneric) return FullName;
            var j = string.Join(",", generics);
            return $"{Name}<{j}>";
        }
        public bool Equals(INamedTypeSymbol interf)
        {
            if (interf?.Name != Name) return false;
            var ts = interf.TypeArguments.Select(a => a.ToString()).ToList();
            if (GenericNames.Length!=ts.Count) return false;
            for (var f = 0; f < GenericNames.Length; f++)
            {
                var a = ts[f];
                var b = GenericNames[f].FullName;
                if (a.Equals(b)) continue;
                var p = a.LastIndexOf('.');
                if (p != -1)
                {
                    a = a.Substring(p + 1);
                }
                if (!a.Equals(b)) return false;
            }
            return true;
        }
    }
}