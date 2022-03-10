
#ifndef BSTYLE_LIB_STDIO_H
#define BSTYLE_LIB_STDIO_H

#include <bstyle/types.h>

int getchar() {
  int r;
  logic "interrupt getchar @@prefixes@temps@string r";
  return r;
}

int putchar(int i) {
  logic "interrupt putchar i @@prefixes@temps@string";
  return i;
}

int eof() {
  int r;
  logic "copy r @@prefixes@constants@eof";
  return r;
}

#endif  // BSTYLE_LIB_STDIO_H