using Microsoft.CodeAnalysis;
using Rocketcress.Core.Attributes;
using Rocketcress.SourceGenerators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using static Rocketcress.SourceGenerators.Common.CodeGenerationHelpers;

namespace Rocketcress.SourceGenerators
{
    [Generator]
    public class UIMapPartsGenerator : ISourceGenerator
    {
        private static readonly Dictionary<FrameworkType, string> ByFullName = new()
        {
            [FrameworkType.UIAutomation] = "Rocketcress.UIAutomation.By",
            [FrameworkType.Selenium] = "OpenQA.Selenium.By",
        };
        private static readonly Dictionary<FrameworkType, string> ByEmpty = new()
        {
            [FrameworkType.UIAutomation] = $"{ByFullName[FrameworkType.UIAutomation]}.Empty",
            [FrameworkType.Selenium] = $"{ByFullName[FrameworkType.Selenium]}.XPath(\"//*\")",
        };
        private static readonly Dictionary<FrameworkType, string> InitializeName = new()
        {
            [FrameworkType.UIAutomation] = "Initialize",
            [FrameworkType.Selenium] = "InitializeControls",
        };
        private static readonly Dictionary<FrameworkType, string> BaseControlClassFullName = new()
        {
            [FrameworkType.UIAutomation] = "Rocketcress.UIAutomation.Controls.UITestControl",
            [FrameworkType.Selenium] = "Rocketcress.Selenium.Controls.WebElement",
        };

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var debugGeneratorSymbol = context.Compilation.GetTypeByMetadataName("Rocketcress.Core.Attributes.DebugGeneratorAttribute");
            var generateAttributeSymbol = context.Compilation.GetTypeByMetadataName("Rocketcress.Core.Attributes.GenerateUIMapPartsAttribute");
            var controlOptionsAttributeSymbol = context.Compilation.GetTypeByMetadataName("Rocketcress.Core.Attributes.UIMapControlOptionsAttribute");

            var controlSymbols = (from kv in BaseControlClassFullName
                                  let symbol = context.Compilation.GetTypeByMetadataName(kv.Value)
                                  where symbol != null
                                  select (FrameworkType: (FrameworkType?)kv.Key, TypeSymbol: symbol)).ToArray();

            if (generateAttributeSymbol == null)
                return;

            var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.GetNamespaceAndNestedTypes()
                        let generateAttr = typeSymbol.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, generateAttributeSymbol))
                        where generateAttr != null
                        let baseTypes = typeSymbol.GetAllBaseTypes().ToArray()
                        let frameworkType = controlSymbols.FirstOrDefault(x => baseTypes.Any(y => SymbolEqualityComparer.Default.Equals(y, x.TypeSymbol))).FrameworkType
                        where frameworkType.HasValue
                        select (typeSymbol, generateAttr, frameworkType.Value);

            foreach (var (typeSymbol, generateAttr, frameworkType) in query)
            {
                if (typeSymbol.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, debugGeneratorSymbol)))
                    LaunchDebuggerOnBuild();

                var generateCtors = generateAttr.NamedArguments.FirstOrDefault(x => x.Key == "GenerateDefaultConstructors").Value.Value is bool tmpCtor ? tmpCtor : GenerateUIMapPartsAttributeDefaults.GenerateDefaultConstructors;
                var controlDefType = generateAttr.NamedArguments.FirstOrDefault(x => x.Key == "ControlsDefinition").Value.Value as INamedTypeSymbol;

                var controls = (from prop in controlDefType?.GetMembers().OfType<IPropertySymbol>() ?? Array.Empty<IPropertySymbol>()
                                where prop.GetMethod != null
                                let optionsAttr = controlOptionsAttributeSymbol == null ? null : prop.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, controlOptionsAttributeSymbol))
                                let initialize = optionsAttr?.NamedArguments.FirstOrDefault(x => x.Key == "Initialize").Value.Value is bool tmpInit ? tmpInit : UIMapControlOptionsAttributeDefault.Initialize
                                let parentArg = optionsAttr?.NamedArguments.FirstOrDefault(x => x.Key == "ParentControl")
                                let parent = parentArg.HasValue && parentArg.Value.Key == "ParentControl" ? parentArg.Value.Value.Value as string : UIMapControlOptionsAttributeDefault.ParentControl
                                let accessibility = optionsAttr?.NamedArguments.FirstOrDefault(x => x.Key == "Accessibility").Value.Value is int tmpAccess ? (ControlPropertyAccessibility)tmpAccess : UIMapControlOptionsAttributeDefault.Accessibility
                                let isVirtual = optionsAttr?.NamedArguments.FirstOrDefault(x => x.Key == "IsVirtual").Value.Value is bool tmpVirtual ? tmpVirtual : UIMapControlOptionsAttributeDefault.IsVirtual
                                select new ControlDefinition(prop, initialize, parent, accessibility, isVirtual)).ToArray();

                var builder = new SourceBuilder();

                using (builder.AddBlock($"namespace {typeSymbol.ContainingNamespace}"))
                using (AddPartialClasses(builder, typeSymbol, controlDefType))
                {
                    bool hasContent = false;

                    hasContent |= AddFieldsAndProperties(!hasContent, builder, typeSymbol, frameworkType, controls);

                    if (generateCtors)
                        hasContent |= AddConstructors(!hasContent, builder, typeSymbol, frameworkType);

                    hasContent |= AddInitialize(!hasContent, builder, frameworkType, controls);
                }

                context.AddSource(typeSymbol, builder, nameof(UIMapPartsGenerator));
            }
        }

        private static IDisposable AddPartialClasses(SourceBuilder builder, INamedTypeSymbol typeSymbol, INamedTypeSymbol controlDefType)
        {
            var typeStack = new Stack<INamedTypeSymbol>();
            var current = typeSymbol;
            while (current != null)
            {
                typeStack.Push(current);
                current = current.ContainingType;
            }

            var blockStack = new DisposableStack();
            while (typeStack.Count > 0)
            {
                // Do not include interface implementation automatically because of potential private members.
                // var impl = typeStack.Count == 0 && controlDefType != null ? $" : {controlDefType.ToDisplayString(DefinitionFormat)}" : string.Empty;
                var impl = string.Empty;
                blockStack.Push(builder.AddBlock($"partial class {typeStack.Pop().ToDisplayString(TypeDefinitionFormat)}{impl}"));
            }

            return blockStack;
        }

        private static bool AddConstructors(bool isFirst, SourceBuilder builder, INamedTypeSymbol typeSymbol, FrameworkType frameworkType)
        {
            var ctors = typeSymbol.BaseType.GetMembers().OfType<IMethodSymbol>().Where(x => x.MethodKind == MethodKind.Constructor && !x.IsStatic && (x.DeclaredAccessibility == Accessibility.Public || x.DeclaredAccessibility == Accessibility.Protected));
            var xCtors = typeSymbol.GetMembers().OfType<IMethodSymbol>().Where(x => !x.IsImplicitlyDeclared && x.MethodKind == MethodKind.Constructor && !x.IsStatic && (x.DeclaredAccessibility == Accessibility.Public || x.DeclaredAccessibility == Accessibility.Protected));

            bool hasCtor = false;
            foreach (var ctor in ctors)
            {
                if (xCtors.Any(x => x.Parameters.Select(x => x.Type).SequenceEqual(ctor.Parameters.Select(x => x.Type), SymbolEqualityComparer.Default)))
                    continue;

                if (!hasCtor && !isFirst)
                    builder.AppendLine();

                /* Visual Studio currently has problems with XML documentation on constructors in partial classes
                var doc = ctor.GetFormattedDocumentationCommentXml()?.Replace(BaseControlClassFullName[frameworkType], typeSymbol.ToDisplayString(DefinitionFormat));
                if (!string.IsNullOrWhiteSpace(doc))
                    builder.AppendLine(doc);
                */

                var accessibility = ctor.DeclaredAccessibility == Accessibility.Public ? "public" : "protected";
                var paramDef = string.Join(", ", ctor.Parameters.Select(y => y.ToDisplayString(DefinitionFormat)));
                var paramUsg = string.Join(", ", ctor.Parameters.Select(y => y.ToDisplayString(UsageFormat)));
                builder.AppendLine($"{accessibility} {typeSymbol.Name}({paramDef}) : base({paramUsg}) {{ }}");
                hasCtor = true;
            }

            return hasCtor;
        }

        private static bool AddFieldsAndProperties(bool isFirst, SourceBuilder builder, INamedTypeSymbol typeSymbol, FrameworkType frameworkType, IList<ControlDefinition> controls)
        {
            var existingFields = new HashSet<string>(typeSymbol.GetMembers().OfType<IFieldSymbol>().Select(x => x.Name));
            bool hasFields = false;
            foreach (var control in controls)
            {
                var fieldName = $"By{control.Property.Name}";
                if (!existingFields.Contains(fieldName))
                {
                    if (!hasFields && !isFirst)
                        builder.AppendLine();

                    builder.AppendLine($"private static readonly {ByFullName[frameworkType]} {fieldName} = {ByEmpty[frameworkType]};");
                    hasFields = true;
                }
            }

            var existingProperties = new HashSet<string>(typeSymbol.GetMembers().OfType<IPropertySymbol>().Select(x => x.Name));
            bool hasProperties = false;
            foreach (var control in controls)
            {
                if (!existingProperties.Contains(control.Property.Name))
                {
                    if ((hasFields && !hasProperties) || (!hasFields && !hasProperties && !isFirst))
                        builder.AppendLine();

                    string accessibility = control.Accessibility switch
                    {
                        ControlPropertyAccessibility.Private => "private",
                        ControlPropertyAccessibility.Protected => "protected",
                        _ => "public",
                    };
                    if (control.IsVirtual && control.Accessibility != ControlPropertyAccessibility.Private)
                        accessibility += " virtual";

                    bool hasSetter = control.Property.SetMethod != null;
                    string setterAccessibility = hasSetter || control.Accessibility == ControlPropertyAccessibility.Private ? string.Empty : "private ";

                    builder.AppendLine($"/// <inheritdoc/>")
                           .AppendLine($"{accessibility} {control.Property.Type.ToDisplayString(DefinitionFormat)} {control.Property.Name} {{ get; {setterAccessibility}set; }}");
                    hasProperties = true;
                }
            }

            return hasFields || hasProperties;
        }

        private static bool AddInitialize(bool isFirst, SourceBuilder builder, FrameworkType frameworkType, IList<ControlDefinition> controls)
        {
            if (!isFirst)
                builder.AppendLine();
            using (builder.AddBlock($"protected override void {InitializeName[frameworkType]}()"))
            {
                builder.AppendLine("OnInitializing();")
                       .AppendLine($"base.{InitializeName[frameworkType]}();")
                       .AppendLine("OnBaseInitialized();");

                var propNames = new HashSet<string>(controls.Where(x => x.Initialize).Select(x => x.Property.Name));
                var existingInits = new HashSet<string>();
                var controlQueue = new Queue<ControlDefinition>(controls.Where(x => x.Initialize));
                while (controlQueue.Count > 0)
                {
                    var control = controlQueue.Dequeue();
                    var (prop, _, parent, _, _) = control;
                    if (propNames.Contains(parent) && !existingInits.Contains(parent))
                    {
                        controlQueue.Enqueue(control);
                        continue;
                    }

                    builder.AppendLine($"{prop.Name} = new {prop.Type.ToDisplayString(DefinitionFormat)}(By{prop.Name}{(string.IsNullOrWhiteSpace(parent) ? string.Empty : $", {parent}")});")
                           .AppendLine($"On{prop.Name}Initialized();");
                    existingInits.Add(prop.Name);
                }

                builder.AppendLine("OnInitialized();");
            }

            builder.AppendLine()
                   .AppendLine("partial void OnInitializing();")
                   .AppendLine("partial void OnBaseInitialized();");

            foreach (var control in controls.Where(x => x.Initialize))
                builder.AppendLine($"partial void On{control.Property.Name}Initialized();");

            builder.AppendLine("partial void OnInitialized();");

            return true;
        }

        private class DisposableStack : Stack<IDisposable>, IDisposable
        {
            public void Dispose()
            {
                while (Count > 0)
                    Pop().Dispose();
            }
        }

        private record ControlDefinition(
            IPropertySymbol Property,
            bool Initialize,
            string Parent,
            ControlPropertyAccessibility Accessibility,
            bool IsVirtual);

        private enum FrameworkType
        {
            UIAutomation,
            Selenium,
        }
    }
}
