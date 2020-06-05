
%using QUT.Gppg;

%option out:ScannerRes\Scanner.cs
%tokentype Token

%namespace MiniCompiler


IntVal      (0|[1-9][0-9]*)
DoubleVal   (0\.[0-9]+|[1-9][0-9]*\.[0-9]+)
Id          [a-zA-Z][a-zA-Z0-9]*
PrintErr    "print"("@"|"$"|[a-z0-9])[a-z0-9]*
Endl        (\r\n|\n)

%%

"program"	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Program; }
"{"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.OpenBrace; }
"}"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.CloseBrace; }
"return"	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Return; }
";"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Colon; }
"write"       { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Write; }
"int"         { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.IntKey; }
"double"      { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.DoubleKey; }
"bool"        { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.BoolKey; }
"true"		  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.True; }
"false"	      { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.False; }
{IntVal}	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return (int)Token.IntVal; }
{DoubleVal}   { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return (int)Token.DoubleVal; }
"="           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Assign; }
"+"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Plus; }
"-"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Minus; }
"*"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Multiplies; }
"/"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Divides; }
"("           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.OpenPar; }
")"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.ClosePar; }
{Id}          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return (int)Token.Id; }
{Endl}		  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
<<EOF>>       { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Eof; }
" "           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
"\t"          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
{PrintErr}    { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Error; }
.             { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); Console.WriteLine("Unexpected token: {0}/{1}", (int)yytext[0], yytext[0]); return (int)Token.Error; }
