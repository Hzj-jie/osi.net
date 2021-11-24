
#ifndef B2STYLE_LIB_B2STYLE_UFLOAT_H
#define B2STYLE_LIB_B2STYLE_UFLOAT_H

#include <bstyle.h>
#include <b2style_types.h>

namespace b2style {
  bool equal(ufloat i, ufloat j) {
    bool result;
    logic "float_equal result i j";
    return result;
  }

  bool not_equal(ufloat i, ufloat j) {
    bool result;
    logic "float_equal result i j";
    return not(result);
  }

  bool greater_than(ufloat i, ufloat j) {
    bool result;
    logic "float_more result i j";
    return result;
  }

  bool greater_or_equal(ufloat i, ufloat j) {
    return or(greater_than(i, j), equal(i, j));
  }

  bool less_than(ufloat i, ufloat j) {
    bool result;
    logic "float_less result i j";
    return result;
  }

  bool less_or_equal(ufloat i, ufloat j) {
    return or(less_than(i, j), equal(i, j));
  }

  ufloat add(ufloat i, ufloat j) {
    logic "float_add i i j";
    return i;
  }

  ufloat minus(ufloat i, ufloat j) {
    logic "float_subtract i i j";
    return i;
  }

  ufloat multiply(ufloat i, ufloat j) {
    logic "float_multiply i i j";
    return i;
  }

  ufloat divide(ufloat i, ufloat j) {
    ufloat result;
    logic "float_divide result i j";
    return result;
  }

  ufloat power(ufloat i, ufloat j) {
    logic "float_power i i j";
    return i;
  }

  string ufloat_to_str(ufloat i) {
    ::bstyle::load_method("big_udec_to_str");
    string result;
    logic "interrupt execute_loaded_method i result";
    return result;
  }

  void std_out(ufloat i) {
    std_out(ufloat_to_str(i));
  }

  ufloat ufloat__from(biguint i) {
    ::bstyle::load_method("big_uint_to_big_udec");
    ufloat result;
    logic "interrupt execute_loaded_method i result";
    return result;
  }

  ufloat ufloat__from(int i) {
    return ufloat__from(::bstyle::to_biguint(i));
  }

  ufloat ufloat__from(long i) {
    return ufloat__from(::bstyle::to_biguint(i));
  }

  ufloat ufloat__fraction(biguint n, biguint d) {
    ufloat result = ufloat__from(n);
    ufloat ud = ufloat__from(d);
    return divide(result, ud);
  }

  ufloat ufloat__fraction(biguint n, int d) {
    return ufloat__fraction(n, ::bstyle::to_biguint(d));
  }

  ufloat ufloat__fraction(biguint n, long d) {
    return ufloat__fraction(n, ::bstyle::to_biguint(d));
  }

  ufloat ufloat__fraction(int n, biguint d) {
    return ufloat__fraction(::bstyle::to_biguint(n), d);
  }

  ufloat ufloat__fraction(long n, biguint d) {
    return ufloat__fraction(::bstyle::to_biguint(n), d);
  }

  ufloat ufloat__fraction(int n, int d) {
    return ufloat__fraction(::bstyle::to_biguint(n), ::bstyle::to_biguint(d));
  }

  ufloat ufloat__fraction(long n, long d) {
    return ufloat__fraction(::bstyle::to_biguint(n), ::bstyle::to_biguint(d));
  }

}  // namespace b2style
#endif  // B2STYLE_LIB_B2STYLE_UFLOAT_H