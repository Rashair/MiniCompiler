using MiniCompiler.Syntax.Variables;
using System;

namespace MiniCompiler.Syntax.Operators
{
    public abstract class UnaryOperator : Operator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB)
        {
            return false;
        }

        public override MiniType GetResultType(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            if (typeB != MiniType.Unknown || typeA == MiniType.Unknown)
            {
                throw new ArgumentException("You can't use this operator on this types.");
            }

            return GetResultType(typeA);
        }

        public abstract MiniType GetResultType(MiniType type);
    }
}