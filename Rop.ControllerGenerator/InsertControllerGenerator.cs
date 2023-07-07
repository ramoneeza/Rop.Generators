﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.Generators.Shared;

namespace Rop.Winforms7.ControllerGenerator
{
    [Generator]
    public class InsertControllerGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ClassesToAugmentReceiver());
        }
        public void Execute(GeneratorExecutionContext context)
        {
            var collector = context.SyntaxReceiver as ClassesToAugmentReceiver;
            if (collector == null || collector.ClassesToAugment.Count == 0 || collector.ControllersToInclude.Count==0) return;
            var dic=collector.ProcessControllers(context.Compilation);
            foreach (var classtoaugment in collector.ClassesToAugment)
            {
                generateCode(context, classtoaugment,dic);
            }
            collector.Clear();
        }
        private void generateCode(GeneratorExecutionContext context, PartialClassToAugment classtoaugment,Dictionary<string,List<ControllerToInclude>> diccontrollers)
        {
            var formname=classtoaugment.Identifier;
            var file = classtoaugment.FileName +".Controllers_.g.cs";
            var model = context.Compilation.GetSemanticModel(classtoaugment.Original.SyntaxTree);
            var classmodel = (INamedTypeSymbol)model.GetDeclaredSymbol(classtoaugment.Original);
            if (classmodel is null) return;
            if (!diccontrollers.TryGetValue(formname, out var finalcontrollers)) return;
            var sb = new StringBuilder();
            sb.AppendLine("// Autogenerated code for Controllers");
            var usings = finalcontrollers.Select(x => x.ControllerNamesPace).Distinct().ToList();
            sb.AppendLines(classtoaugment.GetHeader(usings));
            foreach (var symbol in finalcontrollers)
            {
                var name = symbol.DesiredInstanceName;
                sb.AppendLines($"\t\tpublic {symbol.ControllerName} {name}{{get; private set;}}");
            }
            sb.AppendLine("\t\tprivate void InitControllers(){");
            foreach (var symbol in finalcontrollers)
            {
                sb.AppendLines($"\t\t\t {symbol.DesiredInstanceName}=new(this);");
            }
            sb.AppendLine("\t\t}");
            sb.AppendLines(classtoaugment.GetFooter());
            var final = sb.ToString();
            context.AddSource(file, final);
        }
        
        class ClassesToAugmentReceiver : ISyntaxReceiver
        {
            public ConcurrentBag<PartialClassToAugment> ClassesToAugment { get; private set; } = new ConcurrentBag<PartialClassToAugment>();
            public ConcurrentBag<ClassDeclarationSyntax> ControllersToInclude { get; private set; } = new ConcurrentBag<ClassDeclarationSyntax>();
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (!(syntaxNode is ClassDeclarationSyntax cds)) return;
                // Business logic to decide what we're interested in goes here
                if (cds.IsDecoratedWith("InsertControllers"))
                {
                    var ac = new PartialClassToAugment(cds);
                    ClassesToAugment.Add(ac);
                }
                if (cds.IsDecoratedWith("Controller") || (cds.SyntaxTree.GetNamespace().EndsWith("Controllers")))
                {
                    if ((cds.BaseList?.Types.Count??0) != 0) ControllersToInclude.Add(cds);
                }
            }
            public void Clear()
            {
                ClassesToAugment = new ConcurrentBag<PartialClassToAugment>();
                ControllersToInclude = new ConcurrentBag<ClassDeclarationSyntax>();
            }

            public Dictionary<string,List<ControllerToInclude>> ProcessControllers(Compilation contextCompilation)
            {
                var dic=new Dictionary<string,List<ControllerToInclude>>();
                foreach (var controller in ControllersToInclude)
                {
                    var cti=ControllerToInclude.Factory(controller,contextCompilation);
                    if (cti is null) continue;
                    var name = cti.ControllerFor;
                    if (!dic.TryGetValue(name, out var lst))
                    {
                        lst=new List<ControllerToInclude>();
                        dic[name]=lst;
                    }
                    lst.Add(cti);
                }
                return dic;
            }
        }
    }

    
}