using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.GeneratorHelper;

namespace Rop.Winforms7.StaticExtensionGenerator
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
        public IEnumerable<string> GetFooter()
        {
            yield return "\t}";
            yield return "}";
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
