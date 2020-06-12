namespace MiniCompiler.Syntax.Operators.Relation
{
    public class LessOrEqual : BinaryOperator
    {
        public override bool CanUse(Type typeA, Type typeB = Type.Unknown)
        {
            switch (typeA)
            {
                case Type.Int:
                    return typeB == Type.Int || typeB == Type.Double;

                case Type.Double:
                    return typeB == Type.Int || typeB == Type.Double;
            }

            return false;
        }

        public override Type GetResultTypeBinary(Type typeA, Type typeB)
        {
            return Type.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}