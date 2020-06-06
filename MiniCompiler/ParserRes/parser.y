
%output=.\ParserRes\Parser.cs
%partial
%tokentype Token

%using System.Linq;
%using MiniCompiler.Syntax;
%using MiniCompiler.Syntax.General;
%using MiniCompiler.Syntax.Declaration;
%using MiniCompiler.Syntax.Declaration.Scopes;
%using MiniCompiler.Syntax.Operators;

%namespace MiniCompiler

%union
{
    public Type             type;
    public string           val;
    public SyntaxNode       node;
    public List<SyntaxNode> orphans;
}

%token Program OpenBrace CloseBrace Return Colon True False Eof Error 
%token Write Assign Plus Minus Multiplies Divides OpenPar ClosePar 
%token IntKey DoubleKey BoolKey
%token <val> IntVal DoubleVal Id

%type <orphans> content
%type <node> block none
%type <node> instr assign declar 
%type <node> exp
%type <type> declarKey

%%

start         : Program block Eof
                {
                    var unit = new CompilationUnit(Loc);
                    unit.Add($2);
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
                    newBlock.SetChildren($3);
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
              ;
instr         : declar { $$ = $1; }
              | assign { $$ = $1; }
              ;
assign        : Id Assign exp
              ;
/* IDENTIFIERS -----------------------------------------------------------------------------------------------*/
declar        : declarKey Id Colon
                {
                    if($1 != Type.Unknown && currentScope.AddToScope($2))
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
              | declarKey Id
                {
                    Error("Missing semicolon at col: {0}", Loc.EndColumn);
                }
              | Id Id 
                {
                    Error("Uncrecognized type.");
                }   
              ;
declarKey     : IntKey  { $$ = Type.Int; }
              | BoolKey { $$ = Type.Bool; }
              | DoubleKey { $$ = Type.Double; }
              | Error { $$ = Type.Unknown; }
              ; 

/* ARITHEMITIC ---------------------------------------------------------------------------------------------- */ 
                /* TODO */
exp           : Plus Minus Divides
              ;
/* Other  ---------------------------------------------------------------------------------------------- */ 
end           : Colon
              | Eof
                {
                    Error("Syntax error - unexpected symbol Eof.");
                    YYABORT;
                }
              ;
none          :       { $$ = null; }
              ;


%%

/* HELPER FUNCTIONS ------------------------------------------------------------------------------------------------*/