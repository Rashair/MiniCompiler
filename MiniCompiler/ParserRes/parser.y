
%output=.\ParserRes\Parser.cs
%partial
%tokentype Token

%using System.Linq;
%using MiniCompiler.Extensions;
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
    public Token            token;
    public Type             type;
    public string           val;
    public SyntaxNode       node;
    public TypeNode         typeNode;
    public List<SyntaxNode> orphans;
}

%token Program OpenBrace CloseBrace Return 
%token If Else While Read Write
%token IntKey DoubleKey BoolKey
%token <val> True False IntVal DoubleVal Id
%token Assign Or And BitOr BitAnd Negation BitNegation
%token Equals NotEquals Greater GreaterOrEqual Less LessOrEqual
%token Plus Minus Multiplies Divides OpenPar ClosePar 
%token Colon Eof Error 

%type <orphans> content
%type <node> block none
%type <node> instr 
%type <typeNode> exp declar assign factor
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
instr         : declar Colon { $$ = $1; }
              | exp Colon { $$ = $1; }
              | declar error end { Error("Missing semicolon at col: {0}", @1.EndColumn); }
              | exp error end { Error("Missing semicolon at col: {0}", @1.EndColumn); }
              | declarKey error end { Error("Unexpected statement."); }
              ;
/* IDENTIFIERS -----------------------------------------------------------------------------------------------*/
declar        : declarKey Id
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
              | Id Id
                {
                    Error("Uncrecognized type.");
                }
              | declarKey unreconWord
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
assign        : Id Assign exp
                {
                    if($3.Type == Type.Unknown)
                    {
                        $$ = $3;
                        return;
                    }

                    VariableDeclaration declar = null;
                    if(!currentScope.TryGetVariable($1, ref declar))
                    {
                        Error("Variable {0} not declared.", $1);
                        return;
                    }
                    var reference = new VariableReference(declar, @1);
            
                    $$ = TryCreateOperator($2.token, reference, $3);
                }
              ;
/* ARITHEMITIC ---------------------------------------------------------------------------------------------- */ 
                /* TODO */
exp           : Minus exp
                {
                    $$ = TryCreateOperator($1.token, $2);
                }
              | Negation exp
              | BitNegation exp
              | OpenPar IntKey ClosePar exp
              | OpenPar DoubleKey ClosePar exp
              | assign
              | factor
              ;
factor        : IntVal
                {
                    $$ = CreateValue();
                }
              | DoubleVal
                {
                    $$ = CreateValue();
                }
              | True
                {
                    $$ = CreateValue();
                }
              | False
                {
                    $$ = CreateValue();
                }
              | Id 
                {
                    VariableDeclaration declar = null;
                    if(!currentScope.TryGetVariable($1, ref declar))
                    {
                        Error("Variable {0} not declared.", $1);
                        return;
                    }
                    $$ = new VariableReference(declar, @1);
                }
              ;
/* ERRORS  ---------------------------------------------------------------------------------------------- */ 
unreconWord : Id Error
            | Error
            | unreconWord Error
            | unreconWord Id
            ;
/* OTHER  ---------------------------------------------------------------------------------------------- */ 
// TODO: Discard 2 errors 
end           : Colon
              | CloseBrace
              | OpenBrace
              | Eof
              ;
none          :       { $$ = null; }
              ;


%%

/* HELPER FUNCTIONS ------------------------------------------------------------------------------------------------*/