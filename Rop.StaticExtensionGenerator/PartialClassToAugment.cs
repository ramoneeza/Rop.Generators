using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.Generators.Shared;
using Rop.GeneratorShared;

namespace Rop.Winforms7.StaticExtensionGenerator
{
    public class PartialClassToAugment:BasePartialClassToAugment
    {
        public PartialClassToAugment(ClassDeclarationSyntax classToAugment) : base(classToAugment)
        {
        }
        public IEnumerable<string> GetHeader(IEnumerable<string> additionalusings)
        {
            yield return "#nullable enable";
            foreach (var u in Usings)
            {
                yield return u.sentence;
            }
            foreach (var additionalusing in additionalusings)
            {
                yield return $"using {additionalusing};";
            }
            yield return $"namespace {Namespace}";
            yield return "{";
            yield return $"\t{Modifier} {(IsStatic?"static ":"")}partial class {Identifier}{GenericTypes}";
            yield return "\t{";
        }
    }

    public class ProxyPartialClassToAugment
    {
        public PartialClassToAugment Original { get; }
        public GenericNameSyntax ExtensionNameType { get; }

        public ProxyPartialClassToAugment(PartialClassToAugment original,GenericNameSyntax att)
        {
            Original = original;
            ExtensionNameType = att;
        }
    }
}
