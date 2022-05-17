
#ifndef B2STYLE_LIB_B2STYLE_STDIO_H
#define B2STYLE_LIB_B2STYLE_STDIO_H

#include <b2style/to_str.h>
#include <b2style/types.h>

namespace b2style {

void std_out(string i) {
  logic "interrupt stdout b2style__i @@prefixes@temps@string";
}

void std_err(string i) {
  logic "interrupt stderr b2style__i @@prefixes@temps@string";
}

template <T>
void std_out(T i) {
  std_out(to_str(i));
}

template <T>
void std_err(T i) {
  std_err(to_str(i));
}

// TODO: Implement template inference.
void std_out(bool i) {
  std_out<bool>(i);
}

void std_err(bool i) {
  std_err<bool>(i);
}

void std_out(int i) {
  std_out<int>(i);
}

void std_err(int i) {
  std_err<int>(i);
}

void std_out(biguint i) {
  std_out<biguint>(i);
}

void std_err(biguint i) {
  std_err<biguint>(i);
}

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_STDIO_H
