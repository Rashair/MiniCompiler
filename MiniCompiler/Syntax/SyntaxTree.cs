using MiniCompiler.Syntax.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompiler.Syntax
{
    // Based on https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis
    public class SyntaxTree
    {
        public SyntaxTree()
        {
        }


        public CompilationUnit CompilationUnit { get; set; }
    }
}
