
%using QUT.Gppg;

%option out:ScannerRes\Scanner.cs
%tokentype Token

%namespace MiniCompiler


IntVal             (0|[1-9][0-9]*)
DoubleVal          (0\.[0-9]+|[1-9][0-9]*\.[0-9]+)
Id                 [a-zA-Z][a-zA-Z0-9]*
String             \"[^\n]*\"
Endl               (\r\n|\n)

%%

"program"	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Program; }
"{"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.OpenBrace; }
"}"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.CloseBrace; }
"return"	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Return; }
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
";"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Colon; }
{Endl}		  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
<<EOF>>       { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Token.Eof; }
" "           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
"\t"          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
.             { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return (int)Token.Error; }
