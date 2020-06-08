
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
%token Add Minus Multiplies Divides OpenPar ClosePar 
%token Colon Endl Eof Error 

%type <orphans> content
%type <node> block none
%type <node> instr 
%type <typeNode> declar exp logic_exp relat_exp addit_exp mult_exp bit_exp unary_exp factor_exp
%type <type> declar_key 
%type <type> unrecon_word

%%

start         : mult_endl Program mult_endl block mult_endl Eof
                {
                    var unit = new CompilationUnit(Loc);
                    unit.Child = $4;
                    GenerateCode(unit);
                    YYACCEPT;
                }
              | mult_endl Program error Eof
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
block         : enter_scope content leave_scope
                {
                    var newBlock = new Block(Loc);
                    newBlock.AddChildren($2);
                    $$ = newBlock;
                }
              | enter_scope content error Eof
                {
                    Error("No brace matching.");
                    YYABORT;
                }
              ;
enter_scope   : OpenBrace 
                {
                    EnterScope(new SubordinateScope(currentScope));
                }
              ;
leave_scope   : CloseBrace
                {
                    LeaveScope();
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
              | content Endl 
              | content Colon
              ;
instr         : declar Colon { $$ = $1; }
              | exp Colon { $$ = $1; }

                // Errors
             // | declar error Endl { Error("Missing semicolon at col: {0}", @1.EndColumn); }
             // | declar_key error Endl { Error("Invalid declaration."); }

            //  | exp Error { Error("Invalid token at col: {0}", @2.EndColumn); }
            //  | exp error Endl { Error("Missing semicolon at col: {0}", @1.EndColumn); }
            //  | exp error Colon { Error("Invalid statement."); }
                | error Endl { Error("Invalid statement"); }
              ;
/* IDENTIFIERS -----------------------------------------------------------------------------------------------*/
declar        : declar_key Id
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
              | declar_key unrecon_word
                {
                    Error("Identifier is restricted keyword or contains prohibited characters.");
                }
              ;
declar_key    : IntKey  { $$ = Type.Int; }
              | BoolKey { $$ = Type.Bool; }
              | DoubleKey { $$ = Type.Double; }
              ;
/* ARITHEMITIC ---------------------------------------------------------------------------------------------- */ 

exp           : Id Assign exp
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
              | logic_exp
              ;
logic_exp     : logic_exp Or  relat_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | logic_exp And relat_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | relat_exp
              ;
relat_exp     : relat_exp Equals         addit_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | relat_exp NotEquals      addit_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | relat_exp Greater        addit_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | relat_exp GreaterOrEqual addit_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | relat_exp Less           addit_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | relat_exp LessOrEqual    addit_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | addit_exp
              ;
addit_exp     : addit_exp Add   mult_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | addit_exp Minus mult_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | mult_exp
              ;
mult_exp      : mult_exp Multiplies bit_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | mult_exp Divides    bit_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | bit_exp
              ;
bit_exp       : bit_exp BitOr  unary_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | bit_exp BitAnd unary_exp  { $$ = TryCreateOperator($2.token, $3, $1); }
              | unary_exp
              ;
unary_exp     : Minus                      factor_exp { $$ = TryCreateOperator($1.token, $2); }
              | Negation                   factor_exp { $$ = TryCreateOperator($1.token, $2); }
              | BitNegation                factor_exp { $$ = TryCreateOperator($1.token, $2); }
              | OpenPar IntKey ClosePar    factor_exp { $$ = TryCreateOperator($2.token, $4); }
              | OpenPar DoubleKey ClosePar factor_exp { $$ = TryCreateOperator($2.token, $4); }
              | factor_exp
              ;
factor_exp    : OpenPar exp ClosePar { $$ = $2; }
              | IntVal
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
unrecon_word  : Id Error
              | Error
              | unrecon_word Error
              | unrecon_word Id
              ;
/* OTHER  ---------------------------------------------------------------------------------------------- */ 
mult_endl     : /* empty */
              | mult_endl Endl
              ;
end           : Colon
              | Endl
              | Eof
              ;
end_no_eof    : Colon
              | Endl
              ;
end_no_colon  : Endl
              | Eof
              ;

any_operator  : 
              ;
none          : { $$ = null; }
              ;


%%

/* HELPER FUNCTIONS ------------------------------------------------------------------------------------------------*/