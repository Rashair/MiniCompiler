namespace MiniCompiler.Syntax.Operators.Bitwise
{
    public class BitOr : BinaryOperator
    {
        public override bool CanUse(Type typeA, Type typeB = Type.Unknown)
        {
            switch (typeA)
            {
                case Type.Int:
                    return typeB == Type.Int;
            }

            return false;
        }

        public override Type GetResultTypeBinary(Type typeA, Type typeB)
        {
            return Type.Int;
        }
    }
}