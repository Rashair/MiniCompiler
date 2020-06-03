﻿using QUT.Gppg;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniCompiler.Syntax
{
    public abstract class SyntaxNode :
        IEquatable<SyntaxNode>,
        IEnumerable<SyntaxNode>
    {
        protected SyntaxNode parent;
        protected List<SyntaxNode> children;

        public SyntaxNode()
        {
            children = new List<SyntaxNode>();
        }

        public SyntaxNode(LexLocation loc)
            : this()
        {
            this.Location = new LexLocation(loc.StartLine, loc.StartColumn, loc.EndLine, loc.EndColumn);
        }

        public SyntaxTree Tree { get; set; }

        public LexLocation Location { get; }

        public SyntaxNode this[int i] => children[i];

        public int Count => children.Count;

        public bool IsLeaf()
        {
            return children.Count == 0;
        }

        public SyntaxNode Add(SyntaxNode node)
        {
            node.parent = this;
            node.Tree = Tree;
            children.Add(node);

            return this;
        }

        public void SetChildren(List<SyntaxNode> children)
        {
            this.children = children;
        }

        public bool Equals(SyntaxNode other)
        {
            if (other == null)
            {
                return false;
            }

            var typeThis = GetType();
            var typeOther = other.GetType();
            if (typeThis == typeOther)
            {

                return children.Count == other.children.Count;
            }

            return false;
        }


        public IEnumerator<SyntaxNode> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.GetEnumerator();
        }
    }
}
