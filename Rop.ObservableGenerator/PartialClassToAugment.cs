using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.GeneratorShared;

namespace Rop.ObservableGenerator
{
    public class PartialClassToAugment:BasePartialClassToAugment
    {
        
        public PartialClassToAugment(ClassDeclarationSyntax classToAugment):base(classToAugment)
        {
        }
        public new IEnumerable<string> GetHeader()
        {
            yield return "#nullable enable";
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
