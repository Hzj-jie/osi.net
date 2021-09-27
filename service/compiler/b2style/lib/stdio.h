
#ifndef B2STYLE_LIB_STDIO_H
#define B2STYLE_LIB_STDIO_H

int EOF = b2style::eof();

int getchar() {
  return b2style::getchar();
}

int putchar(int i) {
  return b2style::putchar(i);
}

#endif