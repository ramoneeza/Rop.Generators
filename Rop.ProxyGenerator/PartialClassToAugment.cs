using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.Generators.Shared;
using Rop.GeneratorShared;

namespace Rop.ProxyGenerator
{
    public class PartialClassToAugment:BasePartialClassToAugment
    {
        public bool DisableNullable { get; }
        public PartialClassToAugment(ClassDeclarationSyntax classToAugment):base(classToAugment)
        {
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
    }
}
