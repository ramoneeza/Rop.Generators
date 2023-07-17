﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.Generators.Shared;

namespace Rop.DerivedFromGenerator
{
    [Generator]
    public class DerivedFromGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ClassesToAugmentReceiver());
        }


        public void Execute(GeneratorExecutionContext context)
        {
            var collector = context.SyntaxReceiver as ClassesToAugmentReceiver;
            if (collector==null) return;
            var normal=collector.GetNormalClassesToAugment();
            var forms=collector.GetFormClassesToAugment();
            var formscomplet=collector.GetFormClassesToAugmentComplet();
            var namespacenormalflatten = "";
            var namespaceformflatten = "";
            if (normal.Count != 0)
            {
                namespacenormalflatten=generateNormalFlatten(context,normal,collector);
            }
            if (forms.Count != 0)
            {
               namespaceformflatten=generateSpecialFlatten(context,forms,collector);
            }
            if (normal.Count != 0)
            {
                foreach (var classtoaugment in collector.ClassesToAugment)
                {
                    generateCode(context, classtoaugment,namespacenormalflatten);
                }
            }
            if (formscomplet.Count != 0 && namespaceformflatten!="")
            {
                foreach (var classtoaugment in collector.ClassesToAugment)
                {
                    generateCode(context, classtoaugment,namespaceformflatten);
                }
            }
            collector.Clear();
        }

        private string generateNormalFlatten(GeneratorExecutionContext context, List<ProxyPartialClassToAugment> normal, ClassesToAugmentReceiver collector)
        {
            if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.namespace", out var mynamespace))
            {
                collector.Ts.TraceEvent(TraceEventType.Error, 1, "build_property.namespace not found");
                return "";
            }
            var sb = new StringBuilder();
            sb.AppendLine("// Autogenerated code for flatten");
            var basestoflat = normal.Select(x =>(x.BaseToFlat.ToString(),x.FlatBaseName)).Distinct().ToList();
            sb.AppendLine($"namespace {mynamespace};");
            foreach (var b in basestoflat)
            {
                var (basename, flatbasename) = b;
                sb.AppendLine($"public partial class {flatbasename}: {basename} {{}}");
            }
            var final = sb.ToString();
            context.AddSource("flattenclasses.g.cs", final);
            return mynamespace;
        }
        private string generateSpecialFlatten(GeneratorExecutionContext context, List<ProxyPartialClassToAugment> special, ClassesToAugmentReceiver collector)
        {
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var dir);
            if (string.IsNullOrEmpty(dir))
            {
                collector.Ts.TraceEvent(TraceEventType.Error, 1, "build_property.projectdir not found");
                return "";
            }
            
            var prjfile = "";
            try
            {
                prjfile = Directory.GetFiles(dir,"*.csproj").FirstOrDefault();
            }
            catch (Exception e)
            {
                collector.Ts.TraceEvent(TraceEventType.Error, 1, e.Message);
                return "";
            }

            if (string.IsNullOrEmpty(prjfile))
            {
                collector.Ts.TraceEvent(TraceEventType.Error, 1, "Project file not found");
                return "";
            }
            var fds=File.ReadAllLines(prjfile).Where(l =>_regex.IsMatch(l));
            var fdsm=fds.Select(l => _regex.Match(l).Groups[1].Value).ToList();
            var fd=fdsm.FirstOrDefault(p=>p.EndsWith(".FormDerived.csproj",StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(fd))
            {
                collector.Ts.TraceEvent(TraceEventType.Error, 1, "FormDerived project not found. In order to use FormDerivedFrom you need an auxiliar project called <ProjectDir>/FormDerived/<ProyectName>.FormDerived.csproj");
                return "";
            }
            if (fd.StartsWith(".."))fd = Path.GetFullPath(Path.Combine(dir,fd));
            if (!File.Exists(fd))
            {
                collector.Ts.TraceEvent(TraceEventType.Error, 1, "FormDerived directory not found. In order to use FormDerivedFrom you need an auxiliar project called <ProyectName>.FormDerived.csproj");
                return "";
            }
            if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.rootnamespace", out var mynamespace))
            {
                collector.Ts.TraceEvent(TraceEventType.Error, 1, "build_property.rootnamespace not found");
                return "";
            }

            var finalfile=Path.Combine(Path.GetDirectoryName(fd), "Flatten.cs");
            var originalfile=File.ReadAllText(finalfile);
            
            mynamespace = mynamespace + ".FormDerived";
            var sb = new StringBuilder();
            sb.AppendLine("// Autogenerated code for flatten");
            var basestoflat = special.Select(x =>(x.BaseToFlat,x.FlatBaseName)).Distinct().ToList();
            sb.AppendLine($"namespace {mynamespace};");
            sb.AppendLine("internal partial class Dummy{}");
            foreach (var b in basestoflat)
            {
                var (basename, flatbasename) = b;
                sb.AppendLine($"public partial class {flatbasename}: {basename} {{}}");
            }
            var final = sb.ToString();
            if (final != originalfile) File.WriteAllText(finalfile,final);
            return mynamespace;
        }


        private void generateCode(GeneratorExecutionContext context, ProxyPartialClassToAugment proxyclasstoaugment,string flattennamespace)
        {
            var (classtoaugment, basetoflat) = (proxyclasstoaugment.Original, proxyclasstoaugment.BaseToFlat);
            var formname=classtoaugment.Identifier;
            var basename=basetoflat.ToString();
            var flatbasename = new string(basename.Select(c => char.IsLetterOrDigit(c) ? c : '_').ToArray());
            var file = classtoaugment.FileName +"." +flatbasename+".g.cs";
            var sb = new StringBuilder();
            sb.AppendLine("// Autogenerated code for "+basename);
            sb.AppendLines(classtoaugment.GetHeader(new []{flattennamespace}));
            sb.AppendLines(classtoaugment.GetClassNew(formname, flatbasename));
            sb.AppendLines(classtoaugment.GetFooter());
            var final = sb.ToString();
            context.AddSource(file, final);
        }
        
        private readonly Regex _regex = new Regex(@"<ProjectReference Include=\""(.*?)\""\s+\/>",RegexOptions.IgnoreCase|RegexOptions.Compiled);

        class ClassesToAugmentReceiver : ISyntaxReceiver
        {
            public TraceSource Ts { get; } = new TraceSource("DerivedFromGenerator", SourceLevels.All);
            public ConcurrentBag<ProxyPartialClassToAugment> ClassesToAugment { get; private set; } = new ConcurrentBag<ProxyPartialClassToAugment>();
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (!(syntaxNode is ClassDeclarationSyntax cds)) return;
                // Business logic to decide what we're interested in goes here
                var basesof = cds.GenericBasesOfAny("IDerivedFrom", "IFormDerivedFrom", "IFormFlat");
                if (basesof.Any())
                {
                    var ac = new PartialClassToAugment(cds);
                    foreach (var b in basesof)
                    {
                        var t=b.TypeArgumentList.Arguments[0];
                        ClassesToAugment.Add(new ProxyPartialClassToAugment(ac,t,b.Identifier.Text));
                    }
                }
            }
            public void Clear()
            {
                ClassesToAugment = new ConcurrentBag<ProxyPartialClassToAugment>();
            }

            public List<ProxyPartialClassToAugment> GetNormalClassesToAugment()
            {
                return ClassesToAugment.Where(c=>c.DerivedType=="IDerivedFrom").ToList();
            }

            public List<ProxyPartialClassToAugment> GetFormClassesToAugment()
            {
                return ClassesToAugment.Where(c => c.DerivedType!="IDerivedFrom").ToList();
            }

            public List<ProxyPartialClassToAugment> GetFormClassesToAugmentComplet()
            {
                return ClassesToAugment.Where(c => c.DerivedType!="IFormDerivedFrom").ToList();
            }
        }
    }

    
}
