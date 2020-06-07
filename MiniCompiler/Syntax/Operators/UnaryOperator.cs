using System;

namespace MiniCompiler.Syntax.Operators
{
    public abstract class UnaryOperator : Operator
    {
        public override bool CanUse(Type typeA, Type typeB)
        {
            return false;
        }

        public override Type GetResultType(Type typeA, Type typeB = Type.Unknown)
        {
            if (typeB != Type.Unknown || typeA == Type.Unknown)
            {
                throw new ArgumentException("You can't use this operator on this types.");
            }

            return GetResultType(typeA);
        }

        public abstract Type GetResultType(Type type);
    }
}