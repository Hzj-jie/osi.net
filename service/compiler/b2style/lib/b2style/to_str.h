
#ifndef B2STYLE_LIB_B2STYLE_TO_STR_H
#define B2STYLE_LIB_B2STYLE_TO_STR_H

#include <b2style/loaded_method.h>
#include <b2style/operators.h>
#include <limits.h>

namespace b2style {

string to_str(string i) {
  return i;
}

string to_str(byte i) {
  return ::to_str(i);
}

string to_str(bool i) {
  if (i) return "True";
  return "False";
}

string legacy_biguint_to_str(biguint i) {
  if (i == 0L) {
    return "0";
  }
  string s;
  while (i > 0L) {
    int b = ::to_int(mod(i, 10L));
    i /= 10L;
    b += 48;
    s = ::str_concat(::to_str(::to_byte(b)), s);
  }
  return s;
}

// Unsupported yet.
#ifndef B3STYLE
string biguint_to_str(biguint i) {
  load_method("big_uint_to_str");
  return execute_loaded_method<biguint, string>(i);
}
#else
string biguint_to_str(biguint i) {
  return legacy_biguint_to_str(i);
}
#endif

string to_str(biguint i) {
  return biguint_to_str(i);
}

#ifndef B3STYLE
template <T>
string biguint_to_str_forward(T i, T MAX) {
  if (i <= MAX) {
    return biguint_to_str(::to_biguint(i));
  }
  i -= MAX;
  i = MAX - i;
  T _2 = 2;
  i += _2;
  return ::str_concat("-", biguint_to_str(::to_biguint(i)));
}

string int_to_str(int i) {
  return biguint_to_str_forward<int>(i, ::INT_MAX);
}

string ufloat_to_str(ufloat i) {
  load_method("big_udec_to_str");
  return execute_loaded_method<ufloat, string>(i);
}

string long_to_str(long i) {
  return biguint_to_str_forward<long>(i, ::LONG_MAX);
}
#else
string biguint_to_str_forward(biguint i, biguint max) {
  if (i <= max) {
    return biguint_to_str(i);
  }
  i -= max;
  i = max - i;
  biguint _2 = 2;
  i += _2;
  return ::str_concat("-", biguint_to_str(i));
}

string int_to_str(int i) {
  return biguint_to_str_forward(::to_biguint(i), ::to_biguint(::INT_MAX));
}

string long_to_str(long i) {
  return biguint_to_str_forward(::to_biguint(i), ::to_biguint(::LONG_MAX));
}

string ufloat_to_str(ufloat i) {
  return "ufloat (unsupported)";
}
#endif

string to_str(int i) {
  return int_to_str(i);
}

string to_str(ufloat i) {
  return ufloat_to_str(i);
}

string to_str(long i) {
  return long_to_str(i);
}

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_TO_STR_H
