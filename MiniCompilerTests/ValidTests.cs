using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;
using MiniCompiler.Syntax;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MiniCompilerTests
{
    public class ValidTests : BaseTests
    {
        protected const int DefaultTimeout = 1000;

        private readonly string ilasm = ToolLocationHelper.GetPathToDotNetFrameworkFile("ilasm.exe",
            TargetDotNetFrameworkVersion.VersionLatest);

        private readonly string peverify = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7 Tools\PEVerify.exe";

        public ValidTests() :
            base(0, Helpers.Self())
        {
        }

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            ExpectedTree = null;
        }

        protected SyntaxTree ExpectedTree { get; set; }

        protected override void AssertCorrect(string testCasePath)
        {
            if (ExpectedTree != null)
            {
                SyntaxTree syntaxTree = Compiler.parser.SyntaxTree;
                Assert.AreEqual(ExpectedTree, syntaxTree, "Trees should be equal");
            }

            string result;
            int exitCode;
            using (var process = CreateProcess(ilasm, testCasePath + ".il"))
            {
                process.Start();

                StreamReader reader = process.StandardOutput;
                result = reader.ReadToEnd();

                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            Console.WriteLine(result);
            Assert.AreEqual(0, exitCode, "Ilasm should return 0");

            string exePath = testCasePath + ".exe";
            using (var process = CreateProcess(peverify, exePath))
            {
                process.Start();

                StreamReader reader = process.StandardOutput;
                result = reader.ReadToEnd();

                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            Console.WriteLine(result);
            Assert.AreEqual(0, exitCode, "PEVerify should return 0");

            exitCode = -1;
            using (var process = CreateProcess(exePath))
            {
                process.StartInfo.RedirectStandardInput = true;
                process.Start();

                var inFile = testCasePath.Substring(0,
                    testCasePath.Length - extension.Length) + "in";
                if (File.Exists(inFile))
                {
                    using (var inReader = new StreamReader(inFile))
                    {
                        for (string str = inReader.ReadLine(); str != null; str = inReader.ReadLine())
                        {
                            process.StandardInput.WriteLine(str);
                        }
                    }
                }
                process.StandardInput.Close();

                StreamReader reader = process.StandardOutput;
                result = reader.ReadToEnd();
                reader.Close();

                if (process.WaitForExit(DefaultTimeout / 2))
                {
                    exitCode = process.ExitCode;
                }
                else
                {
                    process.Kill();
                    process.Close();
                    process.WaitForExit();
                }
            }
            Console.WriteLine(result);
            Assert.AreEqual(0, exitCode, "Program should return 0");

            string outFile = testCasePath.Substring(0,
                testCasePath.Length - extension.Length) + "out";
            if (File.Exists(outFile))
            {
                string output = File.ReadAllText(outFile);
                bool areEqual = output == result;

                if (!areEqual)
                {
                    int index = output.Zip(result, (c1, c2) => c1 == c2).TakeWhile(b => b).Count() + 1;
                    Console.WriteLine("Expected: " + (output.Length > index ? $"[{(int)output[index]}]" +
                        output[index].ToString() :
                        "shorter result"));
                    Console.WriteLine($"Got: " + (result.Length > index ? $"[{(int)result[index]}]" +
                        result[index].ToString() :
                        "result too short"));

                    int interv = 10;
                    if (index - interv >= 0 && index + interv < output.Length)
                    {
                        Console.WriteLine($"Env[{index}]:'{output.Substring(index - interv, 2 * interv - 1)}'");
                    }

                    Assert.Fail();
                }
            }
        }

        private static Process CreateProcess(string path, string args = "")
        {
            var process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = args;

            return process;
        }
    }
}