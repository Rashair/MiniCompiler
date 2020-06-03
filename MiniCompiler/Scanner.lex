
%using QUT.Gppg;

%namespace MiniCompiler

IntVal      (0|[1-9][0-9]*)
DoubleVal   (0\.[0-9]+|[1-9][0-9]*\.[0-9]+)
Id          [a-zA-Z][a-zA-Z0-9]*
PrintErr    "print"("@"|"$"|[a-z0-9])[a-z0-9]*
Endl        (\r\n|\n)

%%

"program"	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); Console.WriteLine("Found program"); return (int)Tokens.Program; }
"{"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); Console.WriteLine("Found openBrace"); return (int)Tokens.OpenBrace; }
"}"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); Console.WriteLine("Found closeBrace"); return (int)Tokens.CloseBrace; }
"return"	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Return; }
";"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Colon; }
"write"       { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Write; }
"int"         { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.IntKey; }
"double"      { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.DoubleKey; }
"bool"        { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.BoolKey; }
"true"		  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.True; }
"false"	      { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.False; }
{IntVal}	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return (int)Tokens.IntVal; }
{DoubleVal}   { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return (int)Tokens.DoubleVal; }
"="           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Assign; }
"+"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Plus; }
"-"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Minus; }
"*"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Multiplies; }
"/"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Divides; }
"("           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.OpenPar; }
")"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.ClosePar; }
{Id}          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return (int)Tokens.Id; }
{Endl}		  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
<<EOF>>       { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); Console.WriteLine("Found eof"); return (int)Tokens.Eof; }
" "           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
"\t"          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
{PrintErr}    { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Error; }
.             { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); Console.WriteLine("Unexpected token: {0}/{1}", (int)yytext[0], yytext[0]); return (int)Tokens.Error; }
