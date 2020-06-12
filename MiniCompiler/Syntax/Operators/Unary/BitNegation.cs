namespace MiniCompiler.Syntax.Operators.Unary
{
    public class BitNegation : UnaryOperator
    {
        public override bool CanUse(Type typeA)
        {
            switch (typeA)
            {
                case Type.Int:
                    return true;
            }

            return false;
        }

        public override Type GetResultType(Type type)
        {
            return type;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}