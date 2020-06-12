using System;

namespace MiniCompiler.Syntax.Abstract
{
    public abstract class SingleChildNode<T> : SyntaxNode
        where T : SyntaxNode
    {
        private T child;
        protected int count = 0;

        public T Child
        {
            get => child;
            set
            {
                SetChild(ref child, value);
            }
        }

        protected void SetChild<TSet>(ref TSet childToSet, TSet value)
            where TSet : SyntaxNode
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "You cannot set child to null.");
            }

            if (childToSet == null && value != null)
            {
                ++count;
            }

            childToSet = value;
            childToSet.Parent = this;
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
                    Child = (T)value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("You cannot give me what I seek.");
                }
            }
        }
    }
}