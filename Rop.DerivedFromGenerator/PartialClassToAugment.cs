using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.Generators.Shared;
using Rop.GeneratorShared;


namespace Rop.Winforms7.DerivedFromGenerator
{
    public class PartialClassToAugment:BasePartialClassToAugment
    {
        public PartialClassToAugment(ClassDeclarationSyntax classToAugment) : base(classToAugment)
        {
        }

       public IEnumerable<string> GetClassNew(string formname,string newname,string basename)
        {
            yield return $"\tpublic abstract class {newname}:{basename}{{}}";
            yield return $"\tpublic partial class {formname}:{newname}{{}}";
        }

        public new IEnumerable<string> GetHeader() => this.GetHeader(Array.Empty<string>());
        public IEnumerable<string> GetHeader(IEnumerable<string> additionalusings)
        {
            return GetHeader0().Concat(GetHeader1()).Concat(GetNamespace0());
            // Local functions
            IEnumerable<string> GetHeader1()
            {
                foreach (var additionalusing in additionalusings)
                {
                    yield return $"using {additionalusing};";
                }
            }
        }
        public override IEnumerable<string> GetFooter()
        {
            yield return "}";
        }
    }
}
