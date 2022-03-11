
#ifndef B2STYLE_LIB_ASSERT_H
#define B2STYLE_LIB_ASSERT_H

#include <b2style/stdio.h>
#include <b2style/types.h>

void assert(bool v, string msg) {
  if (v) return;
  b2style::std_out(msg);
  logic "stop";
}

void assert(bool v) {
  assert(v, "Assertion failure");
}

#endif  // B2STYLE_LIB_ASSERT_H