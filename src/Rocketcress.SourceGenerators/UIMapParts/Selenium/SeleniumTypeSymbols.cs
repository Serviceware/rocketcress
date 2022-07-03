using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.UIMapParts.Selenium
{
    internal class SeleniumTypeSymbols : UITestTypeSymbols
    {
        public SeleniumTypeSymbols(Compilation compilation)
            : base(compilation)
        {
        }

        protected override string LocationKeyTypeName { get; } = Names.LocationKeyType;
        protected override string UIMapBaseClassTypeName { get; } = Names.UIMapBaseClassType;
        protected override string ControlBaseTypeName { get; } = Names.ControlBaseType;

        internal static class Names
        {
            internal const string LocationKeyType = "Rocketcress.Selenium.By";
            internal const string UIMapBaseClassType = "Rocketcress.Selenium.Base.WebElementContainer";
            internal const string ControlBaseType = "Rocketcress.Selenium.Controls.WebElement";
        }
    }
}
