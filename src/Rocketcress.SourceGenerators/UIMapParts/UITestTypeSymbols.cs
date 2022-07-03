using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.UIMapParts
{
    internal abstract class UITestTypeSymbols
    {
        protected UITestTypeSymbols(Compilation compilation)
        {
            LocationKeyType = compilation.GetTypeByMetadataName(LocationKeyTypeName)!;
            UIMapBaseClassType = compilation.GetTypeByMetadataName(UIMapBaseClassTypeName)!;
            ControlBaseType = compilation.GetTypeByMetadataName(ControlBaseTypeName)!;

            HasAllSymbols =
                LocationKeyTypeName is not null &&
                UIMapBaseClassTypeName is not null &&
                ControlBaseTypeName is not null;
        }

        public INamedTypeSymbol LocationKeyType { get; }
        public INamedTypeSymbol UIMapBaseClassType { get; }
        public INamedTypeSymbol ControlBaseType { get; }

        public bool HasAllSymbols { get; protected set; }

        protected abstract string LocationKeyTypeName { get; }
        protected abstract string UIMapBaseClassTypeName { get; }
        protected abstract string ControlBaseTypeName { get; }
    }
}
