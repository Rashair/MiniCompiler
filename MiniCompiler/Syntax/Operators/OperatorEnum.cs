using MiniCompiler.Extensions;

namespace MiniCompiler.Syntax.Operators
{
    public enum OperatorEnum
    {
        Unknown = 0,
        Assign = Token.Assign,
        UnaryMinus = Token.Minus * TokenExtensions.UnaryMultipler,
        Minus = Token.Minus,
    }
}