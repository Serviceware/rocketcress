using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.Extensions;
using Rocketcress.SourceGenerators.UIMapParts.Models;
using Rocketcress.SourceGenerators.UIMapParts.Selenium;

namespace Rocketcress.SourceGenerators.UIMapParts.Generation
{
    internal readonly struct MethodGenerator
    {
        private readonly UIMapPartsGeneratorContext _context;
        private readonly SourceBuilder _builder;

        public MethodGenerator(UIMapPartsGeneratorContext context, SourceBuilder builder)
        {
            _context = context;
            _builder = builder;
        }

        public static MethodGenerator Generate(UIMapPartsGeneratorContext context, SourceBuilder builder) => new(context, builder);

        public void InitializeOverride(ControlDefinition[] controls)
        {
            using (_builder.AddBlock($"protected override void Initialize()"))
            {
                _builder.AppendLine("OnInitializing();")
                        .AppendLine($"base.Initialize();")
                        .AppendLine("OnBaseInitialized();");

                foreach (var control in GetControlsToInitialize(controls))
                {
                    var parent = control.ParentName;
                    if (parent == "this" &&
                        _context is SeleniumUIMapPartsGeneratorContext seleniumContext &&
                        _context.TypeSymbol.IsAssignableTo(seleniumContext.UITestTypeSymbols.View))
                    {
                        parent = null;
                    }

                    var propertyName = control.Property.Name;
                    var type = control.ControlType.ToUsageString();
                    var firstParam = _context.ControlInitFirstParamName;
                    var lastParam = string.IsNullOrWhiteSpace(parent) ? string.Empty : $", {parent}";

                    _builder.AppendLine($"{propertyName} = new {type}({firstParam}, By{propertyName}{lastParam});")
                            .AppendLine($"On{propertyName}Initialized();");
                }

                _builder.AppendLine("OnInitialized();");
            }
        }

        public void PartialInitialize(ControlDefinition[] controls)
        {
            _builder.AppendLine("partial void OnInitializing();")
                    .AppendLine("partial void OnBaseInitialized();");

            foreach (var control in GetControlsToInitialize(controls))
                _builder.AppendLine($"partial void On{control.Property.Name}Initialized();");

            _builder.AppendLine("partial void OnInitialized();");
        }

        public void InitUsing()
        {
            var locationKeyTypeName = _context.UITestTypeSymbols.LocationKeyType.ToUsageString();
            var typeName = _context.TypeSymbol.ToUsageString();
            var controlBaseTypeName = _context.UITestTypeSymbols.ControlBaseType.ToUsageString();

            _builder.AppendLine($"private static TRocketcressControl InitUsing<TRocketcressControl>(global::System.Linq.Expressions.Expression<global::System.Func<{locationKeyTypeName}>> locationKeyExpression = null) where TRocketcressControl : {controlBaseTypeName} => default(TRocketcressControl);");
            _builder.AppendLine($"private static TRocketcressControl InitUsing<TRocketcressControl>(global::System.Linq.Expressions.Expression<global::System.Func<{typeName}, {locationKeyTypeName}>> locationKeyExpression) where TRocketcressControl : {controlBaseTypeName} => default(TRocketcressControl);");
        }

        private static IEnumerable<ControlDefinition> GetControlsToInitialize(IEnumerable<ControlDefinition> controls)
        {
            return controls.Where(x => x.Initialize);
        }
    }
}
