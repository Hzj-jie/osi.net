
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

#endif  // BSTYLE_LIB_BSTYLE_STR_H