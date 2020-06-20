using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Syntax.Operators
{
    public class UnknownOperator : Operator
    {
        public override bool CanUse(MiniType typeA)
        {
            return false;
        }

        public override bool CanUse(MiniType typeA, MiniType typeB)
        {
            return false;
        }

        public override MiniType GetResultType(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            return MiniType.Unknown;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
        }
    }
}