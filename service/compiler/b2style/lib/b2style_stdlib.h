
#ifndef B2STYLE_LIB_B2STYLE_STDLIB_H
#define B2STYLE_LIB_B2STYLE_STDLIB_H

#include <b2style_types.h>
#include <b2style_const.h>

namespace b2style {

template <T>
void dealloc(T p) {
  if (p == npos) return;
  logic "dealloc_heap b2style__p";
}

template <T, RT>
void alloc(T& p, int size) {
  dealloc<T>(p);
  RT x[size];
  reinterpret_cast(p, int);
  p = x;
  logic "undefine b2style__x";
}

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_STDLIB_H
