void main() {
  int x = 0;
  for (int i = 0; i < 100; i = i++) {
    x = x + i + 1;
  }
  b2style::int_std_out(x);
  b2style::std_out("\n");
}
