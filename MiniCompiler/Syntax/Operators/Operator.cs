using MiniCompiler;
using MiniCompiler.Syntax.Abstract;
using MiniCompiler.Syntax.Operators;
using MiniCompiler.Syntax.Operators.Assignment;
using MiniCompiler.Syntax.Operators.Unary;
using MiniCompiler.Syntax.Variables;
using QUT.Gppg;

public abstract class Operator : TypeNode
{
    protected Operator() { }

    public OperatorEnum Token { get; private set; }

    public abstract bool CanUse(Type typeA);

    public abstract bool CanUse(Type typeA, Type typeB = Type.Unknown);

    public abstract Type GetResultType(Type typeA, Type typeB = Type.Unknown);

    public static bool CanUse(Token token, Type typeA, Type? typeB = null) => Factory.CanUse(token, typeA, typeB);

    public static Operator Create(Token token, Type typeA, Type typeB = Type.Unknown, LexLocation location = null) =>
        Factory.Create(token, typeA, typeB, location);

    public static class Factory
    {
        private static OperatorEnum lastToken;
        private static Operator lastOperator;

        public static bool CanUse(Token token, Type typeA, Type? typeB = null)
        {
            lastToken = (OperatorEnum)token;
            lastOperator = CreateFromToken(lastToken);
            return typeB.HasValue ?
                lastOperator.CanUse(typeA, typeB.Value) :
                lastOperator.CanUse(typeA);
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

        public static Operator Create(Token token, Type typeA, Type typeB = Type.Unknown, LexLocation location = null)
        {
            Operator result;
            var operatorToken = (OperatorEnum)token;
            if (operatorToken == lastToken)
            {
                result = lastOperator;
                lastToken = OperatorEnum.Unknown;
                lastOperator = null;
            }
            else
            {
                result = CreateFromToken(operatorToken);
            }

            result.Token = operatorToken;
            result.Location = location;
            result.Type = result.GetResultType(typeA, typeB);

            return result;
        }
    }
}