using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.GeneratorHelper;

namespace Rop.Winforms7.ControllerGenerator
{
    public class ControllerToInclude
    {
        public string ControllerName { get; }
        public string DesiredInstanceName { get; private set; } = "";
        public ClassDeclarationSyntax Cds { get; }
        public INamedTypeSymbol NamedTypeSymbol { get; private set; } = null;
        public string ControllerFor { get; private set; }="";
        public string ControllerNamesPace { get; private set; }
        public ControllerToInclude(ClassDeclarationSyntax cds)
        {
            ControllerName = cds.Identifier.Text;
            Cds=cds;
            ControllerNamesPace = cds.SyntaxTree.GetNamespace();
        }

        public bool IsControllerFor(string name, Compilation contextCompilation)
        {
            if (NamedTypeSymbol == null)
            {
                var candidatos=contextCompilation.GetSymbolsWithName(s => s.EndsWith(ControllerName), SymbolFilter.Type);
                NamedTypeSymbol = candidatos.FirstOrDefault() as INamedTypeSymbol;
                var baseType = NamedTypeSymbol?.BaseType;
                if (baseType?.IsGenericType??false)
                {
                    ControllerFor = baseType.TypeArguments[0].Name;
                }
                DesiredInstanceName = ControllerName.StartsWith(ControllerFor) ? ControllerName.Substring(ControllerFor.Length) : ControllerName;
            }
            return ControllerFor == name;
        }
    }
}