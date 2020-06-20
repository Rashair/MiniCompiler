using MiniCompiler.Syntax.Variables;
using System;

namespace MiniCompiler.Syntax.Operators
{
    public abstract class BinaryOperator : Operator
    {
        public override bool CanUse(MiniType typeA)
        {
            return false;
        }

        public override MiniType GetResultType(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            if (typeA == MiniType.Unknown || typeB == MiniType.Unknown)
            {
                throw new ArgumentException("You can't use this operator on this types.");
            }

            return GetResultTypeBinary(typeA, typeB);
        }

        public abstract MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB);
    }
}