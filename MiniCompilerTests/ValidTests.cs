using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;
using MiniCompiler.Syntax;
using System;
using System.Diagnostics;
using System.IO;

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
            using (var process = new Process())
            {
                process.StartInfo.FileName = ilasm;
                process.StartInfo.Arguments = testCasePath + ".il";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();

                StreamReader reader = process.StandardOutput;
                result = reader.ReadToEnd();

                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            Console.WriteLine(result);
            Assert.AreEqual(0, exitCode, "Ilasm should return 0");



            string exePath = testCasePath + ".exe";
            using (var process = new Process())
            {
                process.StartInfo.FileName = peverify;
                process.StartInfo.Arguments = exePath;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();

                StreamReader reader = process.StandardOutput;
                result = reader.ReadToEnd();

                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            Console.WriteLine(result);
            Assert.AreEqual(0, exitCode, "PEVerify should return 0");


            using (var process = new Process())
            {
                process.StartInfo.FileName = exePath;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();

                StreamReader reader = process.StandardOutput;
                result = reader.ReadToEnd();

                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            Console.WriteLine(result);
            Assert.AreEqual(0, exitCode, "Program should return 0");
            // TODO output
        }
    }
}
