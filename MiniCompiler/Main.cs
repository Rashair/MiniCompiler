using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MiniCompiler
{
    public class Compiler
    {
        private static StreamWriter sourceWriter;
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
                }
            }
            catch (Exception e)
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

                sourceWriter = new StreamWriter(file + ".il");
                result = parser.Parse();

                sourceWriter.Close();
            }

            if (errors > 0)
            {
                Console.WriteLine($"\n  {errors} errors detected\n");
                File.Delete(file + ".il");
                return 2;
            }
            else if (!result)
            {
                Console.WriteLine($"\n Unexpected error\n");
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
            sourceWriter.WriteLine(instr);
        }

        public static void EmitCode(string instr, params object[] args)
        {
            sourceWriter.WriteLine(instr, args);
        }
    }
}