
#ifndef BSTYLE_LIB_BSTYLE_CONSTANTS_H
#define BSTYLE_LIB_BSTYLE_CONSTANTS_H

logic "define @@prefixes@constants@int_0 Integer";
logic "define @@prefixes@constants@int_1 Integer";
logic "define @@prefixes@constants@size_of_int Integer";
logic "define @@prefixes@constants@size_of_long Integer";
logic "define @@prefixes@constants@size_of_bool Integer";
logic "define @@prefixes@constants@size_of_byte Integer";
logic "define @@prefixes@constants@size_of_float Integer";
logic "define @@prefixes@constants@eof Integer";

logic "copy_const @@prefixes@constants@int_0 i0";
logic "copy_const @@prefixes@constants@int_1 i1";
logic "copy_const @@prefixes@constants@size_of_int i4";
logic "copy_const @@prefixes@constants@size_of_long i8";
logic "copy_const @@prefixes@constants@size_of_bool i1";
logic "copy_const @@prefixes@constants@size_of_byte i1";
logic "copy_const @@prefixes@constants@size_of_float i16";
logic "copy_const @@prefixes@constants@eof i-1";

logic "define @@prefixes@temps@biguint BigUnsignedInteger";
logic "define @@prefixes@temps@string String";

#endif  // BSTYLE_LIB_BSTYLE_CONSTANTS_H
