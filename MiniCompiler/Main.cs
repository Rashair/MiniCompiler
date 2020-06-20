using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MiniCompiler
{
    public class Compiler
    {
        private static List<string> assemblyLines;
        public static Scanner scanner;
        public static Parser parser;
        public static string file;

        public static int errors = 0;
        public static List<string> sourceLines;

        // arg[0] określa plik źródłowy
        // pozostałe argumenty są ignorowane
        public static int Main(string[] args)
        {
            Console.WriteLine("\nMini Compiler - Gardens Point");

            file = args.FirstOrDefault();
            if (file == null)
            {
                Console.Write("\nsource file:  ");
                file = "test-source.txt"; //Console.ReadLine();
            }

            try
            {
                using (var reader = new StreamReader(file))
                {
                    string source = reader.ReadToEnd();
                    sourceLines = new List<string>(source.Split(new string[] { "\r\n" }, System.StringSplitOptions.None)); ;
                    assemblyLines = new List<string>(sourceLines.Count * 2);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                return 1;
            }

            bool result;
            using (var sourceStream = new FileStream(file, FileMode.Open))
            {
                scanner = new Scanner(sourceStream);
                parser = new Parser(scanner);
                Console.WriteLine();

                result = parser.Parse();

                File.WriteAllLines(file + ".il", assemblyLines);
            }

            if (errors > 0)
            {
                Console.WriteLine($"\n  {errors} errors detected\n");
                File.Delete(file + ".il");
                return 2;
            }
            else if (!result)
            {
                Console.WriteLine($"\n Source code is invalid\n");
                File.Delete(file + ".il");
                return 3;
            }

            if (parser.SyntaxTree.Count < 10 && parser.SyntaxTree.All(nodes => nodes.Count < 15))
            {
                Console.WriteLine(parser.SyntaxTree);
            }
            Console.WriteLine("  compilation successful\n");
            Console.Write("\n\n\r");
            return 0;
        }

        public static void EmitCode(string instr = null)
        {
            assemblyLines.Add(instr);
        }

        public static void EmitAfterFirst(string toFind, string instr)
        {
            for (int i = 0; i < assemblyLines.Count - 1; ++i)
            {
                if (assemblyLines[i].Contains(toFind))
                {
                    assemblyLines.Insert(i + 1, instr);
                    break;
                }
            }
        }
    }
}