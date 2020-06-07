using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.Variables
{
    public class Value : TypeNode
    {
        public Value(Type type, string val, LexLocation loc = null)
        {
            Type = type;
            Val = val;
            Location = loc;
        }

        public string Val { get; private set; }

        protected override bool IsNodeEqual(SyntaxNode node)
        {
            if (base.IsNodeEqual(node))
            {
                var other = (Value)node;
                return Val == other.Val;
            }

            return false;
        }

        protected override int GetNodeHash()
        {
            return CombineHashCode(base.GetNodeHash(), Val);
        }
    }
}