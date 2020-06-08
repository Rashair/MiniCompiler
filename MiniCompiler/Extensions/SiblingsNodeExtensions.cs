using MiniCompiler.Syntax;
using MiniCompiler.Syntax.Abstract;

namespace MiniCompiler.Extensions
{
    public static class SiblingsNodeExtensions
    {
        public static T WithLeft<T>(this T obj, SyntaxNode left)
            where T : SiblingsNode
        {
            obj.Left = left;
            return obj;
        }

        public static T WithRight<T>(this T obj, SyntaxNode right)
            where T : SiblingsNode
        {
            obj.Right = right;
            return obj;
        }
    }
}