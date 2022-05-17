
#ifndef B2STYLE_LIB_B2STYLE_TO_STR_H
#define B2STYLE_LIB_B2STYLE_TO_STR_H

namespace b2style {

string to_str(string i) {
  return i;
}

string to_str(byte i) {
  return bstyle::to_str(i);
}

string to_str(bool i) {
  if (i) return "true";
  return "false";
}

string legacy_biguint_to_str(biguint i) {
  if (i == 0L) {
    return "0";
  }
  string s;
  while (i > 0L) {
    int b = ::bstyle::to_int(mod(i, 10L));
    i /= 10L;
    b += 48;
    s = ::bstyle::str_concat(::bstyle::to_str(::bstyle::to_byte(b)), s);
  }
  return s;
}

string biguint_to_str(biguint i) {
  load_method("big_uint_to_str");
  return execute_loaded_method<biguint, string>(i);
}

string int_to_str(int i) {
  if (i <= 2147483647) {
    return biguint_to_str(::bstyle::to_biguint(i));
  }
  i -= 2147483647;
  i = 2147483647 - i;
  i += 2;
  return ::bstyle::str_concat("-", biguint_to_str(::bstyle::to_biguint(i)));
}

string ufloat_to_str(ufloat i) {
  load_method("big_udec_to_str");
  return execute_loaded_method<ufloat, string>(i);
}

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_TO_STR_H
