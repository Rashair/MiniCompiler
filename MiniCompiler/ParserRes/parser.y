
%output=.\ParserRes\Parser.cs
%partial
%tokentype Token

%using System.Linq;
%using MiniCompiler.Extensions;
%using MiniCompiler.Syntax;
%using MiniCompiler.Syntax.Abstract;
%using MiniCompiler.Syntax.General;
%using MiniCompiler.Syntax.IOStream;
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

%token While Read Write
%nonassoc If 
%nonassoc Else
%token <val> True False IntVal DoubleVal Id String
%token Assign Or And BitOr BitAnd Negation BitNegation
%token Equals NotEquals Greater GreaterOrEqual Less LessOrEqual
%token Add Minus Multiplies Divides OpenPar ClosePar 
%nonassoc Eof Endl Error Colon
 

%token IntKey DoubleKey BoolKey
%token OpenBrace CloseBrace Return 
%token Program 


%type <orphans> content content_declar
%type <node> block none
%type <node> instr instr_block
%type <node> read write
%type <node> if_else if_stmnt
%type <node> while
%type <typeNode> declar_colon declar
%type <typeNode> exp logic_exp relat_exp addit_exp mult_exp bit_exp unary_exp factor_exp
%type <type> declar_key 
%type <type> unrecon_word

%%

start           : Program block Eof
                  {
                      var unit = new CompilationUnit(Loc);
                      unit.Child = $2;
                      GenerateCode(unit);
                      YYACCEPT;
                  }
                | Program error_eof
                  {
                      Error("Braces expected.");
                      YYABORT;
                  }
                
                | error_eof
                  {
                      Error("'program' statement required.");
                      YYABORT;
                  }
                ;
block           : enter_scope content leave_scope
                  {
                      var newBlock = new Block(Loc);
                      newBlock.AddChildren($2);
                      $$ = newBlock;
                  }
                ;
enter_scope     : OpenBrace 
                  {
                      EnterScope(new SubordinateScope(currentScope));
                  }
                ;
leave_scope     : CloseBrace
                  {
                      LeaveScope();
                  }
                ;
content_declar  : content_declar declar_colon
                  { 
                        ($1).Add($2);
                        $$ = $1;
                  }             
                | none
                  {
                      $$ = new List<SyntaxNode>();
                  }
                ;
content         : content instr
                  {
                     if($2.ShouldInclude)
                     {
                        ($1).Add($2);
                     }
                     $$ = $1;
                  }
                | content block
                  {
                      ($1).Add($2);
                      $$ = $1;
                  }
                | content_declar { $$ = $1;}
                ;
instr           : exp Colon { $$ = $1; }
                | if_else
                | while
                | read Colon 
                | write Colon
                | Return Colon { $$ = new Return(Loc); }
                
                // Errors
                | exp great_err error_colon { Error("Invalid tokens at col: {0}", @2.StartColumn); }
                | exp error_colon { Error("Invalid expression"); }
                | error_colon { Error("Invalid statement.");  }
                | error_eof { Error("Unexpected end of file.");  }
                ;
instr_block     : block
                | instr
                ;
/* IDENTIFIERS  --------------------------------------------------------------------------------------------------*/
declar_colon    : declar Colon
                  {
                      $$ = $1;
                  }
                | declar error_colon { Error("Missing semicolon at {0}", @2.EndColumn); }
                | declar error_eof { Error("Invalid declaration."); }
                | declar_key error_colon { Error("Invalid declaration."); }
                | declar_key error_eof { Error("Invalid declaration."); }
                ;
declar          : declar_key Id
                  {
                      var name = $2;
                      if($1 != Type.Unknown && !currentScope.IsPresent(name))
                      {
                          $$ = new VariableDeclaration(name, currentScope, $1, Loc);
                      }
                      else if($1 == Type.Unknown)
                      {
                          Error("Unrecognized type");
                      }
                      else
                      {
                          Error("Variable '{0}' was already declared in this scope.", name);
                      }
                  }
                | declar_key unrecon_word { Error("Invalid character at: ", @2.StartColumn); }
                ;
declar_key      : IntKey  { $$ = Type.Int; }
                | BoolKey { $$ = Type.Bool; }
                | DoubleKey { $$ = Type.Double; }
                ;
/* IF ELSE  --------------------------------------------------------------------------------------------------*/
if_else         : if_stmnt %prec If
                  {
                    $$ = $1;
                  }
                | if_stmnt Else instr_block
                  {                  
                    var loc = @2.Merge(@3);
                    $1[2] = new ElseCond(loc)
                    {
                        Child = $3
                    };
                  }
                ;
if_stmnt        : If OpenPar exp ClosePar instr_block
                  {
                    if($3.Type != Type.Bool)
                    {
                        Error("Expression in IF statement must evaluate to bool.");
                        EndRecovery();
                    }
                    else
                    {
                        $$ = new IfCond(Loc)
                        {
                            Left = $3,
                            Middle = $5,
                        };
                    }
                  }
                ;
/* WHILE  --------------------------------------------------------------------------------------------------*/
while           : While OpenPar exp ClosePar instr_block
                  {
                    if($3.Type != Type.Bool)
                    {
                        Error("Expression in WHILE statement must evaluate to bool.");
                        EndRecovery();
                    }
                    else
                    {
                        $$ = new WhileLoop(Loc)
                        {
                            Left = $3,
                            Right = $5,
                        };
                    }
                  }
                ;
/* OUTPUT  --------------------------------------------------------------------------------------------------*/
read            : Read Id 
                  {
                    var variable = TryCreateVariableReference($2, @2);
                    if(variable.Type != Type.Unknown)
                    {
                        $$ = new Read(@1) { Child = variable };
                    }
                    else
                    {
                        $$ = new EmptyNode(@1);
                        EndRecovery();
                    }
                  }
                ;
write           : Write exp     
                  { 
                    if($2.Type != Type.Unknown)
                    {
                        $$ = new Write(@1) { Child = $2 };
                    }
                    else
                    {
                        $$ = new EmptyNode(@1);
                        EndRecovery();
                    } 
                  }
                | Write String  { $$ = new Write(@1) { Child = new SimpleString($2, @2) }; }
                ;

/* ARITHEMITIC ------------------------------------------------------------------------------------------------ */ 
                
exp             : logic_exp Assign exp
                  {
                      var lhs = $1;
                      var rhs = $3;
                      if(rhs.Type == Type.Unknown)
                      {
                          $$ = rhs;
                          EndRecovery();
                      }
                      else if(lhs.Type == Type.Unknown)
                      {
                          $$ = lhs;
                          EndRecovery();
                      }
                      else if(lhs is VariableReference reference)
                      {
                           $$ = TryCreateOperator($2.token, reference, rhs);
                      }
                      else
                      {
                          Error("Can't assign to non-variable.");
                      }
                  }
                | logic_exp 
                ;
logic_exp       : logic_exp Or  relat_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | logic_exp And relat_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | relat_exp
                ;
relat_exp       : relat_exp Equals         addit_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | relat_exp NotEquals      addit_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | relat_exp Greater        addit_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | relat_exp GreaterOrEqual addit_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | relat_exp Less           addit_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | relat_exp LessOrEqual    addit_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | addit_exp
                ;
addit_exp       : addit_exp Add   mult_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | addit_exp Minus mult_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | mult_exp
                ;
mult_exp        : mult_exp Multiplies bit_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | mult_exp Divides    bit_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | bit_exp
                ;
bit_exp         : bit_exp BitOr  unary_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | bit_exp BitAnd unary_exp  { $$ = TryCreateOperator($2.token, $1, $3); }
                | unary_exp
                ;
unary_exp       : Minus                   unary_exp { $$ = TryCreateOperator($1.token, $2); }
                | Negation                unary_exp { $$ = TryCreateOperator($1.token, $2); }
                | BitNegation             unary_exp { $$ = TryCreateOperator($1.token, $2); }
                | OpenPar IntKey ClosePar unary_exp { $$ = TryCreateOperator($2.token, $4); }
                | OpenPar DoubleKey ClosePar unary_exp { $$ = TryCreateOperator($2.token, $4); }
                | factor_exp
                ;
factor_exp      : IntVal    { $$ = CreateValue(); }
                | DoubleVal { $$ = CreateValue(); }
                | True      { $$ = CreateValue(); }
                | False     { $$ = CreateValue(); }
                | OpenPar exp ClosePar { $$ = $2; }
                | Id 
                  {
                      $$ = TryCreateVariableReference($1, @1);
                  }
                | factor_exp Endl
                ;
/* ERRORS  ------------------------------------------------------------------------------------------------ */ 
error_colon     : error Colon
                  {
                    lastErrorToken = $2.token;
                  }
                ;
error_eof       : error Eof
                  {
                    lastErrorToken = $2.token;
                  }
                ;
unrecon_word    : Id Error
                | Error
                | unrecon_word Error
                | unrecon_word Id
                ;
/* OTHER  ------------------------------------------------------------------------------------------------ */ 
mult_endl       : /* empty */
                | mult_endl Endl
                ;
great_err       : Error
                | great_err Error;
end             : Colon
                | Eof
                ;
none            : %prec Error 
                    { $$ = null; }
                ;
                
                
%%              

/* HELPER FUNCTIONS ------------------------------------------------------------------------------------------------*/