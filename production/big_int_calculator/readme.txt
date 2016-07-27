
big integer calculator is a command line tool to calculate big integer expressions.

big integer, just as the name, it's an unlimited accurate integral data type <surely there is a limitation of the memory>. which means, not be the same as Int32 <32 bits> / Int64 <64 bits>, it does not have any limitation about the digit capacity, and the range could be negative infinity to positive infinity.
expression means this tool can not only calculate a + b or a / b, it can also understand expressions, and the priority of different operators. such as (1-(2+3))*4, or 1 + 2 * 3.

1. supporting big integer operators,
+, -, *, /, %, ==, /_, <<, <, <=, >, >=, <>, !=, ^, >>
+, add, a + b will give the result of 'a adds b'
-, minus, a - b will give the result of 'a minuses b'
*, multiply, a * b will give the result of 'a multiplies b'
/, divide, integer division, a / b will give the result of 'a divided by b, and ignore remainder'
%, modulo, a % b will give the result of 'the remainder of a divided by b'
==, equal, a == b will give 4294967295 when a is equal to b, 0 when a is not equal to b.
/_, extract, a /_ b will give the result of 'if a equals to bth power of c plus a number k < c, then the result is c', pay attention, the order of the exponent and radicand is not consistent as common.
<<, left shift, a << b will give the result of 'a left shifts b digits', since a is a big integer, the left shift will always means a * (2 ^ b)
<, less, a < b will give 4294967295 when a is less than b, 0 when a is equal to or larger than b.
<=, less or equal, a <= b will give 4294967295 when a is less than or equal to b, 0 when a is larger than b.
>, larger, a > b will give 4294967295 when a is larger than b, 0 when a is less than or equal to b.
>=, larger or equal, a >= b will give 4294967295 when a is larger than or equal to b, 0 when a is less than b.
<>, not equal, a <> b will give 4294967295 when a is not equal to b, 0 when a is equal to b.
!=, same as <>.
^, power, a ^ b will give the result of '1 multiplies a by b times'.
>>, right shift, a >> b will give the result of 'a right shifts b digits', which is also equal to a / (2 ^ b).

2. supporting brackets
{} [] () are all supported, which also do not have any differences. while the nesting of brackets are almost unlimited. <4294967295 / 5>

3. supporting output format
the implementation of big_int, which is the base of this tool, supports up to 62 scales. which means, no matter hex, dec, oct, binary are all supported. if using 62 scale number system, the digits will be 0 - 9, A - Z, a - z.

4. different syntaxes
the basic syntax has been mentioned above, to choose the output format, you can use output mask '.' with base selection '|'. i.e. 1 << 2 will output 4, but 1 << 2 . | 2 will output 100. the base selection can also be used with other numbers, so, 11 << 2 will output 44, but 11 | 2 << 2 will output 12.
and except for /_, there is also one thing different with school book. you can always write 1--2 as 1-(-2), but the second one is also supported, both will output 3.

5. usage
the interface of the tool is command line based, you can provide input with command line parameters or standard input stream. examples,
osi.production.big_int_calculator.exe "1 + 2" + 3
output 6, no matter how many parameters you have provided, the tool will join them together as one expression
osi.production.big_int_calculator.exe < input.txt
if the input.txt contains

1 + 2
2+2.|2

the output will be
expression lexing error
3
100
expression lexing error
i.e. the empty line will trigger lexing error.

6. error
as normal tools, this tool also has some limitation to the input, and may output error information instead of results.
there are two kinds of errors,
 1. expression formatting errors, which means the tool cannot understand the expression inputted. one is 'expression lexing error', means there is error during lexing stage, usually it means the input line is empty. the other is 'expression parsing error', means there is error during parsing stage, i.e. the order of number or operators are not expected, some unknown characters in the expression, etc.
 2. calculation errors, which means the tool cannot go on with the following calculation. this kind of errors include, 'divide by zero', trigger when a / 0 or a % 0. 'imaginary number', trigger when (-1) /_ 2. 'overflow', trigger when a << b or a >> b, and b is larger than max_uint64. 'operand mismatch', will not trigger in this tool, since the expression parsing will catch out the case. 'bracket mismatch', trigger when '1 + (2', the number of left bracket is not the same as right bracket.

7. platform supported
.net 3.5 on windows 2003 / 2008 / 2008 R2 / 2012 / 8.1 are all tested. so I trust all windows platforms are OK.
mono 2.8 on Ubuntu 12.10 is tested.

8. p.s.
you can directly include osi.service.math.dll in your project if you would like to use expression parsing and evaluation logic, or big_int, or calculator logic. but you also need to include several other dlls in the binary.
the reason I am not using BigInteger in .net 4.0 is,
 1. I would like to try to implement it myself, as a wheel-maker
 2. I do not want to upgrade to .net 4.0, since .net 3.5 is good enough for me now
 3. in fact, the big_int I have implemented is about 30% faster than BigInteger in .net 4.0 in + - * / % operations, and 50% faster than BigInteger in string to big_int parsing. but 100% slower than big_int to string output. I am still working on improve it.

I have put a snapshot of the source code here, but since I am actively working on all the code I have, for the newest change, please refer to geminibranch project at https://geminibranch.codeplex.com. the code structure is consistent with the one here, you will be able to find osi/root and osi/service/math there.

glad if anything can help you, please feel free to drop me a message at hzj_jie@hotmail.com if you have any questions. and
/*******************************
non-commercial use only, or please contact me if you need to use the code for commercial purpose
except for the companies i am now working or was working at.
*******************************/
