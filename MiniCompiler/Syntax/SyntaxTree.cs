using MiniCompiler.Syntax.General;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompiler.Syntax
{
    // Names based on https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis
    public class SyntaxTree :
        IEquatable<SyntaxTree>,
        IEnumerable<List<SyntaxNode>>,
        ICollection<List<SyntaxNode>>
    {
        private readonly List<List<SyntaxNode>> levels;
        private CompilationUnit compilationUnit;

        public SyntaxTree()
        {
            levels = new List<List<SyntaxNode>>();
        }

        public SyntaxTree(CompilationUnit compilationUnit)
        {
            levels = new List<List<SyntaxNode>>();
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

                    levels[lev].Add(node);
                    node.Tree = this;
                });
            }
        }

        public int Count => levels.Count;

        public bool IsReadOnly => true;

        public override string ToString()
        {
            var builder = new StringBuilder("\n");
            PrintTree(builder, compilationUnit, "", true);

            return builder.ToString();
        }

        // https://stackoverflow.com/a/8567550
        public static void PrintTree(StringBuilder builder, SyntaxNode node, string indent, bool last)
        {
            builder.AppendLine(indent + "+- " + node.GetType().Name);
            indent += last ? "   " : "|  ";

            for (int i = 0; i < node.Count; ++i)
            {
                PrintTree(builder, node[i], indent, i == node.Count - 1);
            }
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

        public void Add(List<SyntaxNode> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            levels.Clear();
        }

        public bool Contains(List<SyntaxNode> item)
        {
            return levels.Contains(item);
        }

        public void CopyTo(List<SyntaxNode>[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < levels.Count; ++i)
            {
                array[i] = levels[i];
            }
        }

        public IEnumerator<List<SyntaxNode>> GetEnumerator()
        {
            return levels.GetEnumerator();
        }

        public bool Remove(List<SyntaxNode> item)
        {
            return levels.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return levels.GetEnumerator();
        }
    }
}
