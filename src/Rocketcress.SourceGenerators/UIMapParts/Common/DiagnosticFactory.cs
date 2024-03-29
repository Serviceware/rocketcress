﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.UIMapParts.Selenium;
using Rocketcress.SourceGenerators.UIMapParts.UIAutomation;
using System.Collections.Immutable;

namespace Rocketcress.SourceGenerators;

internal static partial class DiagnosticFactory
{
    public static class UIMapParts
    {
        private const string Category = "Rocketcress.UIMapPartsGenerator";

        private static readonly DiagnosticDescriptor ClassMustBePartialDescriptor = new(
            GetId(1),
            "Classes using the GenerateUIMapParts attribute must be partial",
            "The class '{0}' has the GenerateUIMapParts attribute but is not partial. For the source generator to work properly, the class must be partial.",
            Category,
            DiagnosticSeverity.Error,
            true);

        private static readonly DiagnosticDescriptor AllParentTypesMustBePartialDescriptor = new(
            GetId(2),
            "Classes containing nested classes using the GenerateUIMapParts attribute must be partial",
            "The class '{0}' is not partial but has a nested class '{1}' which has the GenerateUIMapParts attribute. For the source generator to work properly, all parent classes of '{1}' must be partial.",
            Category,
            DiagnosticSeverity.Error,
            true);

        private static readonly DiagnosticDescriptor BadBaseTypeDescriptor = new(
            GetId(3),
            "Classes using the GenerateUIMapParts attribute must derive from a supported type",
            $"The type '{{0}}' has the GenerateUIMapParts attribute but it does not derive from a supported base type. Supported base types are '{SeleniumTypeSymbols.Names.UIMapBaseClassType}' and '{UIAutomationTypeSymbols.Names.UIMapBaseClassType}'.",
            Category,
            DiagnosticSeverity.Error,
            true);

        private static readonly DiagnosticDescriptor MissingParentControlDescriptor = new(
            GetId(4),
            "Defined ParentControl on UIMapControl attributes must exist as property in same class.",
            "The property '{0}' of class '{1}' has the UIMapControl attribute and defines the non-existant property '{2}' as its parent.",
            Category,
            DiagnosticSeverity.Error,
            true);

        private static readonly DiagnosticDescriptor ParentLoopDescriptor = new(
            GetId(5),
            "Defined ParentControl on UIMapControl attributes should not result in a loop.",
            "ParentControl loop detected for property '{0}' of class '{1}': {2}",
            Category,
            DiagnosticSeverity.Error,
            true);

        private static readonly DiagnosticDescriptor ErrorDuringGenerationDescriptor = new(
            GetId(99),
            "An error occured during generation of UI map parts.",
            "An error occured during generation of UI map parts: {0}",
            Category,
            DiagnosticSeverity.Error,
            true);

        public static ImmutableArray<DiagnosticDescriptor> AllDescriptors
        {
            get
            {
                var builder = ImmutableArray.CreateBuilder<DiagnosticDescriptor>(4);
                builder.Add(ClassMustBePartialDescriptor);
                builder.Add(AllParentTypesMustBePartialDescriptor);
                builder.Add(BadBaseTypeDescriptor);
                builder.Add(MissingParentControlDescriptor);
                builder.Add(ParentLoopDescriptor);
                return builder.ToImmutable();
            }
        }

        public static Diagnostic ClassMustBePartial(ClassDeclarationSyntax classDeclarationSyntax)
            => Diagnostic.Create(
                ClassMustBePartialDescriptor,
                classDeclarationSyntax.Identifier.GetLocation(),
                classDeclarationSyntax.Identifier.ValueText);

        public static Diagnostic AllParentTypesMustBePartial(ClassDeclarationSyntax uimapClassDeclarationSyntax, ClassDeclarationSyntax nonPartialParentClassDeclarationSyntax)
            => Diagnostic.Create(
                AllParentTypesMustBePartialDescriptor,
                nonPartialParentClassDeclarationSyntax.Identifier.GetLocation(),
                nonPartialParentClassDeclarationSyntax.Identifier.ValueText,
                uimapClassDeclarationSyntax.Identifier.ValueText);

        public static Diagnostic BadBaseType(ClassDeclarationSyntax classDeclarationSyntax)
            => Diagnostic.Create(
                BadBaseTypeDescriptor,
                classDeclarationSyntax.Identifier.GetLocation(),
                classDeclarationSyntax.Identifier.ValueText);

        public static Diagnostic MissingParentControl(Location? location, string className, string propertyName, string parentName)
            => Diagnostic.Create(MissingParentControlDescriptor, location, propertyName, className, parentName);

        public static Diagnostic ParentLoop(Location? location, string className, string propertyName, IEnumerable<string> loopMembers)
            => Diagnostic.Create(ParentLoopDescriptor, location, propertyName, className, string.Join(" -> ", loopMembers));

        public static Diagnostic ErrorDuringGeneration(Location? location, string message)
            => Diagnostic.Create(ErrorDuringGenerationDescriptor, location, message);

        private static string GetId(int id) => DiagnosticFactory.GetId(100 + id);
    }
}
