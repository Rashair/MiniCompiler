using MiniCompiler.Syntax;
using MiniCompiler.Syntax.Abstract;

namespace MiniCompiler.Extensions
{
    public static class SiblingsNodeExtensions
    {
        public static TObj WithLeft<TObj, TChild>(this TObj obj, TChild left)
            where TObj : SiblingsNode<TChild, TChild>
            where TChild : SyntaxNode
        {
            obj.Left = left;
            return obj;
        }

        public static TObj WithRight<TObj, TChild>(this TObj obj, TChild right)
            where TObj : SiblingsNode<TChild, TChild>
            where TChild : SyntaxNode
        {
            obj.Right = right;
            return obj;
        }
    }
}