using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.GeneratorShared;

namespace Rop.OneOfExtensionGenerator
{
    public class PartialClassToAugment:BasePartialClassToAugment
    {
        public PartialClassToAugment(ClassDeclarationSyntax classToAugment) : base(classToAugment)
        {
        }
        public new IEnumerable<string> GetHeader()=>base.GetHeader();
    }
}
