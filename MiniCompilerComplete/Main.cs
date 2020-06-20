using QUT.Gppg;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Reflection;
using System.Collections;
using System.Text;

namespace MiniCompilerComplete
{
    public static class EnumToCilExtensions
    {
        public static string ToCil(this MiniType type)
        {
            switch (type)
            {
                case MiniType.Int:
                    return "int32";

                case MiniType.Double:
                    return "float64";

                case MiniType.Bool:
                    return "bool";

                case MiniType.String:
                    return "string";
            }

            return null;
        }

        public static string ToPrimitive(this MiniType type)
        {
            switch (type)
            {
                case MiniType.Int:
                    return "i4";

                case MiniType.Double:
                    return "r8";

                case MiniType.Bool:
                    return "i4";
            }

            return null;
        }

        public static string ToCSharp(this MiniType type)
        {
            switch (type)
            {
                case MiniType.Int:
                    return "Int32";

                case MiniType.Double:
                    return "Double";

                case MiniType.Bool:
                    return "Boolean";

                case MiniType.String:
                    return "String";
            }

            return null;
        }
    }

    public static class LexLocationExtensions
    {
        public static LexLocation Copy(this LexLocation location)
        {
            return new LexLocation(location.StartLine, location.StartColumn, location.EndLine, location.EndColumn);
        }
    }

    public static class OperatorEnumExtensions
    {
        public delegate T ObjectActivator<T>(params object[] args);

        public static Operator CreateOperator(this OperatorEnum op)
        {
            var className = op.ToString();
            var type = GetTypeByName(className);
            if (type == null)
            {
                return new UnknownOperator();
            }

            return (Operator)Activator.CreateInstance(type, true);
        }

        private static System.Type GetTypeByName(string className)
        {
            System.Type[] assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            return assemblyTypes.FirstOrDefault(type => type.Name == className);
        }
    }

    public static class SiblingsNodeExtensions
    {
        public static TObj WithLeft<TObj, TChild>(this TObj obj, TChild left)
            where TObj : SiblingsNode<TChild, TChild>
            where TChild : SyntaxNode
        {
            obj.Left = left;
            return obj;
        }

        public static TObj WithRight<TObj, TChild>(this TObj obj, TChild right)
            where TObj : SiblingsNode<TChild, TChild>
            where TChild : SyntaxNode
        {
            obj.Right = right;
            return obj;
        }
    }

    public static class TokenExtensions
    {
        public const int UnaryMultipler = 100;

        public static MiniType ConvertToType(this Token token)
        {
            switch (token)
            {
                case Token.DoubleKey:
                case Token.DoubleVal:
                    return MiniType.Double;

                case Token.IntKey:
                case Token.IntVal:
                    return MiniType.Int;

                case Token.BoolKey:
                case Token.True:
                case Token.False:
                    return MiniType.Bool;
            }

            return MiniType.Unknown;
        }

        public static OperatorEnum ConvertToOperator(this Token token, bool unary = false)
        {
            if (unary)
            {
                switch (token)
                {
                    case Token.Minus:
                        return OperatorEnum.UnaryMinus;
                }
            }

            return (OperatorEnum)token;
        }
    }

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

    public partial class Parser
    {
        private IScope currentScope;
        private Token lastErrorToken;
        private bool recovering;

        public Parser(Scanner scanner) : base(scanner)
        {
            currentScope = new EmptyScope();
        }

        public SyntaxTree SyntaxTree { get; private set; }

        public LexLocation Loc => CurrentLocationSpan;

        private void GenerateCode(CompilationUnit unit)
        {
            SyntaxTree = new SyntaxTree(unit);
            if (Compiler.errors > 0)
            {
                return;
            }

            var visitor = new SyntaxVisitor(SyntaxTree);
            visitor.Visit();
        }

        private void EnterScope(IScope scope)
        {
            currentScope = scope;
        }

        private void LeaveScope()
        {
            currentScope = currentScope.GetParentScope();
        }

        private void StartRecovery()
        {
            recovering = true;
        }

        private void EndRecovery()
        {
            recovering = false;
        }

        private ValueType Error(string msg, params object[] pars)
        {
            if (!recovering)
            {
                Console.WriteLine($"  line {Loc.StartLine,3}: {string.Format(msg, pars)}");
                ++Compiler.errors;
            }
            yyerrok();

            CurrentSemanticValue.node = new EmptyNode(Loc);
            CurrentSemanticValue.typeNode = new EmptyTypeNode(Loc);
            CurrentSemanticValue.type = MiniType.Unknown;

            if (lastErrorToken == Token.Eof)
            {
                YYAbort();
            }

            return CurrentSemanticValue;
        }

        private TypeNode TryCreateOperator(Token token, TypeNode left)
        {
            TypeNode result;
            var expType = left.Type;
            if (!Operator.CanUse(token, expType))
            {
                result = Error("Cannot use {0} on {1}.", token, expType)
                       .typeNode;
                StartRecovery();
            }
            else
            {
                result = Operator.Create(token, expType, Loc)
                         .WithLeft(left);
            }

            return result;
        }

        private TypeNode TryCreateOperator(Token token, TypeNode left, TypeNode right)
        {
            TypeNode result;
            if (!Operator.CanUse(token, left.Type, right.Type))
            {
                result = Error("Cannot {0} {1} and {2}.", token, left.Type, right.Type.ToString())
                       .typeNode;
                StartRecovery();
            }
            else
            {
                result = Operator.Create(token, left.Type, right.Type, Loc)
                         .WithLeft(left)
                         .WithRight(right);
            }

            return result;
        }

        private TypeNode CreateValue()
        {
            var value = ValueStack[ValueStack.Depth - 1];
            MiniType type = value.token.ConvertToType();
            string val = value.val;

            TypeNode result;
            if (type == MiniType.Unknown)
            {
                result = Error("Cannot use provided type: {0}", value.token).typeNode;
                StartRecovery();
            }
            else
            {
                result = new Value(type, val, CurrentLocationSpan);
            }

            return result;
        }

        private TypeNode TryCreateVariableReference(string id, LexLocation loc = null)
        {
            VariableDeclaration declar = null;
            TypeNode result;
            if (!currentScope.TryGetVariable(id, ref declar))
            {
                result = Error("Variable {0} not declared.", id).typeNode;
                StartRecovery();
            }
            else
            {
                result = new VariableReference(declar, loc);
            }

            return result;
        }
    }

    public abstract class BigParentNode : SyntaxNode,
        IEnumerable<SyntaxNode>
    {
        protected List<SyntaxNode> children;

        public BigParentNode(LexLocation loc = null)
            : base(loc)
        {
            children = new List<SyntaxNode>();
        }

        public override int Count => children.Count;

        public override SyntaxNode this[int i]
        {
            get => children[i];
            set
            {
                children[i] = value;
                children[i].Parent = this;
            }
        }

        public override int GetHashCode()
        {
            return CombineHashCode(base.GetHashCode(), children.Count);
        }

        public IEnumerator<SyntaxNode> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.GetEnumerator();
        }

        public SyntaxNode Add(SyntaxNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node), "You cannot add null child.");
            }

            node.Parent = this;
            node.Tree = Tree;
            children.Add(node);

            return this;
        }

        public void AddChildren(List<SyntaxNode> children)
        {
            foreach (var child in children)
            {
                Add(child);
            }
        }
    }

    public abstract class SiblingsNode<T1, T2> : SingleChildNode<T1>
        where T1 : SyntaxNode where T2 : SyntaxNode
    {
        private T2 secondChild;

        public T1 Left { get => Child; set => Child = value; }

        public T2 Right
        {
            get => secondChild;
            set
            {
                SetChild(ref secondChild, value);
            }
        }

        public override SyntaxNode this[int i]
        {
            get
            {
                if (i < count)
                {
                    return (i == 0 ? (SyntaxNode)Left : Right);
                }

                throw new ArgumentOutOfRangeException("I cannot give you what you seek.");
            }
            set
            {
                if (i == 0)
                {
                    Left = (T1)value;
                }
                else if (i == 1)
                {
                    Right = (T2)value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("You cannot give me what I seek.");
                }
            }
        }
    }

    public abstract class SingleChildNode<T> : SyntaxNode
        where T : SyntaxNode
    {
        private T child;
        protected int count = 0;

        public T Child
        {
            get => child;
            set
            {
                SetChild(ref child, value);
            }
        }

        protected void SetChild<TSet>(ref TSet childToSet, TSet value)
            where TSet : SyntaxNode
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "You cannot set child to null.");
            }

            if (childToSet == null && value != null)
            {
                ++count;
            }

            childToSet = value;
            childToSet.Parent = this;
        }

        public override int Count => count;

        public override SyntaxNode this[int i]
        {
            get => i < count ? Child :
                   throw new ArgumentOutOfRangeException("I cannot give you what you seek.");
            set
            {
                if (i == 0)
                {
                    Child = (T)value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("You cannot give me what I seek.");
                }
            }
        }
    }

    public abstract class TrinityNode<T1, T2, T3> : SiblingsNode<T1, T3>
        where T1 : SyntaxNode where T2 : SyntaxNode where T3 : SyntaxNode
    {
        private T2 middleChild;

        public T2 Middle
        {
            get => middleChild;
            set
            {
                SetChild(ref middleChild, value);
            }
        }

        public override SyntaxNode this[int i]
        {
            get
            {
                if (i < count)
                {
                    return i == 0 ? Left : i == 1 ? Middle : (SyntaxNode)Right;
                }

                throw new ArgumentOutOfRangeException("I cannot give you what you seek.");
            }
            set
            {
                if (i == 0)
                {
                    Left = (T1)value;
                }
                else if (i == 1)
                {
                    Middle = (T2)value;
                }
                else if (i == 2)
                {
                    Right = (T3)value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("You cannot give me what I seek.");
                }
            }
        }
    }

    public abstract class TypeNode : SiblingsNode<TypeNode, TypeNode>
    {
        public virtual MiniType Type { get; protected set; }

        protected override bool IsNodeEqual(SyntaxNode node)
        {
            if (base.IsNodeEqual(node))
            {
                var other = node as TypeNode;
                return other.Type == Type;
            }

            return false;
        }

        public override bool HasValue => true;

        protected override int GetNodeHash()
        {
            return CombineHashCode(base.GetNodeHash(), Type);
        }
    }

    public class Block : BigParentNode
    {
        public Block(LexLocation location = null) : base(location)
        {
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class CompilationUnit : SingleChildNode<SyntaxNode>
    {
        public CompilationUnit(LexLocation location = null)
        {
            Location = location;
        }

        public CompilationUnit(SyntaxTree tree, LexLocation location = null) :
            this(location)
        {
            this.Tree = tree;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ElseCond : SingleChildNode<SyntaxNode>
    {
        public ElseCond(LexLocation loc = null)
        {
            Location = loc;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class EmptyNode : SyntaxNode
    {
        public EmptyNode(LexLocation loc = null) : base(loc)
        {
        }

        public override SyntaxNode this[int i] { get => throw new NotImplementedException(); set { } }

        public override int Count => 0;

        public override bool ShouldInclude => false;

        public override void Visit(SyntaxVisitor visitor)
        {
        }
    }

    public class EmptyTypeNode : TypeNode
    {
        public EmptyTypeNode(LexLocation loc = null)
        {
            Location = loc;
            Type = MiniType.Unknown;
        }

        public override SyntaxNode this[int i] { get => throw new NotImplementedException(); set { } }

        public override int Count => 0;

        public override void Visit(SyntaxVisitor visitor)
        {
        }
    }

    public class IfCond : TrinityNode<TypeNode, SyntaxNode, SyntaxNode>
    {
        public IfCond(LexLocation loc = null)
        {
            Location = loc;
        }

        public bool HasElse => Count > 2;

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Return : EmptyNode
    {
        public Return(LexLocation loc = null)
        {
            Location = loc;
        }

        public override bool ShouldInclude => true;

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class WhileLoop : SiblingsNode<TypeNode, SyntaxNode>
    {
        public WhileLoop(LexLocation loc = null)
        {
            Location = loc;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    internal class Usage
    {
        public int Used { get; set; }
        public int Max { get; set; }
    }

    public class Read : SingleChildNode<TypeNode>
    {
        public Read(LexLocation loc = null)
        {
            Location = loc;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class SimpleString : TypeNode
    {
        public SimpleString(string value, LexLocation loc = null)
        {
            Value = value;
            Type = MiniType.String;
            Location = loc;
        }

        public string Value { get; }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Write : SingleChildNode<TypeNode>
    {
        public Write(LexLocation loc = null)
        {
            Location = loc;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Assign : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB)
        {
            switch (typeA)
            {
                case MiniType.Double:
                    return typeB == MiniType.Double || typeB == MiniType.Int;
            }

            return typeA == typeB;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return typeA;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public abstract class BinaryOperator : Operator
    {
        public override bool CanUse(MiniType typeA)
        {
            return false;
        }

        public override MiniType GetResultType(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            if (typeA == MiniType.Unknown || typeB == MiniType.Unknown)
            {
                throw new ArgumentException("You can't use this operator on this types.");
            }

            return GetResultTypeBinary(typeA, typeB);
        }

        public abstract MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB);
    }

    public class BitAnd : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            return typeA == MiniType.Int && typeB == MiniType.Int;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class BitOr : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            return typeA == MiniType.Int && typeB == MiniType.Int;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class And : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            return typeA == MiniType.Bool && typeB == MiniType.Bool;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Or : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            return typeA == MiniType.Bool && typeB == MiniType.Bool;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Add : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return typeA == MiniType.Double || typeB == MiniType.Double ? MiniType.Double : MiniType.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Divides : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return typeA == MiniType.Double || typeB == MiniType.Double ? MiniType.Double : MiniType.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Multiplies : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return typeA == MiniType.Double || typeB == MiniType.Double ? MiniType.Double : MiniType.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Subtract : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return typeA == MiniType.Double || typeB == MiniType.Double ? MiniType.Double : MiniType.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public abstract class Operator : TypeNode
    {
        protected Operator()
        {
        }

        public OperatorEnum Token { get; private set; }

        public abstract bool CanUse(MiniType typeA);

        public abstract bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown);

        public abstract MiniType GetResultType(MiniType typeA, MiniType typeB = MiniType.Unknown);

        public Operator WithResultType(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            Type = GetResultType(typeA, typeB);
            return this;
        }

        public static bool CanUse(Token token, MiniType typeA, MiniType typeB) => Factory.CanUse(token, typeA, typeB);

        public static bool CanUse(Token token, MiniType typeA) => Factory.CanUse(token, typeA);

        public static Operator Create(Token token, MiniType typeA, MiniType typeB, LexLocation location = null) =>
            Factory.Create(token, typeA, typeB, location);

        public static Operator Create(Token token, MiniType typeA, LexLocation location = null) =>
            Factory.Create(token, typeA, location);

        public static class Factory
        {
            private static OperatorEnum lastToken;
            private static Operator lastOperator;

            public static bool CanUse(Token token, MiniType typeA, MiniType typeB)
            {
                lastToken = token.ConvertToOperator();
                lastOperator = CreateFromToken(lastToken);
                return lastOperator.CanUse(typeA, typeB);
            }

            public static bool CanUse(Token token, MiniType typeA)
            {
                lastToken = token.ConvertToOperator(true);
                lastOperator = CreateFromToken(lastToken);
                return lastOperator.CanUse(typeA);
            }

            private static Operator CreateFromToken(OperatorEnum op)
            {
                return op.CreateOperator();
            }

            public static Operator Create(Token token, MiniType typeA, MiniType typeB, LexLocation location = null)
            {
                var operatorToken = token.ConvertToOperator();
                return Create(operatorToken, location).WithResultType(typeA, typeB);
            }

            public static Operator Create(Token token, MiniType typeA, LexLocation location = null)
            {
                var operatorToken = token.ConvertToOperator(true);
                return Create(operatorToken, location).WithResultType(typeA);
            }

            private static Operator Create(OperatorEnum op, LexLocation location)
            {
                Operator result;
                if (op == lastToken)
                {
                    result = lastOperator;
                    lastToken = OperatorEnum.Unknown;
                    lastOperator = null;
                }
                else
                {
                    result = CreateFromToken(op);
                }

                result.Token = op;
                result.Location = location;

                return result;
            }
        }
    }

    public enum OperatorEnum
    {
        Unknown = 0,
        UnaryMinus = Token.Minus * TokenExtensions.UnaryMultipler,
        BitNegation = Token.BitNegation,
        LogicNegation = Token.Negation,
        IntCast = Token.IntKey,
        DoubleCast = Token.DoubleKey,
        BitOr = Token.BitOr,
        BitAnd = Token.BitAnd,
        Multiplies = Token.Multiplies,
        Divides = Token.Divides,
        Add = Token.Add,
        Subtract = Token.Minus,
        Equals = Token.Equals,
        NotEquals = Token.NotEquals,
        Greater = Token.Greater,
        GreaterOrEqual = Token.GreaterOrEqual,
        Less = Token.Less,
        LessOrEqual = Token.LessOrEqual,
        Or = Token.Or,
        And = Token.And,
        Assign = Token.Assign,
    }

    public class Equals : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Bool:
                    return typeB == MiniType.Bool;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Greater : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class GreaterOrEqual : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Less : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class LessOrEqual : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class NotEquals : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Bool:
                    return typeB == MiniType.Bool;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class BitNegation : UnaryOperator
    {
        public override bool CanUse(MiniType typeA)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return true;
            }

            return false;
        }

        public override MiniType GetResultType(MiniType type)
        {
            return type;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class DoubleCast : UnaryOperator
    {
        public override bool CanUse(MiniType typeA)
        {
            return typeA != MiniType.Unknown;
        }

        public override MiniType GetResultType(MiniType type)
        {
            return MiniType.Double;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class IntCast : UnaryOperator
    {
        public override bool CanUse(MiniType typeA)
        {
            return typeA != MiniType.Unknown;
        }

        public override MiniType GetResultType(MiniType type)
        {
            return MiniType.Int;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class LogicNegation : UnaryOperator
    {
        public override bool CanUse(MiniType typeA)
        {
            switch (typeA)
            {
                case MiniType.Bool:
                    return true;
            }

            return false;
        }

        public override MiniType GetResultType(MiniType type)
        {
            return type;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class UnaryMinus : UnaryOperator
    {
        public override bool CanUse(MiniType typeA)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return true;

                case MiniType.Double:
                    return true;
            }

            return false;
        }

        public override MiniType GetResultType(MiniType type)
        {
            return type;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public abstract class UnaryOperator : Operator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB)
        {
            return false;
        }

        public override MiniType GetResultType(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            if (typeB != MiniType.Unknown || typeA == MiniType.Unknown)
            {
                throw new ArgumentException("You can't use this operator on this types.");
            }

            return GetResultType(typeA);
        }

        public abstract MiniType GetResultType(MiniType type);
    }

    public class UnknownOperator : Operator
    {
        public override bool CanUse(MiniType typeA)
        {
            return false;
        }

        public override bool CanUse(MiniType typeA, MiniType typeB)
        {
            return false;
        }

        public override MiniType GetResultType(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            return MiniType.Unknown;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
        }
    }

    public abstract class SyntaxNode
    {
        protected SyntaxNode parent;
        protected LexLocation location;

        public SyntaxNode(LexLocation loc = null)
        {
            Location = loc;
        }

        public SyntaxNode Parent { private get => parent; set => parent = value; }

        public SyntaxTree Tree { get; set; }

        public LexLocation Location
        {
            get => location;
            set
            {
                location = value?.Copy() ?? new LexLocation();
            }
        }

        public virtual bool ShouldInclude => true;

        public virtual bool HasValue => false;

        public abstract int Count { get; }

        public abstract SyntaxNode this[int i] { get; set; }

        public abstract void Visit(SyntaxVisitor visitor);

        public override bool Equals(object obj)
        {
            if (obj is SyntaxNode other)
            {
                return IsNodeEqual(other);
            }

            return false;
        }

        protected virtual bool IsNodeEqual(SyntaxNode node)
        {
            if (node == null)
            {
                return false;
            }

            if (GetType() == node.GetType())
            {
                return Count == node.Count &&
                    parent?.GetType() == node.parent?.GetType();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return GetNodeHash();
        }

        protected virtual int GetNodeHash()
        {
            int hash = 17;
            hash = CombineHashCode(hash, GetType(), Count);
            hash = CombineHashCode(hash, parent?.GetType());
            return hash;
        }

        public static int CombineHashCode(int start, object b, object c = null)
        {
            int hash = start * 23 + b?.GetHashCode() ?? 0;
            hash = hash * c?.GetHashCode() ?? 0;
            return hash;
        }

        public override string ToString()
        {
            var result = GetType().Name;
            if (Location != null)
            {
                result += $"({Location.StartLine}..{Location.EndLine}) ({Location.StartColumn}..{Location.EndColumn})";
            }
            return result;
        }
    }

    // Names based on https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis
    public class SyntaxTree :
        IEquatable<SyntaxTree>,
        IEnumerable<List<SyntaxNode>>
    {
        public static readonly string defaultStringIndent = new string(' ', 4);
        private readonly List<List<SyntaxNode>> levels;
        private CompilationUnit compilationUnit;

        public SyntaxTree()
        {
            levels = new List<List<SyntaxNode>>();
        }

        public SyntaxTree(CompilationUnit compilationUnit)
            : this()
        {
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

                    if (node == null)
                    {
                        throw new ArgumentNullException(nameof(node), "SyntaxTree cannot have null nodes.");
                    }
                    levels[lev].Add(node);
                    node.Tree = this;
                });
            }
        }

        public int Count => levels.Count;

        public override string ToString()
        {
            var builder = new StringBuilder("\n");
            PrintTree(builder, compilationUnit, "", true);

            return builder.ToString();
        }

        // https://stackoverflow.com/a/8567550
        public static void PrintTree(StringBuilder builder, SyntaxNode node, string indent, bool last)
        {
            builder.AppendLine($"{indent}+- {node}");
            indent += last ? defaultStringIndent : $"|{defaultStringIndent}";

            for (int i = 0; i < node.Count; ++i)
            {
                PrintTree(builder, node[i], indent, i == node.Count - 1);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is SyntaxTree other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            // Should be different, but performance wise.
            return base.GetHashCode();
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

        public IEnumerator<List<SyntaxNode>> GetEnumerator()
        {
            return levels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return levels.GetEnumerator();
        }

        public void Clear()
        {
            levels.Clear();
        }

        public void Visit(SyntaxVisitor visitor)
        {
            compilationUnit.Visit(visitor);
        }
    }

    public class SyntaxTreeWalker
    {
        private readonly SyntaxTree tree;

        public SyntaxTreeWalker(SyntaxTree tree)
        {
            this.tree = tree;
        }

        public void Walk(Action<int, SyntaxNode> action)
        {
            RecursiveWalk(action, tree.CompilationUnit, 0);
        }

        private void RecursiveWalk(Action<int, SyntaxNode> action, SyntaxNode node, int level)
        {
            action(level, node);

            for (int i = 0; i < node.Count; ++i)
            {
                RecursiveWalk(action, node[i], level + 1);
            }
        }
    }

    public class SyntaxVisitor
    {
        private readonly SyntaxTree tree;
        private int stackDepth = 0;
        private int maxStackDepth = 8;

        private const string TTY = "[mscorlib]System.Console";
        private readonly string PrintStr = $"call void {TTY}::Write(string)";

        private readonly Stack<int> ifLabels;
        private int lastIfLabel;

        private int lastWhileLabel;

        private int lastAndLabel;
        private int lastOrlabel;

        public SyntaxVisitor(SyntaxTree tree)
        {
            this.tree = tree;
            ifLabels = new Stack<int>();
            ifLabels.Push(0);
        }

        public void Visit()
        {
            GenProlog();
            tree.Visit(this);
            GenEpilog();
        }

        private void GenProlog()
        {
            Emit(".assembly extern mscorlib { auto }");
            Emit(".assembly MiniCompiler { }");
            Emit($".module MiniCompiler.exe");

            /* From Expert .NET 2.0 IL Assembler */
        }

        private void GenEpilog()
        {
            Compiler.EmitAfterFirst(".entrypoint", $".maxstack {maxStackDepth + 1}");
        }

        public void Visit(CompilationUnit compilationUnit)
        {
            Emit(".method static void Program()");
            Emit("{");
            Emit(".entrypoint");

            Emit(".try");
            Emit("{");
            Emit();

            compilationUnit.Child.Visit(this);

            Emit("leave Return");
            Emit("}");

            Emit("catch [mscorlib]System.Exception");
            Emit("{");
            Emit("callvirt instance string [mscorlib]System.Exception::get_Message()");
            Emit(PrintStr);
            Emit("leave Return");
            Emit("}");

            Emit("Return: ret");
            Emit("}");
        }

        public void Visit(Block block)
        {
            Emit("{");

            for (int i = 0; i < block.Count; ++i)
            {
                block[i].Visit(this);
                PopIfHasValue(block[i]);
            }

            Emit("}");
        }

        public void Visit(VariableDeclaration declare)
        {
            Emit($".locals init ({declare.Type.ToCil()} '{declare.Name}')");
        }

        public void Visit(Write write)
        {
            var node = write.Child;
            if (node.Type == MiniType.Double)
            {
                EmitStackUp("call class [mscorlib]System.Globalization.CultureInfo " +
                    "[mscorlib]System.Globalization.CultureInfo::get_InvariantCulture()");
                EmitStackUp("ldstr \"{0:0.000000}\"");

                node.Visit(this);

                Emit("box [mscorlib]System.Double");
                EmitStackDown("call string [mscorlib]System.String:" +
                    ":Format(class [mscorlib]System.IFormatProvider, string, object)", 2);
                Emit(PrintStr);
                return;
            }

            node.Visit(this);
            EmitStackDown($"call void {TTY}::Write({node.Type.ToCil()})");
        }

        public void Visit(SimpleString simpleString)
        {
            EmitStackUp($"ldstr {simpleString.Value}");
        }

        public void Visit(VariableReference variableReference)
        {
            EmitStackUp($"ldloc '{variableReference.Declaration.Name}'");
        }

        public void Visit(Assign assign)
        {
            assign.Right.Visit(this);
            var left = assign.Left as VariableReference;
            ConvertToDoubleIfNeeded(left.Type, assign.Right.Type);

            Emit($"stloc '{left.Declaration.Name}'");
            Emit($"ldloc '{left.Declaration.Name}'");
        }

        public void Visit(Value value)
        {
            EmitStackUp($"ldc.{value.Type.ToPrimitive()} {value.Val}");
        }

        public void Visit(IfCond ifCond)
        {
            ifCond.Left.Visit(this);
            int label = lastIfLabel++;
            if (ifCond.HasElse)
            {
                ifLabels.Push(label);
                EmitStackDown($"brfalse ELSE_{label}");

                ifCond.Middle.Visit(this);
                PopIfHasValue(ifCond.Middle);

                Emit($"br OUT_IF_ELSE_{label}");
                ifCond.Right.Visit(this);
            }
            else
            {
                EmitStackDown($"brfalse OUT_IF_ELSE_{label}");
                ifCond.Middle.Visit(this);
                PopIfHasValue(ifCond.Middle);
            }
            Emit($"OUT_IF_ELSE_{label}: ");
        }

        public void Visit(ElseCond elseCond)
        {
            Emit($"ELSE_{ifLabels.Pop()}: ");
            elseCond.Child.Visit(this);
            PopIfHasValue(elseCond.Child);
        }

        public void Visit(WhileLoop whileLoop)
        {
            int label = lastWhileLabel++;
            Emit($"WHILE_{label}: ");
            whileLoop.Left.Visit(this);

            EmitStackDown($"brfalse OUT_WHILE_{label}");
            whileLoop.Right.Visit(this);
            PopIfHasValue(whileLoop.Right);
            Emit($"br WHILE_{label}");

            Emit($"OUT_WHILE_{label}: ");
        }

        public void Visit(Read read)
        {
            string cilStringType = MiniType.String.ToCil();
            var child = read.Child as VariableReference;
            EmitStackUp($"call {cilStringType} {TTY}::ReadLine()");
            Emit($"call {child.Type.ToCil()} [mscorlib]System.{child.Type.ToCSharp()}::Parse({cilStringType})");
            EmitStackDown($"stloc '{child.Declaration.Name}'");
        }

        public void Visit(Equals bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("ceq");
        }

        public void Visit(NotEquals bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            Emit("ceq");
            Emit($"ldc.{MiniType.Bool.ToPrimitive()} 0");
            EmitStackDown("ceq");
        }

        public void Visit(Greater bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("cgt");
        }

        public void Visit(GreaterOrEqual bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            Emit("clt");
            Emit($"ldc.{MiniType.Bool.ToPrimitive()} 0");
            EmitStackDown("ceq");
        }

        public void Visit(Less bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("clt");
        }

        public void Visit(LessOrEqual bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            Emit("cgt");
            Emit($"ldc.{MiniType.Bool.ToPrimitive()} 0");
            EmitStackDown("ceq");
        }

        public void Visit(Add bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("add");
        }

        public void Visit(Subtract bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("sub");
        }

        public void Visit(Multiplies bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("mul");
        }

        public void Visit(Divides bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("div");
        }

        public void Visit(BitOr bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("or");
        }

        public void Visit(BitAnd bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("and");
        }

        public void Visit(UnaryMinus uno)
        {
            uno.Left.Visit(this);
            Emit("neg");
        }

        public void Visit(LogicNegation uno)
        {
            uno.Left.Visit(this);
            EmitStackUp($"ldc.{MiniType.Bool.ToPrimitive()} 1");
            EmitStackDown("xor");
        }

        public void Visit(BitNegation uno)
        {
            uno.Left.Visit(this);
            Emit("not");
        }

        public void Visit(IntCast uno)
        {
            uno.Left.Visit(this);
            Emit("conv." + MiniType.Int.ToPrimitive());
        }

        public void Visit(DoubleCast uno)
        {
            uno.Left.Visit(this);
            Emit("conv." + MiniType.Double.ToPrimitive());
        }

        public void Visit(And and)
        {
            and.Left.Visit(this);
            int label = lastAndLabel++;
            EmitStackUp("dup");
            EmitStackDown($"brfalse AND_OUT_{label}");

            and.Right.Visit(this);
            EmitStackDown("and");

            Emit($"AND_OUT_{label}: ");
        }

        public void Visit(Or or)
        {
            or.Left.Visit(this);
            int label = lastOrlabel++;
            EmitStackUp("dup");
            EmitStackDown($"brtrue OR_OUT_{label}");

            or.Right.Visit(this);
            EmitStackDown("or");

            Emit($"OR_OUT_{label}: ");
        }

        public void Visit(Return @return)
        {
            Emit("leave Return");
        }

        private void PopIfHasValue(SyntaxNode node)
        {
            if (node.HasValue)
            {
                EmitStackDown("pop");
            }
        }

        private void ConvertToDoubleIfNeeded(MiniType a, MiniType b)
        {
            if (a == MiniType.Double && b != MiniType.Double)
            {
                Emit("conv." + MiniType.Double.ToPrimitive());
            }
        }

        private void Emit(string instr = null)
        {
            Compiler.EmitCode(instr);
        }

        private void EmitStackUp(string instr, int shift = 1)
        {
            Compiler.EmitCode(instr);
            stackDepth += shift;
            if (stackDepth > maxStackDepth)
            {
                maxStackDepth = stackDepth;
            }
        }

        private void EmitStackDown(string instr, int shift = 1)
        {
            Compiler.EmitCode(instr);
            stackDepth -= shift;
        }

        private void PrepareBinaryOperation(TypeNode left, TypeNode right)
        {
            left.Visit(this);
            ConvertToDoubleIfNeeded(right.Type, left.Type);

            right.Visit(this);
            ConvertToDoubleIfNeeded(left.Type, right.Type);
        }
    }

    public enum MiniType
    {
        Unknown = 0,
        Int = Token.IntKey,
        Bool = Token.BoolKey,
        Double = Token.DoubleKey,
        String = Token.String * -1,
    }

    public class EmptyScope : IScope
    {
        public bool IsPresent(string name)
        {
            return false;
        }

        public void AddToScope(VariableDeclaration variable)
        {
        }

        public bool TryGetVariable(string id, ref VariableDeclaration variable)
        {
            return false;
        }

        public IScope GetParentScope()
        {
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is EmptyScope;
        }

        public override int GetHashCode()
        {
            return typeof(EmptyScope).GetHashCode();
        }
    }

    public interface IScope
    {
        bool IsPresent(string name);

        void AddToScope(VariableDeclaration variable);

        bool TryGetVariable(string id, ref VariableDeclaration variable);

        IScope GetParentScope();
    }

    public class SubordinateScope : IScope
    {
        private readonly IScope parentScope;
        private readonly Dictionary<string, VariableDeclaration> variables;

        public SubordinateScope(IScope parentScope)
        {
            this.parentScope = parentScope;
            this.variables = new Dictionary<string, VariableDeclaration>();
        }

        public bool IsPresent(string name)
        {
            return variables.ContainsKey(name);
        }

        public void AddToScope(VariableDeclaration variable)
        {
            variables.Add(variable.Name, variable);
        }

        public bool TryGetVariable(string id, ref VariableDeclaration variable)
        {
            return variables.TryGetValue(id, out variable) || parentScope.TryGetVariable(id, ref variable);
        }

        public IScope GetParentScope()
        {
            return parentScope;
        }

        public override bool Equals(object obj)
        {
            return obj is SubordinateScope scope && variables.Count == scope.variables.Count
                && parentScope.Equals(scope.parentScope);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + variables.Count;
            hash = hash * 23 + parentScope.GetHashCode();

            return hash;
        }
    }

    public class Value : TypeNode
    {
        public Value(MiniType type, string val, LexLocation loc = null)
        {
            Type = type;
            Val = val;
            Location = loc;
        }

        public string Val { get; private set; }

        protected override bool IsNodeEqual(SyntaxNode node)
        {
            if (base.IsNodeEqual(node))
            {
                var other = (Value)node;
                return Val == other.Val;
            }

            return false;
        }

        protected override int GetNodeHash()
        {
            return CombineHashCode(base.GetNodeHash(), Val);
        }

        public override string ToString()
        {
            return base.ToString() + ": " + Val;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class VariableDeclaration : TypeNode
    {
        public string Name { get; private set; }
        public IScope Scope { get; private set; }

        public VariableDeclaration(string name, IScope scope, MiniType type, LexLocation loc = null)
        {
            Name = name;
            Scope = scope;
            scope.AddToScope(this);
            Type = type;
            Location = loc;
        }

        public override bool HasValue => false;

        protected override bool IsNodeEqual(SyntaxNode node)
        {
            if (base.IsNodeEqual(node))
            {
                var other = (VariableDeclaration)node;
                return Name == other.Name && Scope.Equals(other.Scope);
            }

            return false;
        }

        protected override int GetNodeHash()
        {
            return CombineHashCode(base.GetNodeHash(), Name, Scope);
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {Type} {Name};";
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class VariableReference : TypeNode
    {
        public VariableReference(VariableDeclaration declar, LexLocation loc = null)
        {
            Declaration = declar;
            Location = loc;
        }

        public VariableDeclaration Declaration { get; }

        public override MiniType Type
        {
            get => Declaration.Type;
            protected set => throw new InvalidOperationException("You cannot set type of declared variable");
        }

        protected override bool IsNodeEqual(SyntaxNode node)
        {
            if (base.IsNodeEqual(node))
            {
                var other = (VariableReference)node;
                return Declaration.Equals(other.Declaration);
            }

            return false;
        }

        protected override int GetNodeHash()
        {
            return CombineHashCode(base.GetNodeHash(), Declaration);
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {Declaration.Name}";
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

}
