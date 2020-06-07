using System;

namespace MiniCompiler.Syntax.Abstract
{
    public abstract class SingleChildNode : SyntaxNode
    {
        private SyntaxNode child;
        protected int count = 0;

        public SyntaxNode Child
        {
            get => child;
            set
            {
                if (child == null && value != null)
                {
                    ++count;
                }
                else if (child != null && value == null)
                {
                    --count;
                }

                child = value;
            }
        }

        public override int Count => count;

        public override SyntaxNode this[int i]
        {
            get => i < count ? Child :
                   throw new ArgumentOutOfRangeException("I cannot give you what you seek.");
            set
            {
                if (i == 0)
                {
                    Child = value;
                }

                throw new ArgumentOutOfRangeException("You cannot give me what I seek.");
            }
        }
    }
}