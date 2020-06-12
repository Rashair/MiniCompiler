namespace MiniCompiler.Syntax.Operators.Logical
{
    public class And : BinaryOperator
    {
        public override bool CanUse(Type typeA, Type typeB = Type.Unknown)
        {
            return typeA == Type.Bool && typeB == Type.Bool;
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