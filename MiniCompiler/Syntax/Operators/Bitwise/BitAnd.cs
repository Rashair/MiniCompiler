using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Syntax.Operators.Bitwise
{
    public class BitAnd : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            return typeA == MiniType.Int && typeB == MiniType.Int;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}