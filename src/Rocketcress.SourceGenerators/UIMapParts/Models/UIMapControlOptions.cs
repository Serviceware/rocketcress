using Microsoft.CodeAnalysis;
using Rocketcress.Core.Attributes;

namespace Rocketcress.SourceGenerators.UIMapParts.Models;

[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "Id is fine.")]
internal readonly struct UIMapControlOptions
{
    public UIMapControlOptions(
        bool initialize,
        string? parentControl,
        IdStyle idStyle,
        string? idFormat,
        string? id)
    {
        Initialize = initialize;
        ParentControl = parentControl;
        IdStyle = idStyle;
        IdFormat = idFormat;
        Id = id;
    }

    public bool Initialize { get; }
    public string? ParentControl { get; }
    public IdStyle IdStyle { get; }
    public string? IdFormat { get; }
    public string? Id { get; }

    public static UIMapControlOptions FromAttribute(AttributeData attribute, GenerateUIMapPartsOptions classOptions)
    {
        var initialize = UIMapControlAttributeDefault.Initialize;
        var parentControl = UIMapControlAttributeDefault.ParentControl;
        var idStyle = UIMapControlAttributeDefault.IdStyle;
        var idFormat = UIMapControlAttributeDefault.IdFormat;
        var id = UIMapControlAttributeDefault.Id;

        foreach (var argument in attribute.NamedArguments)
        {
            if (argument.Key == "Initialize")
            {
                if (argument.Value.Value is bool boolValue)
                    initialize = boolValue;
            }
            else if (argument.Key == "ParentControl")
            {
                parentControl = argument.Value.Value as string;
            }
            else if (argument.Key == "IdStyle")
            {
                if (argument.Value.Value is int intValue)
                    idStyle = (IdStyle)intValue;
            }
            else if (argument.Key == "IdFormat")
            {
                idFormat = argument.Value.Value as string;
            }
            else if (argument.Key == "Id")
            {
                id = argument.Value.Value as string;
            }
        }

        return new UIMapControlOptions(
            initialize,
            parentControl,
            idStyle == IdStyle.Unset ? classOptions.IdStyle : idStyle,
            idFormat is null ? classOptions.IdFormat : idFormat,
            id);
    }
}
