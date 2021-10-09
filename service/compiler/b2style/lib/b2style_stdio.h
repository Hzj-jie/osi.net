
#ifndef B2STYLE_LIB_B2STYLE_STDIO_H
#define B2STYLE_LIB_B2STYLE_STDIO_H

#include <bstyle_types.h>
#include <b2style_operators.h>

namespace b2style {
  typedef string ::string;
  typedef void ::void;
  typedef bool ::bool;

  string true_str = "True";
  string false_str = "False";

  void std_out(string i) {
    logic "interrupt stdout i @@prefixes@temps@string";
  }

  void std_err(string i) {
    logic "interrupt stderr i @@prefixes@temps@string";
  }

  void bool_std_out(bool i) {
    if (i) {
      std_out(true_str);
    }
    else {
      std_out(false_str);
    }
  }

  void bool_std_err(bool i) {
    if (i) {
      std_err(true_str);
    }
    else {
      std_err(false_str);
    }
  }

  void std_out(bool i) {
    bool_std_out(i);
  }

  void std_err(bool i) {
    bool_std_err(i);
  }

  string legacy_biguint_to_str(biguint i) {
    if (equal(i, 0L)) {
      return "0";
    }
    string s;
    while (greater_than(i, 0L)) {
      int b = to_int(mod(i, 10L));
      i = divide(i, 10L);
      b = add(b, 48);
      s = str_concat(to_str(to_byte(b)), s);
    }
    return s;
  }

  string loaded_methods(string m) {
    return str_concat(
      "osi.service.interpreter.primitive.loaded_methods, osi.service.interpreter:",
      m);
  }

  string biguint_to_str(biguint i) {
    string method_name = loaded_methods("big_uint_to_str");
    logic "interrupt load_method method_name @@prefixes@temps@string";
    string result;
    logic "interrupt execute_loaded_method i result";
    return result;
  }

  string int_to_str(int i) {
    if (greater_than(i, 2147483647)) {
      i = minus(i, 2147483647);
      i = minus(2147483647, i);
      i = add(i, 2);
      string s = "-";
      return str_concat(s, biguint_to_str(to_biguint(i)));
    }
    return biguint_to_str(to_biguint(i));
  }

  void biguint_std_out(biguint i) {
    std_out(biguint_to_str(i));
  }

  void biguint_std_err(biguint i) {
    std_err(biguint_to_str(i));
  }

  void int_std_out(int i) {
    std_out(int_to_str(i));
  }

  void int_std_err(int i) {
    std_err(int_to_str(i));
  }

  void std_out(int i) {
    int_std_out(i);
  }

  void std_err(int i) {
    int_std_err(i);
  }
}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_STDIO_H
