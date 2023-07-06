using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.Generators.Shared
{
    public static partial class SyntaxHelperAtts
    {
        /// <summary>
        /// Get Attributes from AttributeLists
        /// </summary>
        public static IEnumerable<AttributeSyntax> GetAttributes(this MemberDeclarationSyntax item) => item.AttributeLists.SelectMany(a => a.Attributes);

        /// <summary>
        /// Member is decorated with some attribute
        /// </summary>
        public static bool IsDecoratedWith(this MemberDeclarationSyntax item, string attname,params string[] attname2) => IsDecoratedWith(item, attname2.Prepend(attname),out _);

        /// <summary>
        /// Member is decorated with some attribute
        /// </summary>
        public static bool IsDecoratedWith(this MemberDeclarationSyntax item, string attname) =>
            IsDecoratedWith(item, attname, out _);

        /// <summary>
        /// Member is decorated with some attribute
        /// </summary>
        public static bool IsDecoratedWith(this MemberDeclarationSyntax item, string attname,out AttributeSyntax decorated)
        {
            decorated = GetDecoratedWith(item, attname);
            return decorated != null;
        }

        /// <summary>
        /// Member is decorated with some attribute
        /// </summary>
        public static bool IsDecoratedWith(this MemberDeclarationSyntax item,IEnumerable<string> attnames,out AttributeSyntax decorated)
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
        public static bool IsDecoratedWith(this MemberDeclarationSyntax item,ImmutableHashSet<string> attnames,out AttributeSyntax decorated)
        {
            decorated = GetDecoratedWith(item, attnames);
            return decorated != null;
        }

        /// <summary>
        /// Get decorated attribute for a class
        /// </summary>
        public static AttributeSyntax GetDecoratedWith(this MemberDeclarationSyntax item, string attname)
        {
            return GetAttributes(item).FirstOrDefault(a => a.Name.ToString().Equals(attname));
        }
        /// <summary>
        /// Get many decorated attributes for a class
        /// </summary>
        public static AttributeSyntax[] GetDecoratedWithSome(this MemberDeclarationSyntax item, string attname)
        {
            return GetAttributes(item).Where(a => a.Name.ToString().Equals(attname)).ToArray();
        }
        /// <summary>
        /// Get many decorated attributes for a class
        /// </summary>
        public static AttributeSyntax[] GetDecoratedWithSomeGeneric(this MemberDeclarationSyntax item, string attname)
        {
            var genattname=attname+"<";
            return GetAttributes(item).Where(a => a.Name.ToString().StartsWith(genattname)).ToArray();
        }

        /// <summary>
        /// Get decorated attribute for a class
        /// </summary>
        public static AttributeSyntax GetDecoratedWith(this MemberDeclarationSyntax item,IEnumerable<string> attname)
        {
            var lst = attname.ToImmutableHashSet();
            return GetDecoratedWith(item, lst);
        }
        public static AttributeSyntax GetDecoratedWith(this MemberDeclarationSyntax item,ImmutableHashSet<string> attnames)
        {
            return GetAttributes(item).FirstOrDefault(a =>a.Name.ToString().InList(attnames));
        }
        
        public static AttributeSyntax GetDecoratedWith(this MemberDeclarationSyntax item, string attname,params string[] attname2)
        {
            var lst = attname2.Prepend(attname);
            return GetDecoratedWith(item,lst);
        }
    }
}