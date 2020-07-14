// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.LayoutRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Settings.ObjectModel;

    /// <summary>
    /// The opening and closing braces for a multi-line C# statement have been omitted.
    /// </summary>
    /// <remarks>
    /// <para>A violation of this rule occurs when the opening and closing braces for a multi-line statement have been
    /// omitted. In C#, some types of statements may optionally include braces. Examples include <c>if</c>,
    /// <c>while</c>, and <c>for</c> statements. For example, an if-statement may be written without braces:</para>
    ///
    /// <code language="csharp">
    /// if (true)
    ///     return
    ///         this.value;
    /// </code>
    ///
    /// <para>Although this is legal in C#, StyleCop requires the braces to be present when the statement spans multiple
    /// lines, to increase the readability and maintainability of the code.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1521UseLineEndingingsConsistently : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1521UseLineEndingingsConsistently"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1521";

        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1521.md";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(LayoutResources.SA1521Title), LayoutResources.ResourceManager, typeof(LayoutResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(LayoutResources.SA1521MessageFormat), LayoutResources.ResourceManager, typeof(LayoutResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(LayoutResources.SA1521Description), LayoutResources.ResourceManager, typeof(LayoutResources));
        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.MaintainabilityRules, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledByDefault, Description, HelpLink);

        private static readonly Action<SyntaxNodeAnalysisContext, StyleCopSettings> EndOfLineAction = HandleEndOfLineTrivia;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(EndOfLineAction, SyntaxKind.EndOfLineTrivia);
        }

        private static void HandleEndOfLineTrivia(SyntaxNodeAnalysisContext context, StyleCopSettings settings)
        {
            var endOfLine = context.Node.Span.ToString();

            if (endOfLine != settings.LayoutRules.LineEnding)
            {
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, context.Node.GetLocation()));
            }
        }
    }
}
