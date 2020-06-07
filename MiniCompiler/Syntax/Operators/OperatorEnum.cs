using MiniCompiler.Extensions;

namespace MiniCompiler.Syntax.Operators
{
    public enum OperatorEnum
    {
        Unknown = 0,
        UnaryMinus = Token.Minus * TokenExtensions.UnaryMultipler,
        BitNegation = Token.BitNegation,
        LogicNegation = Token.Negation,
        IntCast = Token.IntKey,
        DoubleCast = Token.DoubleKey,
        Minus = Token.Minus,
        Assign = Token.Assign,
    }
}