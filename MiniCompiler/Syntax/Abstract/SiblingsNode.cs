using System;

namespace MiniCompiler.Syntax.Abstract
{
    public abstract class SiblingsNode<T1, T2> : SingleChildNode<T1>
        where T1 : SyntaxNode where T2 : SyntaxNode
    {
        private T2 secondChild;

        public T1 Left { get => Child; set => Child = value; }

        public T2 Right
        {
            get => secondChild;
            set
            {
                SetChild(ref secondChild, value);
            }
        }

        public override SyntaxNode this[int i]
        {
            get
            {
                if (i < count)
                {
                    return (i == 0 ? (SyntaxNode)Left : Right);
                }

                throw new ArgumentOutOfRangeException("I cannot give you what you seek.");
            }
            set
            {
                if (i == 0)
                {
                    Left = (T1)value;
                }
                else if (i == 1)
                {
                    Right = (T2)value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("You cannot give me what I seek.");
                }
            }
        }
    }
}