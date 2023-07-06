using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.Generators.Shared
{
    public static partial class SyntaxHelper
    {
       /// <summary>
        /// Childs of type T
        /// </summary>
        public static IEnumerable<T> ChildNodesOfType<T>(this SyntaxNode node) => node.ChildNodes().OfType<T>();

       public static IEnumerable<SyntaxNode> ChildNodesOfType(this SyntaxNode node,params Type[] types)
        {
            return node.ChildNodes().Where(n => types.Any(t=>IsSameOrSubclass(t,n.GetType())));
        }
        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;
        }

        public static bool IsStatic(this ClassDeclarationSyntax cds)
        {
           return cds.Modifiers.Any(SyntaxKind.StaticKeyword);
        }
        public static bool IsGeneric(this ClassDeclarationSyntax cds)
        {
            return cds.TypeParameterList?.Parameters.Count > 0;
        }

        

        public static List<GenericNameSyntax> GenericBasesOfType(this ClassDeclarationSyntax cds, string typename)
        {
            var res=new List<GenericNameSyntax>();
            if (cds.BaseList is null) return res;
            foreach (var type in cds.BaseList.Types)
            {
                if (type.Type is GenericNameSyntax gns)
                {
                    if (gns.Identifier.ToString() == typename)
                    {
                        res.Add(gns);
                    }
                }
            }
            return res;
        }
        public static string ToStringValue(this ExpressionSyntax expression)
        {
            switch (expression)
            {
                case TypeOfExpressionSyntax tof:
                    return tof.Type.ToString();
                case InvocationExpressionSyntax inv:
                    var a = inv.ArgumentList.Arguments.FirstOrDefault() as ArgumentSyntax;
                    IdentifierNameSyntax i=null;
                    if (a?.Expression is IdentifierNameSyntax aasi)
                    {
                        i = aasi;
                    }
                    else
                    {
                        var b =  a?.Expression as MemberAccessExpressionSyntax;
                        i = b?.ChildNodes().Last() as IdentifierNameSyntax;
                    }
                    return i?.Identifier.ToString() ?? "";
                case ArrayCreationExpressionSyntax arr:
                    var arrv = arr.Initializer.Expressions.Select(c => c.ToStringValue());
                    return string.Join(",", arrv);
                default:
                    var v= expression.ToString();
                    if (v.StartsWith("\"")) v = v.Substring(1);
                    if (v.EndsWith("\"")) v = v.Substring(0,v.Length-1);
                    return v;
            }
        }
        public static IEnumerable<string> ToStringValues(this AttributeArgumentListSyntax argumentlist)
        {
            foreach (var arg in argumentlist.Arguments)
            {
                yield return arg.Expression.ToStringValue();
            }
        }

        public static IEnumerable<(string, string)> GetUsings(this SyntaxTree syntaxTree)
        {
            var r = syntaxTree.GetRoot();
            return r.ChildNodesOfType<UsingDirectiveSyntax>().Select(u => (u.Name.ToString(), u.ToString())).ToList();
        }
        public static string GetNamespace(this SyntaxTree syntaxTree){
            var r = syntaxTree.GetRoot();
            return r.ChildNodesOfType<BaseNamespaceDeclarationSyntax>().FirstOrDefault()?.Name.ToString();
        }

        public static IEnumerable<ISymbol> GetOrderedMembers(this INamedTypeSymbol typeSymbol,bool inherited)
        {
            var morder = new OrderedNames();
            var dic=new Dictionary<string,ISymbol>();
            _getOrderedMembers(typeSymbol,inherited,morder,dic);
            //var mnames = morder.ToImmutableHashSet();
            //var dic=typeSymbol.GetMembers().Where(mm => mm.Name.InList(mnames)).ToDictionary(mm=>mm.Name);
            foreach (var m in dic.Values)
            {
                yield return m;
            }
        }

        private class OrderedName
        {
            public string Name { get; set; }
            public int Order { get; set; } = 999999;
        }

        private class OrderedNames
        {
            private readonly Dictionary<string,OrderedName> _dic=new Dictionary<string,OrderedName>();

            public void Add(string name)
            {
                if (_dic.ContainsKey(name)) return;
                _dic[name] = new OrderedName() { Name = name, Order = _dic.Count };
            }

            public void AddRange(IEnumerable<string> name)
            {
                foreach (var n in name)
                {
                    Add(n);
                }
            }

            public OrderedName Get(string name)
            {
                if (!_dic.TryGetValue(name, out var res)) return null;
                return res;
            }
            public bool Contains(string name) => _dic.ContainsKey(name);
        }

        private class OrderedMember  
        {
           public OrderedMember(ISymbol m, OrderedName order)
            {
                this.Member = m;
                this.Order = order.Order;
                Name = m.Name;
                Signature = m.ToSignature();
            }

            public string Name { get; }
            public string Signature { get; }
            public ISymbol Member { get; }
            public int Order { get; set; } = 999999;
        } 
        
        private static void _getOrderedMembers(this INamedTypeSymbol typeSymbol, bool inherited,OrderedNames morder,Dictionary<string,ISymbol> dic)
        {
            if (inherited)
            {
                var sub = typeSymbol.Interfaces;
                foreach (var s in sub)
                {
                    _getOrderedMembers(s,true, morder,dic);
                }
            }
            var singlemorder = typeSymbol.MemberNames;
            morder.AddRange(singlemorder);
            var lstmembers = new List<OrderedMember>();
            foreach (var m in typeSymbol.GetMembers())
            {
                if (!(m is IEventSymbol || m is IPropertySymbol || m is IMethodSymbol)) continue;
                var order = morder.Get(m.Name);
                if (order == null) continue;
                lstmembers.Add(new OrderedMember(m,order));
            }
            lstmembers = lstmembers.OrderBy(m => m.Order).ToList();
            foreach (var m in lstmembers)
            {
                dic[m.Signature] = m.Member;
            }
        }

        public static string FlatTypeName(string name)
        {
            return new string(name.Select(c => char.IsLetterOrDigit(c)?c:'_').ToArray());
        }
    }
}
