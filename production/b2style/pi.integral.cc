void main() {
  ufloat x = 0.0;
  ufloat dx = 0.01;
  ufloat s = 0.0;
  while (x < 1.0) {
    ufloat c = 1.0 - (x ^ 2.0);
    c = c ^ 0.5;
    c = c * dx;
    s = s + c;
    x = x + dx;
  }

  b2style::std_out(s * 4.0);
}
