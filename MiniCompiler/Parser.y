
// Uwaga: W wywołaniu generatora gppg należy użyć opcji /gplex
%output=Parser.cs

%namespace MiniCompiler

%union
{
    public string  val;
    public char    type;
}

%token Program OpenBrace CloseBrace Return Colon Endl Eof Error
%token Write Assign Plus Minus Multiplies Divides OpenPar ClosePar 
%token <val> IntA DoubleA BoolA Id
%token <val> IntVal DoubleVal True False

%type <type> content newline blokInstr
%type <type> declare
%type <type> exp term factor

%%

start         : Program blokInstr start
              | newline start
              | Eof 
                {
                   Compiler.EmitCode("// linia {0,3} :  "+ Compiler.sourceLines[lineNum - 1], lineNum);
                   Compiler.EmitCode("ldstr \"\\nEnd of execution\\n\"");
                   Compiler.EmitCode("call void [mscorlib]System.Console::WriteLine(string)");
                   Compiler.EmitCode("");
                   YYACCEPT;
                }
              | error Eof
                {
                    Console.WriteLine("  line {0,3}: 'program' statement required.", lineNum);
                    ++Compiler.errors;
                    yyerrok();
                    YYABORT;
                }
              ;
blokInstr     : newline OpenBrace content CloseBrace
              | OpenBrace content CloseBrace
                {
                }
              | OpenBrace blokInstr CloseBrace
              | OpenBrace error Eof
                {
                    Console.WriteLine("  line {0,3}: No brace matching.", lineNum);
                    ++Compiler.errors;
                    yyerrok();
                    YYABORT;
                }
              | error Eof
                {
                    Console.WriteLine("  line {0,3}: Braces expected.", lineNum);
                    ++Compiler.errors;
                    yyerrok();
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
                    Console.WriteLine("  line {0,3}:  syntax error - unexpected symbol Eof", lineNum);
                    ++Compiler.errors;
                    yyerrok();
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

int lineNum = 1;

public Parser(Scanner scanner) : base(scanner) { }

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
