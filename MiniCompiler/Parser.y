
// Uwaga: W wywołaniu generatora gppg należy użyć opcji /gplex
%output=Parser.cs

%using MiniCompiler.Syntax;
%using MiniCompiler.Syntax.General;
%using MiniCompiler.Syntax.Declaration;

%namespace MiniCompiler

%union
{
    public char    type;
    public string  val;
}

%token Program OpenBrace CloseBrace Return Colon Eof Error
%token Write Assign Plus Minus Multiplies Divides OpenPar ClosePar 
%token <val> IntKey DoubleKey BoolKey Id
%token <val> IntVal DoubleVal True False

%type <type> content block
%type <type> declaration
%type <type> exp term factor

%%

start         : Program block Eof
                {
                    var unit = new CompilationUnit(Loc);
                    AddChildren(unit);

                    GenerateCode(unit);
                    YYACCEPT;
                }
              | error Eof
                {
                    Error("'program' statement required.");
                    YYABORT;
                }
              ;
block         : OpenBrace content CloseBrace
                {
                    var node = new Block(Loc);
                    AddChildren(node);
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
content       : 
              | content declaration
                {
                    childrenWaitingForAdoption.Add(new VariableDeclaration(Loc));
                }
              | content block
              ;
declaration   : IntKey Id Colon
              | BoolKey Id Colon
              | DoubleKey Id Colon
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

private List<SyntaxNode> childrenWaitingForAdoption;
private SyntaxTree tree;

public Parser(Scanner scanner) : base(scanner) 
{ 
    childrenWaitingForAdoption = new List<SyntaxNode>();
}

public LexLocation Loc => CurrentLocationSpan;

private void AddChildren(SyntaxNode node)
{
    node.SetChildren(childrenWaitingForAdoption);
    childrenWaitingForAdoption = new List<SyntaxNode>() { node };
}

private void GenerateCode(CompilationUnit unit)
{
    tree = new SyntaxTree(unit);
    var visitor = new SyntaxVisitor(tree);
    visitor.Visit();
}

private void Error(string msg) 
{
      Console.WriteLine($"  line {Loc.StartLine, 3}: {msg}");
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
            Console.WriteLine($"  line {Loc.StartLine,3}:  internal gencode error");
            ++Compiler.errors;
            break;
        }
    return type;
}

