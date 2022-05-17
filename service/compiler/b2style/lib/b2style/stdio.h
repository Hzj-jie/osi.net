
#ifndef B2STYLE_LIB_B2STYLE_STDIO_H
#define B2STYLE_LIB_B2STYLE_STDIO_H

#include <b2style/loaded_method.h>
#include <b2style/operators.h>
#include <b2style/to_str.h>
#include <b2style/types.h>
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
