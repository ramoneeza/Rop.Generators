using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.DerivedFromGenerator
{
    public class ProxyPartialClassToAugment
    {
        public PartialClassToAugment Original { get; }
        public TypeSyntax BaseToFlat { get; }
        public string FlatBaseName { get; }
        public bool IsFormDerivedFrom { get; }
        public ProxyPartialClassToAugment(PartialClassToAugment original, TypeSyntax baseToFlat, bool isFormDerivedFrom)
        {
            Original = original;
            BaseToFlat = baseToFlat;
            IsFormDerivedFrom = isFormDerivedFrom;
            FlatBaseName = new string(baseToFlat.ToString().Select(c => char.IsLetterOrDigit(c) ? c : '_').ToArray());
        }
    }
}