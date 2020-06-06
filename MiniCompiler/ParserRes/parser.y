
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

%token Program OpenBrace CloseBrace Return 
%token If Else While Read Write True False
%token IntKey DoubleKey BoolKey
%token <val> IntVal DoubleVal Id
%token Assign Or And BitOr BitAnd Negation BitNegation
%token Equals NotEquals Greater GreaterOrEqual Less LessOrEqual
%token Plus Minus Multiplies Divides OpenPar ClosePar 
%token Colon Eof Error 

%type <orphans> content
%type <orphans> exp
%type <node> block none
%type <node> instr assign declar 
%type <type> declarKey
%type <type> unreconWord

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
              | Eof { Error("Unexpected end of file."); }
              ;
instr         : declar { $$ = $1; }
              | assign { $$ = $1; }
              | declarKey error end { Error("Unexpected statement."); }
              ;
assign        : Id Assign exp
                {
                    Type type = Type.Unknown;
                    if(!currentScope.TryGetType($1, ref type))
                    {
                        Error("Variable {0} not declared.", $1);
                    }
                    // TODO: Must check final type of expression and collect childrent to assign
                }
              ;
/* IDENTIFIERS -----------------------------------------------------------------------------------------------*/
declar        : declarKey Id Colon
                {
                    if($1 != Type.Unknown && currentScope.AddToScope($2, $1))
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
exp           : Plus Minus Divides
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