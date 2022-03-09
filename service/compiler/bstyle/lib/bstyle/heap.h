
#ifndef BSTYLE_LIB_BSTYLE_STDLIB_H
#define BSTYLE_LIB_BSTYLE_STDLIB_H

#include <bstyle/types.h>

typedef long heap_ptr;

heap_ptr bstyle__npos() {
  return -1;
}

bool bstyle__heap_ptr_equal(heap_ptr a, heap_ptr b) {
  bool result;
  logic "equal result a b";
  return result;
}

void bstyle__dealloc(heap_ptr p) {
  if (bstyle__heap_ptr_equal(p, bstyle__npos())) return;
  logic "dealloc_heap p";
}

void bstyle__alloc(heap_ptr& p, int size) {
  void x[size];
  p = x;
  logic "undefine x";
}

#endif  // BSTYLE_LIB_BSTYLE_STDLIB_H
