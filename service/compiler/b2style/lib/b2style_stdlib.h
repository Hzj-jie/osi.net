
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

void dealloc(int p) {
  dealloc<int>(p);
}

template <T>
void alloc(T& p, int size) {
  T x[size];
  reinterpret_cast(p, int);
  p = x;
  logic "undefine b2style__x";
}

void alloc(int& p, int size) {
  alloc<int>(p, size);
}

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_STDLIB_H
