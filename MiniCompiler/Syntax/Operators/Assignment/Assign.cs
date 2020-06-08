namespace MiniCompiler.Syntax.Operators.Assignment
{
    public class Assign : BinaryOperator
    {
        public override bool CanUse(Type typeA, Type typeB)
        {
            switch (typeA)
            {
                case Type.Double:
                    return typeB == Type.Double || typeB == Type.Int;
            }

            return typeA == typeB;
        }

        public override Type GetResultTypeBinary(Type typeA, Type typeB)
        {
            return typeA;
        }
    }
}