﻿using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.UIMapParts.Common;

namespace Rocketcress.SourceGenerators.UIMapParts.Selenium;

internal class SeleniumTypeSymbols : UITestTypeSymbols
{
    public SeleniumTypeSymbols(Compilation compilation)
        : base(compilation)
    {
        View = compilation.GetTypeByMetadataName(Names.View)!;

        HasAllSymbols = HasAllSymbols && View is not null;
    }

    public INamedTypeSymbol View { get; } = null!;

    protected override string LocationKeyTypeName { get; } = Names.LocationKeyType;
    protected override string UIMapBaseClassTypeName { get; } = Names.UIMapBaseClassType;
    protected override string ControlBaseTypeName { get; } = Names.ControlBaseType;

    internal static class Names
    {
        internal const string LocationKeyType = "Rocketcress.Selenium.By";
        internal const string UIMapBaseClassType = "Rocketcress.Selenium.Base.WebElementContainer";
        internal const string ControlBaseType = "Rocketcress.Selenium.Controls.WebElement";
        internal const string View = "Rocketcress.Selenium.View";
    }
}
