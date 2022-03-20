﻿
#ifndef B2STYLE_LIB_B2STYLE_STDIO_H
#define B2STYLE_LIB_B2STYLE_STDIO_H

#include <b2style/types.h>
#include <b2style/operators.h>
#include <b2style/loaded_method.h>
#include <bstyle/int.h>

namespace b2style {

void std_out(string i) {
  logic "interrupt stdout b2style__i @@prefixes@temps@string";
}

void std_err(string i) {
  logic "interrupt stderr b2style__i @@prefixes@temps@string";
}

void std_out(bool i) {
  if (i) {
    std_out("True");
  } else {
    std_out("False");
  }
}

void std_err(bool i) {
  if (i) {
    std_err("True");
  } else {
    std_err("False");
  }
}

string legacy_biguint_to_str(biguint i) {
  if (i == 0L) {
    return "0";
  }
  string s;
  while (i > 0L) {
    int b = ::bstyle::to_int(mod(i, 10L));
    i /= 10L;
    b += 48;
    s = ::bstyle::str_concat(::bstyle::to_str(::bstyle::to_byte(b)), s);
  }
  return s;
}

string biguint_to_str(biguint i) {
  load_method("big_uint_to_str");
  return execute_loaded_method<biguint, string>(i);
}

string int_to_str(int i) {
  if (i <= 2147483647) {
    return biguint_to_str(::bstyle::to_biguint(i));
  }
  i -= 2147483647;
  i = 2147483647 - i;
  i += 2;
  return ::bstyle::str_concat("-", biguint_to_str(::bstyle::to_biguint(i)));
}

void std_out(biguint i) {
  std_out(biguint_to_str(i));
}

void std_err(biguint i) {
  std_err(biguint_to_str(i));
}

void std_out(int i) {
  std_out(int_to_str(i));
}

void std_err(int i) {
  std_err(int_to_str(i));
}

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_STDIO_H