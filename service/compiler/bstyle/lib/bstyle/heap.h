
#ifndef BSTYLE_LIB_BSTYLE_STDLIB_H
#define BSTYLE_LIB_BSTYLE_STDLIB_H

#include <bstyle/types.h>

typedef long bstyle__heap_ptr;

bstyle__heap_ptr bstyle__npos() {
  return -1;
}

bool bstyle__bstyle__heap_ptr_equal(bstyle__heap_ptr a, bstyle__heap_ptr b) {
  bool result;
  logic "equal result a b";
  return result;
}

void bstyle__dealloc(bstyle__heap_ptr& p) {
  if (bstyle__bstyle__heap_ptr_equal(p, bstyle__npos())) return;
  logic "dealloc_heap p";
  p = bstyle__npos();
}

void bstyle__alloc(bstyle__heap_ptr& p, int size) {
  void x[size];
  p = x;
  logic "undefine x";
}

#endif  // BSTYLE_LIB_BSTYLE_STDLIB_H
