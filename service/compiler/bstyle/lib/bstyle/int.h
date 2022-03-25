
#ifndef BSTYLE_LIB_BSTYLE_INT_H
#define BSTYLE_LIB_BSTYLE_INT_H

#include <bstyle/const.h>
#include <bstyle/types.h>

biguint bstyle__to_biguint(int i) {
  return i;
}

biguint bstyle__to_biguint(long i) {
  return i;
}

long bstyle__to_long(int i) {
  return i;
}

long bstyle__fit_in_long(long& x) {
  logic "cut_len x x @@prefixes@constants@int_0 @@prefixes@constants@size_of_long";
  return x;
}

int bstyle__fit_in_int(int& x) {
  logic "cut_len x x @@prefixes@constants@int_0 @@prefixes@constants@size_of_int";
  return x;
}

byte bstyle__fit_in_byte(byte& x) {
  logic "cut_len x x @@prefixes@constants@int_0 @@prefixes@constants@size_of_byte";
  return x;
}

long bstyle__to_long(biguint x) {
  long y;
  logic "cut_len y x @@prefixes@constants@int_0 @@prefixes@constants@size_of_long";
  return y;
}

int bstyle__to_int(biguint x) {
  int y;
  logic "cut_len y x @@prefixes@constants@int_0 @@prefixes@constants@size_of_int";
  return y;
}

byte bstyle__to_byte(int x) {
  byte y;
  logic "cut_len y x @@prefixes@constants@int_0 @@prefixes@constants@size_of_byte";
  return y;
}

#endif  // BSTYLE_LIB_BSTYLE_INT_H