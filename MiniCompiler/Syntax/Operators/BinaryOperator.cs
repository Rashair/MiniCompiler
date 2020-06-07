using System;

namespace MiniCompiler.Syntax.Operators
{
    public abstract class BinaryOperator : Operator
    {
        public override bool CanUse(Type typeA)
        {
            return false;
        }

        public override Type GetResultType(Type typeA, Type typeB = Type.Unknown)
        {
            if (typeA == Type.Unknown || typeB == Type.Unknown)
            {
                throw new ArgumentException("You can't use this operator on this types.");
            }

            return GetResultTypeBinary(typeA, typeB);
        }

        public abstract Type GetResultTypeBinary(Type typeA, Type typeB);
    }
}