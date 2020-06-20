using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Syntax.Operators.Assignment
{
    public class Assign : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB)
        {
            switch (typeA)
            {
                case MiniType.Double:
                    return typeB == MiniType.Double || typeB == MiniType.Int;
            }

            return typeA == typeB;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return typeA;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}