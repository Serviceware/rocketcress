namespace Rocketcress.SourceGenerators
{
    internal static partial class DiagnosticFactory
    {
        private const string Category = "Rocketcress.Generators";

        private static string GetId(int id) => $"RCG{id:0000}";
    }
}
