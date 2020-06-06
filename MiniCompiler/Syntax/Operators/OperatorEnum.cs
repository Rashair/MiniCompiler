namespace MiniCompiler.Syntax.Operators
{
    public enum OperatorEnum
    {
        Unknown     = 0,
        Assign      = Token.Assign,
        UnaryMinus  = Token.Minus * 100,
        Minus       = Token.Minus,
    }
}
