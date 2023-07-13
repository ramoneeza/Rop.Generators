using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.ObservableGenerator
{
    public class FieldToAugment
    {
        public string FinalName { get; }
        public string FieldName { get; }
        public FieldDeclarationSyntax Field { get; }
        public AttributeSyntax Attribute { get; }
        public MethodDeclarationSyntax Method { get; set; }

        public FieldToAugment(FieldDeclarationSyntax field, AttributeSyntax attribute, MethodDeclarationSyntax method)
        {
            FieldName = field.Declaration.Variables.First().Identifier.Text;
            var finalName = FieldName;
            if (finalName.StartsWith("_")) finalName = finalName.Substring(1);
            if (char.IsLower(finalName[0])) finalName = finalName.Substring(0, 1).ToUpper() + finalName.Substring(1);
            FinalName = finalName;
            Field = field;
            Attribute = attribute;
            Method = method;
        }
    }
}