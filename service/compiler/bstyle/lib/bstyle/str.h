
#ifndef BSTYLE_LIB_BSTYLE_STR_H
#define BSTYLE_LIB_BSTYLE_STR_H

#include <bstyle/int.h>
#include <bstyle/types.h>

string bstyle__str_concat(string i, string j) {
  logic "append i j";
  return i;
}

string bstyle__str_concat(string i, string j, string k) {
  return bstyle__str_concat(bstyle__str_concat(i, j), k);
}

string bstyle__str_concat(string i, byte j) {
  logic "append i j";
  return i;
}

string bstyle__to_str(byte i) {
  string s;
  return bstyle__str_concat(s, i);
}

int bstyle__str_len(string s) {
  int r;
  logic "sizeof r s";
  return r;
}

bool bstyle__str_empty(string s) {
  // Note, empty means the s == null rather than s.length == 0.
  return bstyle__equal(bstyle__str_len(s), 0);
}

// TODO: Use a better way to compare strings, treating them as big_uints is not accurate or efficient.
bool bstyle__str_equal(string i, string j) {
  bool result;
  logic "equal result i j";
  return result;
}

#endif  // BSTYLE_LIB_BSTYLE_STR_H