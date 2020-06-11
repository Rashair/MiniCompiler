
%using QUT.Gppg;

%option out:ScannerRes\Scanner.cs
%tokentype Token

%namespace MiniCompiler


IntVal             (0|[1-9][0-9]*)
DoubleVal          (0\.[0-9]+|[1-9][0-9]*\.[0-9]+)
Id                 [a-zA-Z][a-zA-Z0-9]*
String             \"[^\n]*\"
Comment            \/\/.*/\n
Endl               (\r\n|\n)

%%

"program"	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Program; return (int) Token.Program; }
"{"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.OpenBrace; return (int) Token.OpenBrace;  }
"}"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.CloseBrace; return (int) Token.CloseBrace;  }
"return"	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Return; return (int) Token.Return; }
"if"	      { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.If; return (int) Token.If; }
"else"	      { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Else; return (int) Token.Else; }
"while"	      { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.While; return (int) Token.While; }
"read"        { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Read; return (int) Token.Read; }
"write"       { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Write; return (int) Token.Write; }
"true"		  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val="1";  yylval.token = Token.True; return (int) Token.True; }
"false"	      { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val="0";  yylval.token = Token.False; return (int) Token.False; }
"int"         { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.IntKey; return (int) Token.IntKey; }
"double"      { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.DoubleKey; return (int) Token.DoubleKey; }
"bool"        { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.BoolKey; return (int) Token.BoolKey; }
{Comment}     { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
{IntVal}	  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; yylval.token = Token.IntVal; return (int) Token.IntVal; }
{DoubleVal}   { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; yylval.token = Token.DoubleVal; return (int) Token.DoubleVal; }
"="           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Assign; return (int) Token.Assign; }
"||"          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Or; return (int) Token.Or; }
"&&"          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.And; return (int) Token.And; }
"|"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.BitOr; return (int) Token.BitOr; }
"&"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.BitAnd; return (int) Token.BitAnd; }
"!"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Negation; return (int) Token.Negation; }
"~"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.BitNegation; return (int) Token.BitNegation; }
"=="          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Equals; return (int) Token.Equals; }
"!="          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.NotEquals; return (int) Token.NotEquals; }
">"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Greater; return (int) Token.Greater; }
">="          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.GreaterOrEqual; return (int) Token.GreaterOrEqual; }
"<"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Less; return (int) Token.Less; } 
"<="          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.LessOrEqual; return (int) Token.LessOrEqual; }
"+"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Add; return (int) Token.Add; }
"-"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Minus; return (int) Token.Minus; }
"*"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Multiplies; return (int) Token.Multiplies; }
"/"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Divides; return (int) Token.Divides; }
"("           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.OpenPar; return (int) Token.OpenPar; }
")"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.ClosePar; return (int) Token.ClosePar; }
{Id}          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; yylval.token = Token.Id; return (int) Token.Id; }
";"           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Colon; return (int) Token.Colon; }
{Endl}		  { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
<<EOF>>       { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.token = Token.Eof; return (int) Token.Eof; }
" "           { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
"\t"          { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); }
.             { yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; yylval.token = Token.Error; return (int) Token.Error; }
