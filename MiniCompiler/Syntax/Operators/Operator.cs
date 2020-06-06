using MiniCompiler;
using MiniCompiler.Syntax.Operators;
using MiniCompiler.Syntax.Operators.Assignment;
using MiniCompiler.Syntax.Operators.Unary;

public abstract class Operator
{
    public OperatorEnum Token { get; set; }

    public abstract bool CanUse(Type typeA);

    public abstract bool CanUse(Type typeA, Type typeB);

    public static Operator Create(OperatorEnum token)
    {
        var result = CreateFromToken(token);
        result.Token = token;

        return result;
    }

    private static Operator CreateFromToken(OperatorEnum token)
    {
        switch (token)
        {
            case OperatorEnum.Assign:
                return new Assign();

            case OperatorEnum.UnaryMinus:
                return new UnaryMinus();
        }

        return new Unknown();
    }
}