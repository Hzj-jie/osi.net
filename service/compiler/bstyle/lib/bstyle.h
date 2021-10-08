
#ifndef BSTYLE_LIB_BSTYLE_H
#define BSTYLE_LIB_BSTYLE_H

// Use double-underscore to allow b2style to access these functions with bstyle:: namespace format.
int bstyle__getchar() {
  int r;
  logic "interrupt getchar @@prefixes@temps@string r";
  return r;
}

int bstyle__putchar(int i) {
  logic "interrupt putchar i @@prefixes@temps@string";
  return i;
}

int bstyle__eof() {
  int r;
  logic "copy r @@prefixes@constants@eof";
  return r;
}

#endif  // BSTYLE_LIB_BSTYLE_H
