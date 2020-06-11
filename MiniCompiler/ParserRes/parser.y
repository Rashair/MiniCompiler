
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
%token Eof  
%nonassoc Endl
%nonassoc Colon
%nonassoc Error

%type <orphans> content content_declar
%type <node> block none
%type <node> instr
%type <typeNode> declar_colon declar
%type <typeNode> exp logic_exp relat_exp addit_exp mult_exp bit_exp unary_exp factor_exp
%type <type> declar_key 
%type <type> unrecon_word

%%

start           : mult_endl Program mult_endl block mult_endl Eof
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
block           : enter_scope content leave_scope
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
                | content_declar Endl
                | none
                  {
                      $$ = new List<SyntaxNode>();
                  }
                   // Errors
           //   | content_declar declar error Endl { if($2.Type != Type.Unknown) Error("Missing semicolon at col: {0}", @1.EndColumn); }
           //   | content_declar declar error Colon { if($2.Type != Type.Unknown) Error("Invalid declaration."); }
                ;
content         : content instr
                  {
                      ($1).Add($2);
                      $$ = $1;
                  }
                | content block
                  {
                      ($1).Add($2);
                      $$ = $1;
                  }
                | content Endl { $$ = $1; }
                | content Colon { $$ = $1; }
                | content_declar
                  {
                      $$ = $1;
                  }
                ;
instr           : exp Colon { $$ = $1; }
                
                | exp great_err error Colon { Error("Invalid tokens at col: {0}", @2.StartColumn); }
                | exp error Endl { Error("Missing semicolon at col: {0}", @1.EndColumn); }
                | exp error Colon { Error("Invalid statement."); }
                | error Colon { Error("Invalid statement starting at line: {0}", @1.StartLine); }
                ;
/* IDENTIFIERS  --------------------------------------------------------------------------------------------------*/
declar_colon    : declar Colon
                  {
                      Console.WriteLine("Type: " + $1.Type);
                      $$ = $1;
                  }
                | declar_key error Colon { Error("Invalid declaration."); }
                | declar_key error Endl { Error("Invalid declaration."); }
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
                | declar_key unrecon_word
                  {
                      Error("Identifier is restricted keyword or contains prohibited characters.");
                  }
                | declar Endl
                ;
declar_key      : IntKey  { $$ = Type.Int; }
                | BoolKey { $$ = Type.Bool; }
                | DoubleKey { $$ = Type.Double; }
                | declar_key Endl
                ;
/* ARITHEMITIC ------------------------------------------------------------------------------------------------ */ 
                
exp             : logic_exp Assign mult_endl exp
                  {
                      var lhs = $1;
                      var rhs = $4;
                      if(rhs.Type == Type.Unknown)
                      {
                          $$ = rhs;
                      }
                      else if(lhs.Type == Type.Unknown)
                      {
                          $$ = lhs;
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
logic_exp       : logic_exp Or  mult_endl relat_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | logic_exp And mult_endl relat_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | relat_exp
                ;
relat_exp       : relat_exp Equals         mult_endl addit_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | relat_exp NotEquals      mult_endl addit_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | relat_exp Greater        mult_endl addit_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | relat_exp GreaterOrEqual mult_endl addit_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | relat_exp Less           mult_endl addit_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | relat_exp LessOrEqual    mult_endl addit_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | addit_exp
                ;
addit_exp       : addit_exp Add   mult_endl mult_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | addit_exp Minus mult_endl mult_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | mult_exp
                ;
mult_exp        : mult_exp Multiplies mult_endl bit_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | mult_exp Divides    mult_endl bit_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | bit_exp
                ;
bit_exp         : bit_exp BitOr  mult_endl unary_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | bit_exp BitAnd mult_endl unary_exp  { $$ = TryCreateOperator($2.token, $1, $4); }
                | unary_exp
                ;
unary_exp       : Minus                      mult_endl unary_exp { $$ = TryCreateOperator($1.token, $3); }
                | Negation                   mult_endl unary_exp { $$ = TryCreateOperator($1.token, $3); }
                | BitNegation                mult_endl unary_exp { $$ = TryCreateOperator($1.token, $3); }
                | OpenPar mult_endl IntKey    mult_endl ClosePar mult_endl unary_exp { $$ = TryCreateOperator($3.token, $7); }
                | OpenPar mult_endl DoubleKey mult_endl ClosePar mult_endl unary_exp { $$ = TryCreateOperator($3.token, $7); }
                | factor_exp
                ;
factor_exp      : IntVal    { $$ = CreateValue(); }
                | DoubleVal { $$ = CreateValue(); }
                | True      { $$ = CreateValue(); }
                | False     { $$ = CreateValue(); }
                | OpenPar mult_endl exp ClosePar { $$ = $3; }
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
                | factor_exp Endl
                ;
/* ERRORS  ------------------------------------------------------------------------------------------------ */ 
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
                | Endl
                | Eof
                ;
end_no_eof      : Colon
                | Endl
                ;
end_no_colon    : Endl
                | Eof
                ;
                
any_operator    : 
                ;
none            : { $$ = null; }
                ;
                
                
%%              

/* HELPER FUNCTIONS ------------------------------------------------------------------------------------------------*/