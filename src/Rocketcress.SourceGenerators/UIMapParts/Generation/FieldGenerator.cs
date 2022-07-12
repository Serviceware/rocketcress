using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.Extensions;
using Rocketcress.SourceGenerators.UIMapParts.Models;

namespace Rocketcress.SourceGenerators.UIMapParts.Generation;

internal readonly struct FieldGenerator
{
    private readonly UIMapPartsGeneratorContext _context;
    private readonly SourceBuilder _builder;

    private FieldGenerator(UIMapPartsGeneratorContext context, SourceBuilder builder)
    {
        _context = context;
        _builder = builder;
    }

    public static FieldGenerator Generate(UIMapPartsGeneratorContext context, SourceBuilder builder) => new(context, builder);

    public void LocationKeys(ControlDefinition[] controls)
    {
        foreach (var control in controls)
        {
            if (control.LocationKey is null)
                continue;

            var keywords = control.LocationKey.IsStatic ? "private static readonly" : "private";
            var locationKeyTypeName = _context.UITestTypeSymbols.LocationKeyType.ToUsageString();
            var expression = control.LocationKey.BuildInitExpression(_context);
            var fieldName = $"By{control.Property.Name}";

            _builder.AppendLine($"{keywords} {locationKeyTypeName} {fieldName} = {expression};");
        }
    }
}
