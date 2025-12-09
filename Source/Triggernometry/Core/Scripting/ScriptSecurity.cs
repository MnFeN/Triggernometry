using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;
using System.Linq;
using static Triggernometry.Core.Configuration;

namespace Triggernometry.Core.Scripting
{
    internal static class ScriptSecurity
    {

        internal readonly static List<string> SecurityAPIs = new List<string>
        {
            "Microsoft.CodeAnalysis",
            "Microsoft.Win32",
            "System.CodeDom.Compiler",
            "System.Diagnostics",
            "System.IO",
            "System.Net",
            "System.Reflection",
            "System.Runtime",
            "System.Security",
            "System.Web",
            "Triggernometry.Utilities"
        };

        /// <summary> For features only (unsafe, dynamic...) </summary>
        internal static bool IsFeatureAllowedByConfig(ScriptUsageEnum usage, Context ctx)
        {
            bool isRemote = ctx.Trigger?.Repo != null;
            bool isAdmin = ctx.Plugin.isRunningAsAdmin;

            if (isAdmin && !usage.HasFlag(ScriptUsageEnum.AllowAdmin)) return false;
            if (!isRemote && !usage.HasFlag(ScriptUsageEnum.AllowLocal)) return false;
            if (isRemote && !usage.HasFlag(ScriptUsageEnum.AllowRemote)) return false;

            return true;
        }

        internal static string[] GetRestrictedApisFromConfig(Context ctx)
        {
            bool isRemote = ctx.Trigger?.Repo != null;
            bool isAdmin = ctx.Plugin.isRunningAsAdmin;
            return ctx.Plugin.cfg.GetAPIUsages().Where(a =>
                isAdmin && !a.AllowAdmin ||
                !isRemote && !a.AllowLocal ||
                isRemote && !a.AllowRemote
            ).Select(a => a.Name).ToArray();
        }

        /// <summary>
        /// Validates the script against a list of restricted APIs. <br /><br /> 
        /// Examines using directives, declared variables, invocations, method declarations, and property types, <br />
        /// to detect usage of any namespace or assembly listed in <paramref name="restrictedApis"/>.
        /// </summary>
        internal static bool TryGetViolatingApi(Script script, out string violatingApi, params string[] restrictedApis)
        {
            var comp = script.GetCompilation();
            var syntaxTree = comp.SyntaxTrees.First();
            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            var model = comp.GetSemanticModel(syntaxTree);

            violatingApi = TryGetViolatingApiFromUsings(root, model, restrictedApis)
                        ?? TryGetViolatingApiFromVariables(root, model, restrictedApis)
                        ?? TryGetViolatingApiFromInvocations(root, model, restrictedApis)
                        ?? TryGetViolatingApiFromMethods(root, model, restrictedApis)
                        ?? TryGetViolatingApiFromProperties(root, model, restrictedApis);

            return violatingApi != null;
        }

        /// <returns>Violating API name, or <see langword="null" /> if not found. </returns>
        private static string TryGetViolatingApiFromUsings(CompilationUnitSyntax root, SemanticModel model, string[] restrictedApis)
        {
            foreach (UsingDirectiveSyntax usingdir in root.Usings)
            {
                ISymbol symbol = model.GetSymbolInfo(usingdir.Name).Symbol;
                string name = symbol?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (IsApiRestricted(name, restrictedApis)) return name;
            }
            return null;
        }

        /// <returns>Violating API name, or <see langword="null" /> if not found. </returns>
        private static string TryGetViolatingApiFromVariables(CompilationUnitSyntax root, SemanticModel model, string[] restrictedApis)
        {
            var variables = root.DescendantNodes().OfType<VariableDeclaratorSyntax>();

            foreach (var variable in variables)
            {
                ISymbol symbol = model.GetDeclaredSymbol(variable);
                ITypeSymbol type = null;
                switch (symbol?.Kind)
                {
                    case SymbolKind.Field:
                        type = ((IFieldSymbol)symbol).Type; break;
                    case SymbolKind.Local:
                        type = ((ILocalSymbol)symbol).Type; break;
                }
                string name = type?.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (IsApiRestricted(name, restrictedApis)) return name;
            }
            return null;
        }

        /// <returns>Violating API name, or <see langword="null" /> if not found. </returns>
        private static string TryGetViolatingApiFromInvocations(CompilationUnitSyntax root, SemanticModel model, string[] restrictedApis)
        {
            var invocs = root.DescendantNodes().OfType<InvocationExpressionSyntax>();

            foreach (var invoc in invocs)
            {
                ISymbol symbol = model.GetSymbolInfo(invoc).Symbol;
                string name = symbol?.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                if (IsApiRestricted(name, restrictedApis)) return name;
            }
            return null;
        }

        /// <returns>Violating API name, or <see langword="null" /> if not found. </returns>
        private static string TryGetViolatingApiFromMethods(CompilationUnitSyntax root, SemanticModel model, string[] restrictedApis)
        {
            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methods)
            {
                var symbol = model.GetDeclaredSymbol(method);
                if (!(symbol is IMethodSymbol methodSymbol)) continue;

                string name = methodSymbol.ContainingType?.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                if (IsApiRestricted(name, restrictedApis)) return name;
            }
            return null;
        }

        /// <returns>Violating API name, or <see langword="null" /> if not found. </returns>
        private static string TryGetViolatingApiFromProperties(CompilationUnitSyntax root, SemanticModel model, string[] restrictedApis)
        {
            var props = root.DescendantNodes().OfType<PropertyDeclarationSyntax>();

            foreach (var prop in props)
            {
                var symbol = model.GetDeclaredSymbol(prop);
                if (!(symbol is IPropertySymbol propSymbol) || propSymbol.Type == null) continue;

                string name = propSymbol.Type.ContainingAssembly?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (IsApiRestricted(name, restrictedApis)) return name;

                var nameSpace = propSymbol.Type.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (IsApiRestricted(nameSpace, restrictedApis)) return nameSpace;
            }
            return null;
        }

        private static bool IsApiRestricted(string apiNameToCheck, params string[] restrictedApis)
        {
            if (apiNameToCheck == null || restrictedApis == null || restrictedApis.Length == 0)
                return false;

            return restrictedApis.Any(restrictedApi => apiNameToCheck.Contains(restrictedApi));
        }

        /// <summary>
        /// Determines whether the script uses dynamic typing. <br />
        /// Checks for expressions whose type is dynamic,  
        /// member access on a dynamic receiver,  
        /// or invocation of a dynamic target.
        /// </summary>
        internal static bool ContainsDynamic(Script script)
        {
            var comp = script.GetCompilation();
            var syntaxTree = comp.SyntaxTrees.First();
            var model = comp.GetSemanticModel(syntaxTree);
            var root = syntaxTree.GetRoot();
            return root.DescendantNodes().OfType<ExpressionSyntax>()
                .Any(expr => ContainsDynamic(model, expr));
        }

        private static bool ContainsDynamic(SemanticModel model, ExpressionSyntax exprSyntax)
        {
            if (model == null || exprSyntax == null) return false;

            // check dynamic expressions
            List<ExpressionSyntax> exprSyntaxesToCheck = new List<ExpressionSyntax> { exprSyntax };

            // check dynamic.X
            if (exprSyntax is MemberAccessExpressionSyntax ma)
                exprSyntaxesToCheck.Add(ma.Expression);

            // check MethodReturningDynamic()
            if (exprSyntax is InvocationExpressionSyntax inv)
                exprSyntaxesToCheck.Add(inv.Expression);

            return exprSyntaxesToCheck.Any(expr => {
                var typeInfo = model.GetTypeInfo(expr);
                var type = typeInfo.Type ?? typeInfo.ConvertedType;
                return type?.TypeKind == TypeKind.Dynamic;
            });
        }

    }
}
