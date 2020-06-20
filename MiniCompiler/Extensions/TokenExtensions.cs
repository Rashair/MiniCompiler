using MiniCompiler.Syntax.Operators;
using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Extensions
{
    public static class TokenExtensions
    {
        public const int UnaryMultipler = 100;

        public static MiniType ConvertToType(this Token token)
        {
            switch (token)
            {
                case Token.DoubleKey:
                case Token.DoubleVal:
                    return MiniType.Double;

                case Token.IntKey:
                case Token.IntVal:
                    return MiniType.Int;

                case Token.BoolKey:
                case Token.True:
                case Token.False:
                    return MiniType.Bool;
            }

            return MiniType.Unknown;
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