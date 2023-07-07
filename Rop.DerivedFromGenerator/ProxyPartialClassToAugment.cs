using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.Winforms7.DerivedFromGenerator
{
    public class ProxyPartialClassToAugment
    {
        public PartialClassToAugment Original { get; }
        public TypeSyntax BaseToFlat { get; }

        public ProxyPartialClassToAugment(PartialClassToAugment original, TypeSyntax baseToFlat)
        {
            Original = original;
            BaseToFlat = baseToFlat;
        }
    }
}