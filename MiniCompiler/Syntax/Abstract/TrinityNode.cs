using System;

namespace MiniCompiler.Syntax.Abstract
{
    public abstract class TrinityNode : SiblingsNode
    {
        private SyntaxNode middleChild;

        public SyntaxNode Middle
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
                    return i == 0 ? Left : i == 1 ? Middle : Right;
                }

                throw new ArgumentOutOfRangeException("I cannot give you what you seek.");
            }
            set
            {
                if (i == 0)
                {
                    Left = value;
                }
                else if (i == 1)
                {
                    Middle = value;
                }
                else if (i == 2)
                {
                    Right = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("You cannot give me what I seek.");
                }
            }
        }
    }
}