using MiniCompiler.Extensions;
using MiniCompiler.Syntax.Abstract;
using MiniCompiler.Syntax.Variables;
using QUT.Gppg;


namespace MiniCompiler.Syntax.Operators
{
    public abstract class Operator : TypeNode
    {
        protected Operator()
        {
        }

        public OperatorEnum Token { get; private set; }

        public abstract bool CanUse(MiniType typeA);

        public abstract bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown);

        public abstract MiniType GetResultType(MiniType typeA, MiniType typeB = MiniType.Unknown);

        public Operator WithResultType(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            Type = GetResultType(typeA, typeB);
            return this;
        }

        public static bool CanUse(Token token, MiniType typeA, MiniType typeB) => Factory.CanUse(token, typeA, typeB);

        public static bool CanUse(Token token, MiniType typeA) => Factory.CanUse(token, typeA);

        public static Operator Create(Token token, MiniType typeA, MiniType typeB, LexLocation location = null) =>
            Factory.Create(token, typeA, typeB, location);

        public static Operator Create(Token token, MiniType typeA, LexLocation location = null) =>
            Factory.Create(token, typeA, location);

        public static class Factory
        {
            private static OperatorEnum lastToken;
            private static Operator lastOperator;

            public static bool CanUse(Token token, MiniType typeA, MiniType typeB)
            {
                lastToken = token.ConvertToOperator();
                lastOperator = CreateFromToken(lastToken);
                return lastOperator.CanUse(typeA, typeB);
            }

            public static bool CanUse(Token token, MiniType typeA)
            {
                lastToken = token.ConvertToOperator(true);
                lastOperator = CreateFromToken(lastToken);
                return lastOperator.CanUse(typeA);
            }

            private static Operator CreateFromToken(OperatorEnum op)
            {
                return op.CreateOperator();
            }

            public static Operator Create(Token token, MiniType typeA, MiniType typeB, LexLocation location = null)
            {
                var operatorToken = token.ConvertToOperator();
                return Create(operatorToken, location).WithResultType(typeA, typeB);
            }

            public static Operator Create(Token token, MiniType typeA, LexLocation location = null)
            {
                var operatorToken = token.ConvertToOperator(true);
                return Create(operatorToken, location).WithResultType(typeA);
            }

            private static Operator Create(OperatorEnum op, LexLocation location)
            {
                Operator result;
                if (op == lastToken)
                {
                    result = lastOperator;
                    lastToken = OperatorEnum.Unknown;
                    lastOperator = null;
                }
                else
                {
                    result = CreateFromToken(op);
                }

                result.Token = op;
                result.Location = location;

                return result;
            }
        }
    }
}