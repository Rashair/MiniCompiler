
%using QUT.Gppg;
%namespace GardensPoint

IntNumber   [0-9]+
RealNumber  [0-9]+\.[0-9]+
Ident       ("@"|"$")[a-z]
PrintErr    "print"("@"|"$"|[a-z0-9])[a-z0-9]*
Endl        (\r\n|\n)

%%

"program"	  { Console.WriteLine("Found program"); return (int)Tokens.Program; }
"{"           { Console.WriteLine("Found openBrace"); return (int) Tokens.OpenBrace; }
"}"           { Console.WriteLine("Found closeBrace"); return (int) Tokens.CloseBrace; }
"return"	  { return (int) Tokens.Return; }
";"           { return (int) Tokens.Colon; }
"write"       { return (int)Tokens.Write; }
{IntNumber}   { yylval.val=yytext; return (int)Tokens.IntNumber; }
{RealNumber}  { yylval.val=yytext; return (int)Tokens.RealNumber; }
{Ident}       { yylval.val=yytext; return (int)Tokens.Ident; }
"="           { return (int)Tokens.Assign; }
"+"           { return (int)Tokens.Plus; }
"-"           { return (int)Tokens.Minus; }
"*"           { return (int)Tokens.Multiplies; }
"/"           { return (int)Tokens.Divides; }
"("           { return (int)Tokens.OpenPar; }
")"           { return (int)Tokens.ClosePar; }
{Endl}		  { Console.WriteLine("Found endl"); return (int)Tokens.Endl; }
<<EOF>>       { Console.WriteLine("Found eof"); return (int)Tokens.Eof; }
" "           { }
"\t"          { }
{PrintErr}    { return (int)Tokens.Error; }
.             { Console.WriteLine("Found: " + (int)yytext[0]); return (int)Tokens.Error; }
