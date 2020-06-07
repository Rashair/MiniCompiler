using MiniCompiler.Syntax.General;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MiniCompiler.Syntax
{
    // Names based on https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis
    public class SyntaxTree :
        IEquatable<SyntaxTree>,
        IEnumerable<List<SyntaxNode>>
    {
        public static readonly string defaultStringIndent = new string(' ', 4);
        private readonly List<List<SyntaxNode>> levels;
        private CompilationUnit compilationUnit;

        public SyntaxTree()
        {
            levels = new List<List<SyntaxNode>>();
        }

        public SyntaxTree(CompilationUnit compilationUnit)
            : this()
        {
            this.CompilationUnit = compilationUnit;
        }

        public List<SyntaxNode> this[int i] => levels[i];

        public CompilationUnit CompilationUnit
        {
            get => compilationUnit;
            set
            {
                compilationUnit = value;
                Clear();

                var walker = new SyntaxTreeWalker(this);
                walker.Walk((int lev, SyntaxNode node) =>
                {
                    while (levels.Count <= lev)
                    {
                        levels.Add(new List<SyntaxNode>());
                    }

                    if (node == null)
                    {
                        throw new ArgumentNullException(nameof(node), "SyntaxTree cannot have null nodes.");
                    }
                    levels[lev].Add(node);
                    node.Tree = this;
                });
            }
        }

        public int Count => levels.Count;

        public override string ToString()
        {
            var builder = new StringBuilder("\n");
            PrintTree(builder, compilationUnit, "", true);

            return builder.ToString();
        }

        // https://stackoverflow.com/a/8567550
        public static void PrintTree(StringBuilder builder, SyntaxNode node, string indent, bool last)
        {
            builder.AppendLine($"{indent}+- {node}");
            indent += last ? defaultStringIndent : $"|{defaultStringIndent}";

            for (int i = 0; i < node.Count; ++i)
            {
                PrintTree(builder, node[i], indent, i == node.Count - 1);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is SyntaxTree other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            // Should be different, but performance wise.
            return base.GetHashCode();
        }

        public bool Equals(SyntaxTree other)
        {
            if (this.Count != other.Count)
            {
                return false;
            }

            for (int i = 0; i < other.Count; ++i)
            {
                int levelsCount = other[i].Count;
                if (levels[i].Count != levelsCount)
                {
                    return false;
                }

                for (int j = 0; j < other[i].Count; ++j)
                {
                    if (!this[i][j].Equals(other[i][j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public IEnumerator<List<SyntaxNode>> GetEnumerator()
        {
            return levels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return levels.GetEnumerator();
        }

        public void Clear()
        {
            levels.Clear();
        }
    }
}