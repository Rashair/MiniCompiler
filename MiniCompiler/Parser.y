
// Uwaga: W wywołaniu generatora gppg należy użyć opcji /gplex
%output=Parser.cs

%using MiniCompiler.Syntax;

%namespace MiniCompiler

%union
{
    public char    type;
    public string  val;
}

%token Program OpenBrace CloseBrace Return Colon Endl Eof Error
%token Write Assign Plus Minus Multiplies Divides OpenPar ClosePar 
%token <val> IntA DoubleA BoolA Id
%token <val> IntVal DoubleVal True False

%type <type> content newline block
%type <type> declare
%type <type> exp term factor

%%

start         : Program block start
                {
                }
              | newline start
              | Eof 
                {
                   GenerateCode();
                   YYACCEPT;
                }
              | error Eof
                {
                    Error("'program' statement required.");
                    YYABORT;
                }
              ;
block     : newline OpenBrace content CloseBrace
              | OpenBrace content CloseBrace
                {
                }
              | OpenBrace block CloseBrace
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


content       : newline content
              |
              ;
declare       : IntA Id Colon
              | DoubleA Id Colon
              ;
newline       : Endl { ++lineNum; }
              ;

end           : Colon
              | Eof
                {
                    Error("Syntax error - unexpected symbol Eof.");
                    YYABORT;
                }
              ;

/* IDENTIFIERS -----------------------------------------------------------------------------------------------*/


/* ARITHEMITIC ---------------------------------------------------------------------------------------------- */ 
exp           : exp Plus term
                   { $$ = BinaryOpGenCode(Tokens.Plus, $1, $3); }
              | exp Minus term
                   { $$ = BinaryOpGenCode(Tokens.Minus, $1, $3); }
              | term
                   { $$ = $1; }
              ;
              
term          : term Multiplies factor
                   { $$ = BinaryOpGenCode(Tokens.Multiplies, $1, $3); }
              | term Divides factor
                   { $$ = BinaryOpGenCode(Tokens.Divides, $1, $3); }
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

private int lineNum = 1;
private SyntaxTree tree;
private SyntaxVisitor visitor;

public Parser(Scanner scanner) : base(scanner) 
{ 
    this.tree = new SyntaxTree();
    this.visitor = new SyntaxVisitor(tree);
}

private void GenerateCode()
{
    this.visitor.Visit();
}

private void Error(string msg) 
{
      Console.WriteLine($"  line {lineNum, 3}: {msg}");
      ++Compiler.errors;
      yyerrok();
}

private char BinaryOpGenCode(Tokens t, char type1, char type2)
{
    char type = ( type1=='i' && type2=='i' ) ? 'i' : 'r' ;
    if ( type1!=type )
        {
        Compiler.EmitCode("stloc temp");
        Compiler.EmitCode("conv.r8");
        Compiler.EmitCode("ldloc temp");
        }
    if ( type2!=type )
        Compiler.EmitCode("conv.r8");
    switch ( t )
        {
        case Tokens.Plus:
            Compiler.EmitCode("add");
            break;
        case Tokens.Minus:
            Compiler.EmitCode("sub");
            break;
        case Tokens.Multiplies:
            Compiler.EmitCode("mul");
            break;
        case Tokens.Divides:
            Compiler.EmitCode("div");
            break;
        default:
            Console.WriteLine($"  line {lineNum,3}:  internal gencode error");
            ++Compiler.errors;
            break;
        }
    return type;
}

