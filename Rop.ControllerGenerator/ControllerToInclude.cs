using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.Generators.Shared;

namespace Rop.Winforms7.ControllerGenerator
{
    public class ControllerToInclude
    {
        public string ControllerName { get; }
        public string DesiredInstanceName { get; }
        public INamedTypeSymbol NamedTypeSymbol { get; }
        public string ControllerFor { get;}
        public string ControllerNamesPace { get; }

        private ControllerToInclude(INamedTypeSymbol namedTypeSymbol, string controllerName, string desiredInstanceName, string controllerFor, string controllerNamesPace)
        {
            NamedTypeSymbol = namedTypeSymbol;
            ControllerName = controllerName;
            DesiredInstanceName = desiredInstanceName;
            ControllerFor = controllerFor;
            ControllerNamesPace = controllerNamesPace;
        }
        public static ControllerToInclude Factory(ClassDeclarationSyntax controller, Compilation contextCompilation)
        {
            var controllerName = controller.Identifier.Text;
            var controllerNamesPace = controller.SyntaxTree.GetNamespace();
            if (string.IsNullOrEmpty(controllerNamesPace)) return null;
            var namedtypesymbol = contextCompilation.GetSymbolsWithName(s => s.EndsWith(controllerName), SymbolFilter.Type)
                .OfType<INamedTypeSymbol>().FirstOrDefault();
            if (namedtypesymbol == null) return null;
            var baseType = namedtypesymbol.BaseType;
            if (baseType == null) return null;
            if (!baseType.IsGenericType) return null;
            var controllerFor = baseType.TypeArguments[0].Name;
            var desiredInstanceName = controllerName.StartsWith(controllerFor) ? controllerName.Substring(controllerFor.Length) : controllerName;
            return new ControllerToInclude(namedtypesymbol, controllerName, desiredInstanceName, controllerFor, controllerNamesPace);
        }
    }
}