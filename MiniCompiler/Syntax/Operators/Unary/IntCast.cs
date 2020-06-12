namespace MiniCompiler.Syntax.Operators.Unary
{
    public class IntCast : UnaryOperator
    {
        public override bool CanUse(Type typeA)
        {
            return typeA != Type.Unknown;
        }

        public override Type GetResultType(Type type)
        {
            return Type.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}