﻿using System;

namespace MiniCompiler.Syntax.Abstract
{
    public abstract class SiblingsNode : SingleChildNode
    {
        private SyntaxNode secondChild;

        public SyntaxNode Left { get => Child; set => Child = value; }

        public SyntaxNode Right
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
                    return i == 0 ? Left : Right;
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
                    Right = value;
                }

                throw new ArgumentOutOfRangeException("You cannot give me what I seek.");
            }
        }

        public SiblingsNode WithLeft(SyntaxNode left)
        {
            Left = left;
            return this;
        }

        public SiblingsNode WithRight(SyntaxNode right)
        {
            Right = right;
            return this;
        }
    }
}