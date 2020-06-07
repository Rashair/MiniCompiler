
%output=.\ParserRes\Parser.cs
%partial
%tokentype Token

%using System.Linq;
%using MiniCompiler.Syntax;
%using MiniCompiler.Syntax.Abstract;
%using MiniCompiler.Syntax.General;
%using MiniCompiler.Syntax.Variables;
%using MiniCompiler.Syntax.Variables.Scopes;
%using MiniCompiler.Syntax.Operators;
%using MiniCompiler.Syntax.Operators.Assignment

%namespace MiniCompiler

%union
{
    public Type             type;
    public string           val;
    public SyntaxNode       node;
    public TypeNode         typeNode;
    public List<SyntaxNode> orphans;
}

%token Program OpenBrace CloseBrace Return 
%token If Else While Read Write True False
%token IntKey DoubleKey BoolKey
%token <val> IntVal DoubleVal Id
%token Assign Or And BitOr BitAnd Negation BitNegation
%token Equals NotEquals Greater GreaterOrEqual Less LessOrEqual
%token Plus Minus Multiplies Divides OpenPar ClosePar 
%token Colon Eof Error 

%type <orphans> content
%type <node> block none
%type <node> instr 
%type <typeNode> exp assign declar 
%type <type> declarKey 
%type <type> unreconWord

%%

start         : Program block Eof
                {
                    var unit = new CompilationUnit(Loc);
                    unit.Child = $2;
                    GenerateCode(unit);
                    YYACCEPT;
                }
              | Program error Eof
                {
                    Error("Braces expected.");
                    YYABORT;
                }

              | error Eof
                {
                    Error("'program' statement required.");
                    YYABORT;
                }
              ;
block         : OpenBrace
                {
                     EnterScope(new SubordinateScope(currentScope));
                }
                content CloseBrace
                {
                    LeaveScope();

                    var newBlock = new Block(Loc);
                    newBlock.AddChildren($3);
                    $$ = newBlock;
                }
              | OpenBrace error Eof
                {
                    Error("No brace matching.");
                    YYABORT;
                }
              ;
content       : none
                {
                    $$ = new List<SyntaxNode>();
                }
              | content instr
                {
                    ($1).Add($2);
                    $$ = $1;
                }
              | content block
                {
                    ($1).Add($2);
                    $$ = $1;
                }
              | content Colon
              | Eof { Error("Unexpected end of file."); }
              ;
instr         : declar { $$ = $1; }
              | assign { $$ = $1; }
              | declarKey error end { Error("Unexpected statement."); }
              ;
assign        : Id Assign exp
                {
                    VariableDeclaration declar = null;
                    if(!currentScope.TryGetVariable($1, ref declar))
                    {
                        Error("Variable {0} not declared.", $1);
                        return;
                    }
                    
                    var expType = $3.Type;
                    // TODO: Retrieve this automatically
                    var token = Token.Assign;
                    if(!Operator.CanUse(token, declar.Type, expType))
                    {
                        Error("Cannot assign {0} to {1}.", expType, declar.Type);
                        return;
                    }
                    
                    var oper = Operator.Create(token, declar.Type, expType, Loc);
                    oper.Left = new VariableReference(declar, @1);
                    oper.Right = $3;
                    $$ = oper;
                }
              ;
/* IDENTIFIERS -----------------------------------------------------------------------------------------------*/
declar        : declarKey Id Colon
                {
                    if($1 != Type.Unknown && !currentScope.IsPresent($2))
                    {
                        $$ = new VariableDeclaration($2, currentScope, $1, Loc);
                    }
                    else if($1 == Type.Unknown)
                    {
                        Error("Unrecognized type");
                    }
                    else
                    {
                        Error("Variable '{0}' was already declared in this scope.", $2);
                    }
                }
              | declarKey Id error end /* TODO: endl? */
                {
                    Error("Missing semicolon at col: {0}", @2.EndColumn);
                }
              | Id Id Colon
                {
                    Error("Uncrecognized type.");
                }
              | declarKey unreconWord Colon
                {
                    Error("Identifier is restricted keyword or contains prohibited characters.");
                }
              | Id Error
                {
                    Error("Identifier contains unexpected character.");
                }
              ;
declarKey     : IntKey  { $$ = Type.Int; }
              | BoolKey { $$ = Type.Bool; }
              | DoubleKey { $$ = Type.Double; }
              ; 

/* ARITHEMITIC ---------------------------------------------------------------------------------------------- */ 
                /* TODO */
exp           : IntVal
                {
                    $$ = new Value(Type.Int, $1, Loc);
                }
              | assign { $$ = $1; }
              ;

/* ERRORS  ---------------------------------------------------------------------------------------------- */ 
unreconWord : Id Error
            | Error
            | unreconWord Error
            | unreconWord Id
            ;
/* OTHER  ---------------------------------------------------------------------------------------------- */ 
end           : Colon
              | CloseBrace
              | OpenBrace
              | Eof
              ;
none          :       { $$ = null; }
              ;


%%

/* HELPER FUNCTIONS ------------------------------------------------------------------------------------------------*/