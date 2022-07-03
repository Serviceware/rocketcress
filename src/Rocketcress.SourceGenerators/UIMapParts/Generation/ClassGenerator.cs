using Rocketcress.SourceGenerators.Common;

namespace Rocketcress.SourceGenerators.UIMapParts.Generation
{
    internal readonly struct ClassGenerator
    {
        private readonly SourceBuilder _builder;

        public ClassGenerator(SourceBuilder builder)
        {
            _builder = builder;
        }

        public static ClassGenerator Generate(SourceBuilder builder) => new(builder);

        public void UIMapParts()
        {

        }
    }
}
