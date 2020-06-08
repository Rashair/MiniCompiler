using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MiniCompiler
{
    public class Compiler
    {
        private static StreamWriter sourceWriter;
        public static Scanner scanner;
        public static Parser parser;

        public static int errors = 0;
        public static List<string> sourceLines;

        // arg[0] określa plik źródłowy
        // pozostałe argumenty są ignorowane
        public static int Main(string[] args)
        {
            Console.WriteLine("\nMini Compiler - Gardens Point");

            string file = args.FirstOrDefault();
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

            using (var sourceStream = new FileStream(file, FileMode.Open))
            {
                scanner = new Scanner(sourceStream);
                parser = new Parser(scanner);

                Console.WriteLine();

                sourceWriter = new StreamWriter(file + ".il");
                GenProlog();
                parser.Parse();
                GenEpilog();

                sourceWriter.Close();
            }

            if (errors > 0)
            {
                Console.WriteLine($"\n  {errors} errors detected\n");
                File.Delete(file + ".il");
                return 2;
            }

            Console.WriteLine(parser.SyntaxTree);
            Console.WriteLine("  compilation successful\n");
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

        private static void GenProlog()
        {
            EmitCode(".assembly extern mscorlib { }");
            EmitCode(".assembly MiniCompiler { }");
            EmitCode(".method static void main()");
            EmitCode("{");
            EmitCode(".entrypoint");
            EmitCode(".try");
            EmitCode("{");
            EmitCode();

            EmitCode("// prolog");
            EmitCode();
        }

        private static void GenEpilog()
        {
            EmitCode("leave EndMain");
            EmitCode("}");
            EmitCode("catch [mscorlib]System.Exception");
            EmitCode("{");
            EmitCode("callvirt instance string [mscorlib]System.Exception::get_Message()");
            EmitCode("call void [mscorlib]System.Console::WriteLine(string)");
            EmitCode("leave EndMain");
            EmitCode("}");
            EmitCode("EndMain: ret");
            EmitCode("}");
        }
    }
}