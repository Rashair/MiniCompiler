
using System;
using System.IO;
using System.Collections.Generic;
using GardensPoint;
using System.Linq;

public class Compiler
{
    public static int errors = 0;

    public static List<string> sourceLines;

    // arg[0] określa plik źródłowy
    // pozostałe argumenty są ignorowane
    public static int Main(string[] args)
    {
        FileStream sourceStream;
        Console.WriteLine("\nMini Compiler - Gardens Point");

        string file = args.FirstOrDefault();
        if (file == null)
        {
            Console.Write("\nsource file:  ");
            file = "test-source.txt"; //Console.ReadLine();
        }

        try
        {
            var sr = new StreamReader(file);
            string str = sr.ReadToEnd();
            sr.Close();
            Compiler.sourceLines = new System.Collections.Generic.List<string>(str.Split(new string[] { "\r\n" }, System.StringSplitOptions.None));
            sourceStream = new FileStream(file, FileMode.Open);
        }
        catch (Exception e)
        {
            Console.WriteLine("\n" + e.Message);
            return 1;
        }
        Scanner scanner = new Scanner(sourceStream);
        Parser parser = new Parser(scanner);
        Console.WriteLine();
        sw = new StreamWriter(file + ".il");

        GenProlog();
        parser.Parse();
        GenEpilog();

        sw.Close();
        sourceStream.Close();

        if (errors > 0)
        {
            Console.WriteLine($"\n  {errors} errors detected\n");
            File.Delete(file + ".il");
            return 2;
        }


        Console.WriteLine("  compilation successful\n");
        return 0;
    }

    public static void EmitCode(string instr = null)
    {
        sw.WriteLine(instr);
    }

    public static void EmitCode(string instr, params object[] args)
    {
        sw.WriteLine(instr, args);
    }

    private static StreamWriter sw;

    private static void GenProlog()
    {
        EmitCode(".assembly extern mscorlib { }");
        EmitCode(".assembly calculator { }");
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

