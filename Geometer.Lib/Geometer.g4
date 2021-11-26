grammar Geometer;

/** Parser **/

start : line* EOF ;
line : objref constraint? NEWLINE? 
     | QUERY queryline NEWLINE?
     | IMPORT FILE
     | CLEAR
     | RESET
     | UNDO NUMBER?
     | REDO NUMBER?
     | SAVE FILE?
     ;

queryline : objref constraint?
          | expression (EQUAL | LESS | GREATER | NOT EQUAL | LESS EQUAL | GREATER EQUAL) expression
          ;

onshape : ON (lineref | circref | polyref);
perpbis : PERPENDICULAR BISECTOR OF lineref;
bisect  : BISECTOR OF (lineref | angleref);
tangent : TANGENT OF circref;

constraint : IS (onshape | perpbis | bisect | tangent)
           | (EQUAL | LESS | GREATER | NOT EQUAL | LESS EQUAL | GREATER EQUAL) expression
           | (SQUIGGLE | SQUIGGLE EQUAL) objref
           ;

idchain : IDENTIFIER (COMBINE IDENTIFIER)* ;

pointref : POINT idchain secname? ;
lineref : LINE idchain idchain? secname? ;
circref : CIRCLE idchain idchain? secname? ;
angleref : ANGLE idchain idchain? idchain? secname? ;
polyref : POLYGON idchain idchain idchain+ secname? ;

lengthref : LENGTH OF lineref ;
sizeref : SIZE OF angleref ;
arearef : AREA OF (circref | polyref) ;
periref : PERIMETER OF polyref ;
circumref : CIRCUMFERENCE OF circref ;

secname : NAME IDENTIFIER;

expression : expression (ADD|MINUS) multexpression | multexpression ;
multexpression : multexpression (MULT|DIV) expexpression | expexpression ;
expexpression : term EXP expexpression | term ;
term : NUMBER | numref | PI | TAU | '(' expression ')' ;

numref : lengthref | sizeref | arearef | periref | circumref ;
objref : idchain | pointref | lineref | circref | angleref | polyref | numref ;
/* objtype : POINT | LINE | CIRCLE | ANGLE | POLYGON | NUMTYPE ; */

/** Lexer  **/
POINT         : 'point' ;
LINE          : 'line' ;
CIRCLE        : 'circle' ;
ANGLE         : 'angle' ;
POLYGON       : 'polygon' ;
PARALLEL      : 'parallel' ;
PERPENDICULAR : 'perpendicular' ;
BISECTOR      : 'bisector' ;
CONGRUENT     : 'congruent' ;
SIMILAR       : 'similar' ;

IS            : 'is' ;
ON            : 'on' ;
OF            : 'of' ;
NAME          : 'name' ;
INTERSECTION  : 'intersection' ;
DIAMETER      : 'diameter' ;
RADIUS        : 'radius' ;
CHORD         : 'chord' ;
TANGENT       : 'tangent' ;

LENGTH        : 'length' ;
SIZE          : 'size' ;
AREA          : 'area' ;
PERIMETER     : 'perimeter' ;
CIRCUMFERENCE : 'circumference' ;

QUERY         : '?' ;

LESS          : '<' ;
GREATER       : '>' ;
EQUAL         : '=' ;
NOT           : '!' ;
SQUIGGLE      : '~' ;

ADD           : '+' ;
MINUS         : '-' ;
MULT          : '*' ;
DIV           : '/' ;
EXP           : '^' ; 

COMMA         : ',' ;
SEMICOLON     : ';' ;
COLON         : ':' ;

OPENPAREN     : '(' ;
CLOSEPAREN    : ')' ;
OPENSQUARE    : '[' ;
CLOSESQUARE   : ']' ;
OPENCURLY     : '{' ;
CLOSECURLY    : '}' ;
 
PI            : 'pi' ;
TAU           : 'tau' ;

IMPORT        : 'import' ;
CLEAR         : 'clear' ;
RESET         : 'reset' ;
UNDO          : 'undo' ;
REDO          : 'redo' ;
SAVE          : 'save' ;

NUMTYPE       : 'number';
NUMBER        : '-'?('0'|[1-9][0-9]*) ;
IDENTIFIER    : [A-Z]([a-zA-Z0-9])* ;
CUSTOM_ID     : [a-z]([a-zA-Z0-9])* ;
FILE          : '\'' ('/'|'~/'|'./')? (~[/\n]+ '/')* ~[\n]* '\'' ;
COMBINE       : '_' ;
NEWLINE       : ('\r'? '\n' | '\r')+ ;

WHITESPACE    : (' '|'\t') -> skip ;
