
#ifndef BSTYLE_LIB_BSTYLE_H
#define BSTYLE_LIB_BSTYLE_H

#include <bstyle_types.h>
#include <bstyle_constants.h>

// Use double-underscore to allow bstyle to access these functions with bstyle:: namespace format.
int bstyle__getchar() {
  int r;
  logic "interrupt getchar @@prefixes@temps@string r";
  return r;
}

int bstyle__putchar(int i) {
  logic "interrupt putchar i @@prefixes@temps@string";
  return i;
}

int bstyle__eof() {
  int r;
  logic "copy r @@prefixes@constants@eof";
  return r;
}

int bstyle__current_ms() {
  int result;
  logic "interrupt current_ms @@prefixes@temps@string result";
  return result;
}

string bstyle__str_concat(string i, string j) {
  logic "append i j";
  return i;
}

string bstyle__str_concat(string i, byte j) {
  logic "append i j";
  return i;
}

string bstyle__to_str(byte i) {
  string s;
  return bstyle__str_concat(s, i);
}

biguint bstyle__to_biguint(int i) {
  return i;
}

biguint bstyle__to_biguint(long i) {
  return i;
}

long bstyle__to_long(int i) {
  return i;
}

long bstyle__fit_in_long(long x) {
  logic "cut_len x x @@prefixes@constants@int_0 @@prefixes@constants@size_of_long";
  return x;
}

int bstyle__fit_in_int(int x) {
  logic "cut_len x x @@prefixes@constants@int_0 @@prefixes@constants@size_of_int";
  return x;
}

byte bstyle__fit_in_byte(byte x) {
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

void bstyle__load_method(string m) {
  m = bstyle__str_concat(
		  "osi.service.interpreter.primitive.loaded_methods, osi.service.interpreter:",
		  m);
  logic "interrupt load_method m @@prefixes@temps@string";
}

#endif  // BSTYLE_LIB_BSTYLE_H
