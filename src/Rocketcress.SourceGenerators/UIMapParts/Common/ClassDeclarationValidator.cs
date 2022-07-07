using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.Extensions;

namespace Rocketcress.SourceGenerators.UIMapParts.Common
{
    internal readonly struct ClassDeclarationValidator
    {
        private readonly ClassDeclarationSyntax _classDeclaration;
        private readonly SemanticModel _semanticModel;
        private readonly Action<Diagnostic>? _reportDiagnosic;

        private ClassDeclarationValidator(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, Action<Diagnostic>? reportDiagnosic)
        {
            _classDeclaration = classDeclaration;
            _semanticModel = semanticModel;
            _reportDiagnosic = reportDiagnosic;
        }

        public static ClassDeclarationValidator Validate(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
            => new(classDeclaration, semanticModel, null);

        public static ClassDeclarationValidator Create(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, Action<Diagnostic> reportDiagnosic)
            => new(classDeclaration, semanticModel, reportDiagnosic);

        public bool HasGenerateUIMapPartsAttribute()
            => _classDeclaration.TryGetAttributeSyntax(_semanticModel, TypeSymbols.Names.GenerateUIMapPartsAttribute, out _);

        public bool HasDebugGeneratorAttribute()
            => _classDeclaration.TryGetAttributeSyntax(_semanticModel, TypeSymbols.Names.DebugGeneratorAttribute, out _);

        public bool IsNamedTypeSymbol(out INamedTypeSymbol typeSymbol)
            => (typeSymbol = RetrieveTypeSymbol()!) is not null;

        public bool IsPartial()
        {
            if (!_classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                TryReportClassMustBePartial();
                return false;
            }

            var parent = _classDeclaration.Parent;
            while (parent is ClassDeclarationSyntax parentClassDeclaration)
            {
                if (!parentClassDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
                {
                    TryReportAllParentTypesMustBePartial(parentClassDeclaration);
                    return false;
                }

                parent = parentClassDeclaration.Parent;
            }

            return true;
        }

        private void TryReportClassMustBePartial()
        {
            if (_reportDiagnosic is null)
                return;

            var location = _classDeclaration.Identifier.GetLocation();
            var typeName = _classDeclaration.Identifier.ValueText;
            _reportDiagnosic(DiagnosticFactory.UIMapParts.ClassMustBePartial(location, typeName));
        }

        private void TryReportAllParentTypesMustBePartial(ClassDeclarationSyntax nonPartialParentClassDeclaration)
        {
            if (_reportDiagnosic is null)
                return;

            var location = nonPartialParentClassDeclaration.Identifier.GetLocation();
            var uimapTypeName = _classDeclaration.Identifier.ValueText;
            var nonPartialTypeName = nonPartialParentClassDeclaration.Identifier.ValueText;
            _reportDiagnosic(DiagnosticFactory.UIMapParts.AllParentTypesMustBePartial(location, uimapTypeName, nonPartialTypeName));
        }

        private INamedTypeSymbol? RetrieveTypeSymbol() => _semanticModel.GetDeclaredSymbol(_classDeclaration) as INamedTypeSymbol;
    }
}
