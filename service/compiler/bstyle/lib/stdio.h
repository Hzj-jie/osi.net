
#ifndef BSTYLE_LIB_STDIO_H
#define BSTYLE_LIB_STDIO_H

#include <bstyle.h>

int EOF = bstyle__eof();

int getchar() {
  return bstyle__getchar();
}

int putchar(int i) {
  return bstyle__putchar(i);
}

#endif  // BSTYLE_LIB_STDIO_H