
%using QUT.Gppg;

%namespace MiniCompiler

IntVal      (0|[1-9][0-9]*)
DoubleVal   (0\.[0-9]+|[1-9][0-9]*\.[0-9]+)
Id          [a-zA-Z][a-zA-Z0-9]*
PrintErr    "print"("@"|"$"|[a-z0-9])[a-z0-9]*
Endl        (\r\n|\n)

%%

"program"	  { Console.WriteLine("Found program"); return (int)Tokens.Program; }
"{"           { Console.WriteLine("Found openBrace"); return (int)Tokens.OpenBrace; }
"}"           { Console.WriteLine("Found closeBrace"); return (int)Tokens.CloseBrace; }
"return"	  { return (int)Tokens.Return; }
";"           { return (int)Tokens.Colon; }
"write"       { return (int)Tokens.Write; }
"int"         { return (int)Tokens.IntKey; }
"double"      { return (int)Tokens.DoubleKey; }
"bool"        { return (int)Tokens.BoolKey; }
"true"		  { return (int)Tokens.True; }
"false"	      { return (int)Tokens.False; }
{IntVal}	  { yylval.val=yytext; return (int)Tokens.IntVal; }
{DoubleVal}   { yylval.val=yytext; return (int)Tokens.DoubleVal; }
"="           { return (int)Tokens.Assign; }
"+"           { return (int)Tokens.Plus; }
"-"           { return (int)Tokens.Minus; }
"*"           { return (int)Tokens.Multiplies; }
"/"           { return (int)Tokens.Divides; }
"("           { return (int)Tokens.OpenPar; }
")"           { return (int)Tokens.ClosePar; }
{Id}          { yylval.val=yytext; return (int)Tokens.Id; }
{Endl}		  { }
<<EOF>>       { Console.WriteLine("Found eof"); return (int)Tokens.Eof; }
" "           { }
"\t"          { }
{PrintErr}    { return (int)Tokens.Error; }
.             { Console.WriteLine("Unexpected token: {0}/{1}", (int)yytext[0], yytext[0]); return (int)Tokens.Error; }
