﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;
using System.IO;
using System.Runtime.CompilerServices;

namespace MiniCompilerTests
{
    public abstract class BaseTests
    {
        protected string extension = "txt";

        // Length(Base)
        private const int prefixLength = 4;

        // Length(Tests)
        private const int suffixLength = 5;

        private readonly int expectedResult;
        private readonly string testsPath;

        public BaseTests(int expectedResult, string derriviedClassName)
        {
            this.expectedResult = expectedResult;

            string namePrefix = derriviedClassName.Substring(0, derriviedClassName.Length - suffixLength);
            string realTypeName = GetType().Name;
            this.testsPath = Path.Combine("..", "..", PathSuffix,
                GetTestFolderName(realTypeName, namePrefix.Length),
                namePrefix);
        }

        protected virtual string PathSuffix => "";

        protected abstract void AssertCorrect(string testCasePath);

        [TestInitialize]
        public virtual void Init()
        {
            // Do not run in parallel !!!
            Compiler.errors = 0;
        }

        protected void Invoke([CallerMemberName] string caller = null)
        {
            // Arrange
            string testCasePath = GetPath(caller);
            string[] args = GetArgs(testCasePath);

            // Act
            int result = Compiler.Main(args);

            // Assert
            Assert.AreEqual(expectedResult, result);
            AssertCorrect(testCasePath);
        }

        public string GetPath(string caller)
        {
            string testCaseName = GetCaseName(caller);
            return Path.Combine(testsPath, $"{testCaseName}.{extension}");
        }

        public static string GetCaseName(string methodName)
        {
            return methodName.Substring(prefixLength);
        }

        public static string[] GetArgs(string arg)
        {
            return new string[] { arg };
        }

        private static string GetTestFolderName(string realTypeName, int classNamePrefixLength)
        {
            int beg = realTypeName.Length - classNamePrefixLength - suffixLength;
            string result = realTypeName.Remove(beg, classNamePrefixLength);

            return result;
        }
    }
}