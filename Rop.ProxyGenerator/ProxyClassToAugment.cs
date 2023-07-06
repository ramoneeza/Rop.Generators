using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.ProxyGenerator
{
    public class ProxyClassToAugment
    {
        public PartialClassToAugment ClassToAugment { get; set; }
        public InterfaceToProxy InterfaceToProxy { get; private set; }
        public ProxyClassToAugment(ClassDeclarationSyntax classToAugment,AttributeSyntax att)
        {
            ClassToAugment = new PartialClassToAugment(classToAugment);
            InterfaceToProxy = InterfaceToProxy.Factory(classToAugment,att);
        }
    }

    public class InterfaceToProxy
    {
        public TypeName InterfaceName { get; set; }
        public string FieldName { get; set; }
        public ImmutableHashSet<string> Excludes { get; } 
        public string[] GenericNames { get; set; } = Array.Empty<string>();

        private static string _inside(string left, string right, string cad)
        {
            var p1 = cad.IndexOf(left);
            var p2 = cad.LastIndexOf(right);
            if (p1 == -1 || p2 == -1) return "";
            return cad.Substring(p1 + 1, p2 - p1 - 1);
        }
        private string _stripnames(string cad)
        {
            if (cad.StartsWith("nameof("))
            {
                cad = _inside("(", ")", cad);
            }
            if (cad.StartsWith("\""))
            {
                cad=_inside("\"", "\"", cad);
            }
            return cad;
        }
        private InterfaceToProxy(TypeName interfacename, string fieldname, string excludesstr)
        {
            InterfaceName = interfacename;
            FieldName = fieldname;
            var p1 = excludesstr.IndexOf("{");
            var p2 = excludesstr.IndexOf("}");
            var exc = new List<string>();
            if (p1 != -1 && p2 != -1)
            {
                excludesstr = _inside("{", "}", excludesstr);
            }

            var exct = excludesstr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => _stripnames(s));
            exc.AddRange(exct);
            Excludes = exc.ToImmutableHashSet();
        }

        public static InterfaceToProxy Factory(ClassDeclarationSyntax classToAugment,AttributeSyntax att)
        {
            //var att = classToAugment.GetDecoratedWith("ProxyOf");
            var values = att.ArgumentList.ToStringValues().ToList();
            if (values.Count < 2 || values.Count > 3) return null;
            var tipo =new TypeName(values[0]);
            var field = values[1];
            var excludes = (values.Count == 3) ? values[2] : "";
            return new InterfaceToProxy(tipo, field, excludes);
        }
    }
}