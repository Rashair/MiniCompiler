using MiniCompiler.Extensions;

namespace MiniCompiler.Syntax.Operators
{
    public enum OperatorEnum
    {
        Unknown         = 0,
        UnaryMinus      = Token.Minus * TokenExtensions.UnaryMultipler,
        BitNegation     = Token.BitNegation,
        LogicNegation   = Token.Negation,
        IntCast         = Token.IntKey,
        DoubleCast      = Token.DoubleKey,
        BitOr           = Token.BitOr,
        BitAnd          = Token.BitAnd,
        Multiplies      = Token.Multiplies,
        Divides         = Token.Divides,
        Add             = Token.Add,
        Subtract        = Token.Minus,
        Equals          = Token.Equals,
        NotEquals       = Token.NotEquals,
        Greater         = Token.Greater,
        GreaterOrEqual  = Token.GreaterOrEqual,
        Less            = Token.Less,
        LessOrEqual     = Token.LessOrEqual,
        Or              = Token.Or,
        And             = Token.And,
        Assign          = Token.Assign,
    }
}