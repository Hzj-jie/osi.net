
#ifndef BSTYLE_LIB_BSTYLE_STR_H
#define BSTYLE_LIB_BSTYLE_STR_H

#include <bstyle/types.h>

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

int bstyle__str_len(string s) {
  int r;
  logic "sizeof r s";
  return r;
}

bool bstyle__str_empty(string s) {
  // Note, empty means the s == null rather than s.length == 0.
  int r = bstyle__str_len(s);
  bool result;
  int zero = 0;
  logic "equal result r zero";
  return result;
}

#endif  // BSTYLE_LIB_BSTYLE_STR_H