using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Syntax.Operators.Math
{
    public class Divides : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return typeA == MiniType.Double || typeB == MiniType.Double ? MiniType.Double : MiniType.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}