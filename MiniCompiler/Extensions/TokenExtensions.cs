using MiniCompiler.Syntax.Operators;

namespace MiniCompiler.Extensions
{
    public static class TokenExtensions
    {
        public const int UnaryMultipler = 100;

        public static Type ConvertToType(this Token token)
        {
            switch (token)
            {
                case Token.DoubleKey:
                case Token.DoubleVal:
                    return Type.Double;

                case Token.IntKey:
                case Token.IntVal:
                    return Type.Int;

                case Token.BoolKey:
                case Token.True:
                case Token.False:
                    return Type.Bool;
            }

            return Type.Unknown;
        }

        public static OperatorEnum ConvertToOperator(this Token token, bool unary = false)
        {
            if (unary)
            {
                switch (token)
                {
                    case Token.Minus:
                        return OperatorEnum.UnaryMinus;
                }
            }

            return (OperatorEnum)token;
        }
    }
}
