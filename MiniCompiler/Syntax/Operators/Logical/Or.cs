using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Syntax.Operators.Logical
{
    public class Or : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            return typeA == MiniType.Bool && typeB == MiniType.Bool;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}