using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.ProxyGenerator
{
    public class PartialClassToAugment
    {
        public bool IsStatic { get; }
        public bool IsGeneric { get; }
        public string GenericTypes { get; }
        public string Identifier { get; }
        public string FileName { get;  }
        public string Namespace { get;  }
        public string Modifier { get; }
        public bool DisableNullable { get; }
        public IReadOnlyList<(string name, string sentence)> Usings { get;  }
        public ClassDeclarationSyntax Original { get; }
        public PartialClassToAugment(ClassDeclarationSyntax classToAugment)
        {
            Original= classToAugment;
            Identifier = classToAugment.Identifier.ToString();
            var stfp = Path.GetFileNameWithoutExtension(classToAugment.SyntaxTree.FilePath);
            FileName = (string.IsNullOrEmpty(stfp)) ? Identifier : stfp;
            Usings = classToAugment.SyntaxTree.GetUsings().ToList();
            Namespace = classToAugment.SyntaxTree.GetNamespace();
            Modifier = classToAugment.Modifiers.FirstOrDefault().ToString();
            IsStatic = classToAugment.IsStatic();
            IsGeneric = classToAugment.IsGeneric();
            GenericTypes = (IsGeneric) ? classToAugment.TypeParameterList?.ToString()??"" : "";
            var attr=classToAugment.GetDecoratedWith("ProxyOfDisableNullable");
            if (attr!=null)
            {
                DisableNullable = true;
            }
        }
        public IEnumerable<string> GetHeader(INamedTypeSymbol interfacetoinclude)
        {
            if (!DisableNullable)
            {
                yield return "#nullable enable";
            }
            else
            {
                yield return "// nullable is disabled by attribute";
            }
            foreach (var u in Usings)
            {
                yield return u.sentence;
            }

            yield return $"namespace {Namespace}";
            yield return "{";
            //yield return $"\tpublic {(IsStatic?"static ":"")}partial class {Identifier}:{interfacetoinclude.ToDisplayString()}";
            yield return $"\t{Modifier} {(IsStatic?"static ":"")}partial class {Identifier}{GenericTypes}";
            yield return "\t{";
        }
        public IEnumerable<string> GetFooter()
        {
            yield return "\t}";
            yield return "}";
        }
    }
}
