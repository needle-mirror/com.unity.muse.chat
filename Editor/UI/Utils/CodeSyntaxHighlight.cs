using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Unity.Muse.Chat
{
    internal static class CodeSyntaxHighlight
    {
        internal const string k_ColorAzure = "#7893e5";
        internal const string k_ColorLavender = "#bc92f9";
        internal const string k_ColorTurquoise = "#68c99e";
        internal const string k_ColorSand = "#c2a473";
        internal const string k_ColorLime = "#91c275";

        public static string Highlight(string sourceCode)
        {
            var tree = CSharpSyntaxTree.ParseText(sourceCode);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var formattedCode = new StringBuilder();

            foreach (var token in root.DescendantTokens())
            {
                foreach (var trivia in token.LeadingTrivia)
                {
                    formattedCode.Append(ProcessTrivia(trivia));
                }

                formattedCode.Append(ProcessToken(token));

                foreach (var trivia in token.TrailingTrivia)
                {
                    formattedCode.Append(ProcessTrivia(trivia));
                }
            }

            return formattedCode.ToString();
        }

        static string ProcessToken(SyntaxToken token)
        {
            var tokenText = token.Text;
            var kind = token.Kind();

            if (SyntaxFacts.IsKeywordKind(kind))
                return tokenText.RichColor(k_ColorAzure);
            if (IsTypeIdentifier(token) || IsMethodReturnType(token) || IsClassDeclaration(token))
                return tokenText.RichColor(k_ColorLavender);
            if (IsMethodDeclaration(token))
                return tokenText.RichColor(k_ColorTurquoise);
            if (kind is SyntaxKind.StringLiteralToken or SyntaxKind.InterpolatedStringTextToken)
                return tokenText.RichColor(k_ColorSand);

            return tokenText;
        }

        static string ProcessTrivia(SyntaxTrivia trivia)
        {
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) || trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
                return trivia.ToFullString().RichColor(k_ColorLime);

            return trivia.ToFullString();
        }

        static bool IsClassDeclaration(SyntaxToken token)
        {
            return token.Parent is ClassDeclarationSyntax classNode && token == classNode.Identifier;
        }

        static bool IsMethodDeclaration(SyntaxToken token)
        {
            return token.Parent is MethodDeclarationSyntax methodNode && token == methodNode.Identifier;
        }

        static bool IsTypeIdentifier(SyntaxToken token)
        {
            return token.Parent is IdentifierNameSyntax identifierName &&
                   identifierName.Parent is VariableDeclarationSyntax;
        }

        static bool IsMethodReturnType(SyntaxToken token) =>
            token.Parent is PredefinedTypeSyntax or IdentifierNameSyntax
            && token.Parent.Parent is MethodDeclarationSyntax methodDeclaration
            && methodDeclaration.ReturnType == token.Parent;
    }
}
