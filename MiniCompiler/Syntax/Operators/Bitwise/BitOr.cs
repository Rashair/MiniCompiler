namespace MiniCompiler.Syntax.Operators.Bitwise
{
    public class BitOr : BinaryOperator
    {
        public override bool CanUse(Type typeA, Type typeB = Type.Unknown)
        {
            return typeA == Type.Int && typeB == Type.Int;
        }

        public override Type GetResultTypeBinary(Type typeA, Type typeB)
        {
            return Type.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}