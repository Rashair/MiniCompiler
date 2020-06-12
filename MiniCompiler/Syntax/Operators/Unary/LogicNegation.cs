namespace MiniCompiler.Syntax.Operators.Unary
{
    public class LogicNegation : UnaryOperator
    {
        public override bool CanUse(Type typeA)
        {
            switch (typeA)
            {
                case Type.Bool:
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