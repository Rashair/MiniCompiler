﻿using MiniCompiler.Syntax;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MiniCompilerTests
{
    public static class Helpers
    {
        public static string Self([CallerFilePath] string callerFilePath = null)
        {
            var callerTypeName = Path.GetFileNameWithoutExtension(callerFilePath);
            System.Console.WriteLine(callerTypeName);

            return callerTypeName;
        }

        /// <summary>
        /// Get private value of instance object
        /// </summary>
        public static TReturn GetPrivateValue<TReturn>(this object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentException("Object cannot be null.");
            }

            FieldInfo field = obj.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                throw new ArgumentException($"No such field found: {name}");
            }

            object value = field.GetValue(obj);
            if (value is TReturn result)
            {
                return result;
            }

            throw new ArgumentException($"Value is not object of type: {typeof(TReturn).Name}");
        }
    }
}
