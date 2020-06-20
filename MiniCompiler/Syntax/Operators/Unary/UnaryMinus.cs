using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Syntax.Operators.Unary
{
    public class UnaryMinus : UnaryOperator
    {
        public override bool CanUse(MiniType typeA)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return true;

                case MiniType.Double:
                    return true;
            }

            return false;
        }

        public override MiniType GetResultType(MiniType type)
        {
            return type;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}