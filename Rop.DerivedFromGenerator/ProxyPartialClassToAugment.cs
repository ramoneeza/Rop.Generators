using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.DerivedFromGenerator
{
    public class ProxyPartialClassToAugment
    {
        public PartialClassToAugment Original { get; }
        public TypeSyntax BaseToFlat { get; }
        public string FlatBaseName { get; }
        public string DerivedType { get; }
        public ProxyPartialClassToAugment(PartialClassToAugment original, TypeSyntax baseToFlat, string derivedtype)
        {
            Original = original;
            BaseToFlat = baseToFlat;
            DerivedType = derivedtype;
            FlatBaseName = new string(baseToFlat.ToString().Select(c => char.IsLetterOrDigit(c) ? c : '_').ToArray());
        }
    }
}