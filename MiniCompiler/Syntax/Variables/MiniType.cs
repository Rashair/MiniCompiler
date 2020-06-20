namespace MiniCompiler.Syntax.Variables
{
    public enum MiniType
    {
        Unknown = 0,
        Int = Token.IntKey,
        Bool = Token.BoolKey,
        Double = Token.DoubleKey,
        String = Token.String * -1,
    }
}