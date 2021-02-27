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
        private static readonly Dictionary<FrameworkType, string> ByFullName = new Dictionary<FrameworkType, string>
        {
            [FrameworkType.UIAutomation] = "Rocketcress.UIAutomation.By",
            [FrameworkType.Selenium] = "OpenQA.Selenium.By",
        };
        private static readonly Dictionary<FrameworkType, string> ByEmpty = new Dictionary<FrameworkType, string>
        {
            [FrameworkType.UIAutomation] = $"{ByFullName[FrameworkType.UIAutomation]}.Empty",
            [FrameworkType.Selenium] = $"{ByFullName[FrameworkType.Selenium]}.XPath(\"//*\")",
        };
        private static readonly Dictionary<FrameworkType, string> InitializeName = new Dictionary<FrameworkType, string>
        {
            [FrameworkType.UIAutomation] = $"Initialize",
            [FrameworkType.Selenium] = $"InitializeControls",
        };

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var generateAttributeDefaults = new GenerateUIMapPartsAttribute();
            var controlOptionsAttributeDefaults = new UIMapControlOptionsAttribute();

            var debugGeneratorSymbol = context.Compilation.GetTypeByMetadataName(typeof(DebugGeneratorAttribute).FullName);
            var generateAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(GenerateUIMapPartsAttribute).FullName);
            var controlOptionsAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(UIMapControlOptionsAttribute).FullName);

            var uiaControlSymbol = context.Compilation.GetTypeByMetadataName("Rocketcress.UIAutomation.Controls.UITestControl");
            var seleniumControlSymbol = context.Compilation.GetTypeByMetadataName("Rocketcress.Selenium.Controls.WebElement");

            if (generateAttributeSymbol == null)
                return;

            var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.GetNamespaceAndNestedTypes()
                        let generateAttr = typeSymbol.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, generateAttributeSymbol))
                        where generateAttr != null
                        let baseTypes = typeSymbol.GetAllBaseTypes().ToArray()
                        let isUia = baseTypes.Any(x => SymbolEqualityComparer.Default.Equals(x, uiaControlSymbol))
                        let isSelenium = baseTypes.Any(x => SymbolEqualityComparer.Default.Equals(x, seleniumControlSymbol))
                        where isUia || isSelenium
                        let frameworkType = isUia ? FrameworkType.UIAutomation : FrameworkType.Selenium
                        select (typeSymbol, generateAttr, frameworkType);

            foreach (var (typeSymbol, generateAttr, frameworkType) in query)
            {
                if (typeSymbol.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, debugGeneratorSymbol)))
                    LaunchDebuggerOnBuild();

                var generateCtors = generateAttr.NamedArguments.FirstOrDefault(x => x.Key == nameof(GenerateUIMapPartsAttribute.GenerateDefaultConstructors)).Value.Value is bool tmpCtor ? tmpCtor : generateAttributeDefaults.GenerateDefaultConstructors;
                var controlDefType = generateAttr.NamedArguments.FirstOrDefault(x => x.Key == nameof(GenerateUIMapPartsAttribute.ControlsDefinition)).Value.Value as INamedTypeSymbol;

                var controls = (from prop in controlDefType?.GetMembers().OfType<IPropertySymbol>() ?? Array.Empty<IPropertySymbol>()
                                where prop.GetMethod != null
                                let optionsAttr = controlOptionsAttributeSymbol == null ? null : prop.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, controlOptionsAttributeSymbol))
                                let initialize = optionsAttr?.NamedArguments.FirstOrDefault(x => x.Key == nameof(UIMapControlOptionsAttribute.Initialize)).Value.Value is bool tmpInit ? tmpInit : controlOptionsAttributeDefaults.Initialize
                                let parentArg = optionsAttr?.NamedArguments.FirstOrDefault(x => x.Key == nameof(UIMapControlOptionsAttribute.ParentControl))
                                let parent = parentArg.HasValue && parentArg.Value.Key == nameof(UIMapControlOptionsAttribute.ParentControl) ? parentArg.Value.Value.Value as string : controlOptionsAttributeDefaults.ParentControl
                                select (prop, initialize, parent)).ToArray();

                var builder = new SourceBuilder();

                using (builder.AddBlock($"namespace {typeSymbol.ContainingNamespace}"))
                using (AddPartialClasses(builder, typeSymbol, controlDefType))
                {
                    bool hasContent = false;

                    hasContent |= AddFieldsAndProperties(!hasContent, builder, typeSymbol, frameworkType, controls);

                    if (generateCtors)
                        hasContent |= AddConstructors(!hasContent, builder, typeSymbol);

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
                blockStack.Push(builder.AddBlock($"partial class {typeStack.Pop().Name}{(typeStack.Count == 0 && controlDefType != null ? $" : {controlDefType.ToDisplayString(DefinitionFormat)}" : string.Empty)}"));
            return blockStack;
        }

        private static bool AddConstructors(bool isFirst, SourceBuilder builder, INamedTypeSymbol typeSymbol)
        {
            var ctors = typeSymbol.BaseType.GetMembers().OfType<IMethodSymbol>().Where(x => x.MethodKind == MethodKind.Constructor && !x.IsStatic && (x.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Public || x.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Protected));

            bool hasCtor = false;
            foreach (var ctor in ctors)
            {
                if (!hasCtor && !isFirst)
                    builder.AppendLine();

                var doc = ctor.GetFormattedDocumentationCommentXml();
                if (!string.IsNullOrWhiteSpace(doc))
                    builder.AppendLine(doc);

                var accessibility = ctor.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Public ? "public" : "protected";
                var paramDef = string.Join(", ", ctor.Parameters.Select(y => y.ToDisplayString(DefinitionFormat)));
                var paramUsg = string.Join(", ", ctor.Parameters.Select(y => y.ToDisplayString(UsageFormat)));
                builder.AppendLine($"{accessibility} {typeSymbol.Name}({paramDef}) : base({paramUsg}) {{ }}");
                hasCtor = true;
            }

            return hasCtor;
        }

        private static bool AddFieldsAndProperties(bool isFirst, SourceBuilder builder, INamedTypeSymbol typeSymbol, FrameworkType frameworkType, IList<(IPropertySymbol Property, bool Initialize, string Parent)> controls)
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

                    bool hasSetter = control.Property.SetMethod != null;
                    builder.AppendLine($"/// <inheritdoc/>")
                           .AppendLine($"public {control.Property.Type.ToDisplayString(DefinitionFormat)} {control.Property.Name} {{ get; {(hasSetter ? string.Empty : "private ")}set; }}");
                    hasProperties = true;
                }
            }

            return hasFields || hasProperties;
        }

        private static bool AddInitialize(bool isFirst, SourceBuilder builder, FrameworkType frameworkType, IList<(IPropertySymbol Property, bool Initialize, string Parent)> controls)
        {
            if (!isFirst)
                builder.AppendLine();
            using (builder.AddBlock($"protected override void {InitializeName[frameworkType]}()"))
            {
                builder.AppendLine("OnInitializing();")
                       .AppendLine("base.Initialize();")
                       .AppendLine("OnBaseInitialized();");

                var propNames = new HashSet<string>(controls.Where(x => x.Initialize).Select(x => x.Property.Name));
                var existingInits = new HashSet<string>();
                var controlQueue = new Queue<(IPropertySymbol Property, bool Initialize, string Parent)>(controls.Where(x => x.Initialize));
                while (controlQueue.Count > 0)
                {
                    var (prop, _, parent) = controlQueue.Dequeue();
                    if (propNames.Contains(parent) && !existingInits.Contains(parent))
                    {
                        controlQueue.Enqueue((prop, true, parent));
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

            foreach (var (prop, _, _) in controls.Where(x => x.Initialize))
                builder.AppendLine($"partial void On{prop.Name}Initialized();");

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

        private enum FrameworkType
        {
            UIAutomation,
            Selenium,
        }
    }
}
