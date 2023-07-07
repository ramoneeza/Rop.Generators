using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.Generators.Shared;

namespace Rop.GeneratorShared
{
    public abstract class BasePartialClassToAugment
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

        protected BasePartialClassToAugment(ClassDeclarationSyntax classToAugment)
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

        protected virtual IEnumerable<string> GetHeader0()
        {
            yield return "#nullable enable";
            foreach (var u in Usings)
            {
                yield return u.sentence;
            }
        }
        protected virtual IEnumerable<string> GetNamespace0()
        {
            yield return $"namespace {Namespace}";
            yield return "{";
        }

        protected virtual IEnumerable<string> GetClass0()
        {
            yield return $"\t{Modifier} {(IsStatic?"static ":"")}partial class {Identifier}{GenericTypes}";
            yield return "\t{";
        }
        protected virtual IEnumerable<string> GetHeader()
        {
            return GetHeader0().Concat(GetNamespace0()).Concat(GetClass0());
        }
        public virtual IEnumerable<string> GetFooter()
        {
            yield return "\t}";
            yield return "}";
        }
    }
}
