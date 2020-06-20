using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Syntax.Operators.Unary
{
    public class DoubleCast : UnaryOperator
    {
        public override bool CanUse(MiniType typeA)
        {
            return typeA != MiniType.Unknown;
        }

        public override MiniType GetResultType(MiniType type)
        {
            return MiniType.Double;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}