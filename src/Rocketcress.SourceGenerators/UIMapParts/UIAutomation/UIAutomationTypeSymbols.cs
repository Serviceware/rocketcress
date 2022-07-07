using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.UIMapParts.Common;

namespace Rocketcress.SourceGenerators.UIMapParts.UIAutomation
{
    internal class UIAutomationTypeSymbols : UITestTypeSymbols
    {
        public UIAutomationTypeSymbols(Compilation compilation)
            : base(compilation)
        {
        }

        protected override string LocationKeyTypeName { get; } = Names.LocationKeyType;
        protected override string UIMapBaseClassTypeName { get; } = Names.UIMapBaseClassType;
        protected override string ControlBaseTypeName { get; } = Names.ControlBaseType;

        internal static class Names
        {
            internal const string LocationKeyType = "Rocketcress.UIAutomation.By";
            internal const string UIMapBaseClassType = "Rocketcress.UIAutomation.Controls.UITestControl";
            internal const string ControlBaseType = "Rocketcress.UIAutomation.Controls.IUITestControl";
        }
    }
}
