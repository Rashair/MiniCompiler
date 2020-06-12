using System;

namespace MiniCompiler.Syntax.Abstract
{
    public abstract class TrinityNode<T1, T2, T3> : SiblingsNode<T1, T3>
        where T1 : SyntaxNode where T2 : SyntaxNode where T3 : SyntaxNode
    {
        private T2 middleChild;

        public T2 Middle
        {
            get => middleChild;
            set
            {
                SetChild(ref middleChild, value);
            }
        }

        public override SyntaxNode this[int i]
        {
            get
            {
                if (i < count)
                {
                    return i == 0 ? Left : i == 1 ? Middle : (SyntaxNode)Right;
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
                    Middle = (T2)value;
                }
                else if (i == 2)
                {
                    Right = (T3)value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("You cannot give me what I seek.");
                }
            }
        }
    }
}