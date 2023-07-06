using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Rop.Generators.Shared
{
    public static partial class SymbolHelperAtts
    {
        public static string GetShortName(this AttributeData adata)
        {
            var n=adata?.AttributeClass?.Name ?? "";
            if (n.EndsWith("Attribute"))
                n = n.Substring(0, n.Length - "Attribute".Length);
            return n;
        }


        /// <summary>
        /// Member is decorated with some attribute
        /// </summary>
        public static bool IsDecoratedWith(this ISymbol item, string attname,params string[] attname2) => IsDecoratedWith(item, attname2.Prepend(attname),out _);

        /// <summary>
        /// Member is decorated with some attribute
        /// </summary>
        public static bool IsDecoratedWith(this ISymbol item, string attname) =>
            IsDecoratedWith(item, attname, out _);

        /// <summary>
        /// Member is decorated with some attribute
        /// </summary>
        public static bool IsDecoratedWith(this ISymbol item, string attname,out AttributeData decorated)
        {
            decorated = GetDecoratedWith(item, attname);
            return decorated != null;
        }

        /// <summary>
        /// Member is decorated with some attribute
        /// </summary>
        public static bool IsDecoratedWith(this ISymbol item,IEnumerable<string> attnames,out AttributeData decorated)
        {
            decorated = GetDecoratedWith(item, attnames);
            return decorated != null;
        }
        /// <summary>
        /// Member is decorated with some attributes
        /// </summary>
        /// <param name="item"></param>
        /// <param name="attnames"></param>
        /// <returns></returns>
        public static bool IsDecoratedWith(this ISymbol item,ImmutableHashSet<string> attnames,out AttributeData decorated)
        {
            decorated = GetDecoratedWith(item, attnames);
            return decorated != null;
        }

        /// <summary>
        /// Get decorated attribute for a class
        /// </summary>
        public static AttributeData GetDecoratedWith(this ISymbol item, string attname)
        {
            return item.GetAttributes().FirstOrDefault(a => a.GetShortName().Equals(attname));
        }
        /// <summary>
        /// Get decorated attribute for a class
        /// </summary>
        public static AttributeData GetDecoratedWith(this ISymbol item,IEnumerable<string> attname)
        {
            var lst = attname.ToImmutableHashSet();
            return GetDecoratedWith(item, lst);
        }
        public static AttributeData GetDecoratedWith(this ISymbol item,ImmutableHashSet<string> attnames)
        {
            return item.GetAttributes().FirstOrDefault(a =>a.GetShortName().InList(attnames));
        }
        
        public static AttributeData GetDecoratedWith(this ISymbol item, string attname,params string[] attname2)
        {
            var lst = attname2.Prepend(attname);
            return GetDecoratedWith(item,lst);
        }

        public static string GetParametersDefinition(this IMethodSymbol meth)
        {
            var pars = new List<string>();
            foreach (var p in meth.Parameters)
            {
                var ps =$"{p.Type} {p.MetadataName}";
                switch (p.RefKind)
                {
                    case RefKind.In: ps = "in " + ps;
                        break;
                    case RefKind.Out: ps = "out " + ps;
                        break;
                    case RefKind.Ref: ps = "ref " + ps;
                        break;
                }
                pars.Add(ps);
            }
            var j = string.Join(",",pars);
            return j;
        }
        public static string GetParametersSignature(this IMethodSymbol meth)
        {
            var pars = meth.Parameters.Select(p => $"{p.OriginalDefinition}");
            var pdef = string.Join(",",pars);
            return pdef;
        }

        public static string GetParametersNames(this IMethodSymbol meth)
        {
            var pars = new List<string>();
            foreach (var p in meth.Parameters)
            {
                var ps = p.MetadataName;
                switch (p.RefKind)
                {
                    case RefKind.In: ps = "in " + ps;
                        break;
                    case RefKind.Out: ps = "out " + ps;
                        break;
                    case RefKind.Ref: ps = "ref " + ps;
                        break;
                }
                pars.Add(ps);
            }
            var j = string.Join(",",pars);
            return j;
        }

        public static string ToSignature(this ISymbol s)
        {
            switch (s)
            {
                case IMethodSymbol m:
                    
                    return $"{m.Name}({m.GetParametersSignature()})";
                default:
                    return s.Name;
            }
        }
    }
}