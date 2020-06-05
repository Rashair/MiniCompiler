
%output=.\ParserRes\Parser.cs
%partial
%tokentype Token

%using System.Linq;
%using MiniCompiler.Syntax;
%using MiniCompiler.Syntax.General;
%using MiniCompiler.Syntax.Declaration;

%namespace MiniCompiler

%union
{
    public char             type;
    public string           val;
    public SyntaxNode       node;
    public List<SyntaxNode> orphans;
}

%token Program OpenBrace CloseBrace Return Colon Eof Error
%token Write Assign Plus Minus Multiplies Divides OpenPar ClosePar 
%token <val> IntKey DoubleKey BoolKey Id
%token <val> IntVal DoubleVal True False

%type <orphans> content
%type <node> block none
%type <node> declaration
%type <type> exp term factor

%%

start         : Program block Eof
                {
                    var unit = new CompilationUnit(Loc);
                    unit.Add($2);
                    GenerateCode(unit);
                    YYACCEPT;
                }
              | error Eof
                {
                    Error("'program' statement required.");
                    YYABORT;
                }
              ;
block         : OpenBrace
                content CloseBrace
                {
                    var newBlock = new Block(Loc);
                    newBlock.SetChildren($2);
                    $$ = newBlock;
                }
              | OpenBrace error Eof
                {
                    Error("No brace matching.");
                    YYABORT;
                }
              | error Eof
                {
                    Error("Braces expected.");
                    YYABORT;
                }
              ;
              /* TODO: Fix tree creation ^^ */ 
content       : none
                {
                    $$ = new List<SyntaxNode>();
                }
              | content declaration
                {
                    ($1).Add($2);
                    $$ = $1;
                }
              | content block
                {
                    ($1).Add($2);
                    $$ = $1;
                }
              ;
declaration   : IntKey Id Colon
                {
                    $$ = new VariableDeclaration(Loc);
                }
              | BoolKey Id Colon
                {
                    $$ = new VariableDeclaration(Loc);
                }
              | DoubleKey Id Colon
                {
                    $$ = new VariableDeclaration(Loc);
                }
              ;

end           : Colon
              | Eof
                {
                    Error("Syntax error - unexpected symbol Eof.");
                    YYABORT;
                }
              ;
none          : { $$ = null; }
              ;

/* IDENTIFIERS -----------------------------------------------------------------------------------------------*/


/* ARITHEMITIC ---------------------------------------------------------------------------------------------- */ 
exp           : exp Plus term
                   { $$ = BinaryOpGenCode(Token.Plus, $1, $3); }
              | exp Minus term
                   { $$ = BinaryOpGenCode(Token.Minus, $1, $3); }
              | term
                   { $$ = $1; }
              ;              
term          : term Multiplies factor
                   { $$ = BinaryOpGenCode(Token.Multiplies, $1, $3); }
              | term Divides factor
                   { $$ = BinaryOpGenCode(Token.Divides, $1, $3); }
              | factor
                   { $$ = $1; }
              ;
factor        : OpenPar exp ClosePar
                   { $$ = $2; }
              | IntVal
                   {
                   Compiler.EmitCode("ldc.i4 {0}",int.Parse($1));
                   $$ = 'i'; 
                   }
              | DoubleVal
                   {
                   double d = double.Parse($1,System.Globalization.CultureInfo.InvariantCulture) ;
                   Compiler.EmitCode(string.Format(System.Globalization.CultureInfo.InvariantCulture,"ldc.r8 {0}",d));
                   $$ = 'r'; 
                   }
              | Id
                {
                    Compiler.EmitCode("ldloc _{0}{1}", $1[0]=='@'?'i':'r', $1[1]);
                    $$ = $1[0]=='@'?'i':'r';
                }
              ;
%%

/* HELPER FUNCTIONS ------------------------------------------------------------------------------------------------*/