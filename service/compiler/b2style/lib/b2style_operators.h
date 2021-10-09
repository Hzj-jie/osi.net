﻿
#ifndef B2STYLE_LIB_B2STYLE_OPERATORS_H
#define B2STYLE_LIB_B2STYLE_OPERATORS_H

#include <bstyle.h>

namespace b2style {
  typedef string ::string;
  typedef void ::void;
  typedef bool ::bool;
  typedef biguint ::biguint;
  typedef long ::long;
  typedef int ::int;

  bool and(bool i, bool j) {
    if (i) return j;
    return false;
  }

  bool or(bool i, bool j) {
    if (i) return true;
    if (j) return true;
    return false;
  }

  bool not(bool i) {
    if (i) return false;
    return true;
  }

  bool equal(biguint i, biguint j) {
    bool result;
    logic "equal result i j";
    return result;
  }

  bool not_equal(biguint i, biguint j) {
    bool result;
    logic "equal result i j";
    return not(result);
  }

  bool equal(long i, long j) {
    return equal(to_biguint(i), to_biguint(j));
  }

  bool not_equal(long i, long j) {
    return not_equal(to_biguint(i), to_biguint(j));
  }

  bool equal(int i, int j) {
    return equal(to_biguint(i), to_biguint(j));
  }

  bool not_equal(int i, int j) {
    return not_equal(to_biguint(i), to_biguint(j));
  }

  bool greater_than(biguint i, biguint j) {
    bool result;
    logic "more result i j";
    return result;
  }

  bool greater_than(long i, long j) {
    return greater_than(to_biguint(i), to_biguint(j));
  }

  bool greater_than(int i, int j) {
    return greater_than(to_biguint(i), to_biguint(j));
  }

  bool less_than(biguint i, biguint j) {
    bool result;
    logic "less result i j";
    return result;
  }

  bool less_than(long i, long j) {
    return less_than(to_biguint(i), to_biguint(j));
  }

  bool less_than(int i, int j) {
    return less_than(to_biguint(i), to_biguint(j));
  }

  bool less_or_equal(biguint i, biguint j) {
    return or(less_than(i, j), equal(i, j));
  }

  bool less_or_equal(long i, long j) {
    return or(less_than(i, j), equal(i, j));
  }

  bool less_or_equal(int i, int j) {
    return or(less_than(i, j), equal(i, j));
  }

  bool greater_or_equal(biguint i, biguint j) {
    return or(greater_than(i, j), equal(i, j));
  }

  bool greater_or_equal(long i, long j) {
    return or(greater_than(i, j), equal(i, j));
  }

  bool greater_or_equal(int i, int j) {
    return or(greater_than(i, j), equal(i, j));
  }

  biguint add(biguint i, biguint j) {
    logic "add i i j";
    return i;
  }

  long add(long i, long j) {
    logic "add i i j";
    return fit_in_long(i);
  }

  int add(int i, int j) {
    logic "add i i j";
    return fit_in_int(i);
  }

  byte add(byte i, byte j) {
    logic "add i i j";
    return fit_in_byte(i);
  }

  biguint minus(biguint i, biguint j) {
    logic "subtract i i j";
    return i;
  }

  long minus(long i, long j) {
    logic "subtract i i j";
    return fit_in_long(i);
  }

  int minus(int i, int j) {
    logic "subtract i i j";
    return fit_in_int(i);
  }

  biguint multiply(biguint i, biguint j) {
    logic "multiply i i j";
    return i;
  }

  long multiply(long i, long j) {
    logic "multiply i i j";
    return fit_in_long(i);
  }

  int multiply(int i, int j) {
    logic "multiply i i j";
    return fit_in_int(i);
  }

  biguint divide(biguint i, biguint j) {
    biguint result;
    logic "divide result @@prefixes@temps@string i j";
    return result;
  }

  long divide(long i, long j) {
    long result;
    logic "divide result @@prefixes@temps@string i j";
    return fit_in_long(result);
  }

  int divide(int i, int j) {
    int result;
    logic "divide result @@prefixes@temps@string i j";
    return fit_in_int(result);
  }

  biguint mod(biguint i, biguint j) {
    biguint result;
    logic "divide @@prefixes@temps@string result i j";
    return result;
  }

  long mod(long i, long j) {
    long result;
    logic "divide @@prefixes@temps@string result i j";
    return fit_in_long(result);
  }

  int mod(int i, int j) {
    int result;
    logic "divide @@prefixes@temps@string result i j";
    return fit_in_int(result);
  }

  biguint power(biguint i, biguint j) {
    logic "power i i j";
    return i;
  }

  long power(long i, long j) {
    logic "power i i j";
    return fit_in_long(i);
  }

  int power(int i, int j) {
    logic "power i i j";
    return fit_in_int(i);
  }

  biguint bit_and(biguint i, biguint j) {
    logic "and i i j";
    return i;
  }

  long bit_and(long i, long j) {
    logic "and i i j";
    return i;
  }

  int bit_and(int i, int j) {
    logic "and i i j";
    return i;
  }

  biguint bit_or(biguint i, biguint j) {
    logic "or i i j";
    return i;
  }

  long bit_or(long i, long j) {
    logic "or i i j";
    return i;
  }

  int bit_or(int i, int j) {
    logic "or i i j";
    return i;
  }

  biguint self_inc(biguint x) {
    logic "add x x @@prefixes@constants@int_1";
    return x;
  }

  long self_inc(long x) {
    logic "add x x @@prefixes@constants@int_1";
    return fit_in_long(x);
  }

  int self_inc(int x) {
    logic "add x x @@prefixes@constants@int_1";
    return fit_in_int(x);
  }

  biguint self_dec(biguint x) {
    logic "subtract x x @@prefixes@constants@int_1";
    return x;
  }

  long self_dec(long x) {
    logic "subtract x x @@prefixes@constants@int_1";
    return fit_in_long(x);
  }

  int self_dec(int x) {
    logic "subtract x x @@prefixes@constants@int_1";
    return fit_in_int(x);
  }

  biguint extract(biguint i, biguint j) {
    biguint r;
    logic "extract r @@prefixes@temps@biguint i j";
    return r;
  }

  biguint extract_remainder(biguint i, biguint j) {
    biguint r;
    logic "extract @@prefixes@temps@biguint r i j";
    return r;
  }

  biguint left_shift(biguint i, biguint j) {
    biguint r;
    logic "left_shift r i j";
    return r;
  }

  biguint right_shift(biguint i, biguint j) {
    biguint r;
    logic "right_shift r i j";
    return r;
  }

  biguint left_shift(biguint i, int j) {
    return left_shift(i, to_biguint(j));
  }

  biguint right_shift(biguint i, int j) {
    return right_shift(i, to_biguint(j));
  }

  biguint left_shift(biguint i, long j) {
    return left_shift(i, to_biguint(j));
  }

  biguint right_shift(biguint i, long j) {
    return right_shift(i, to_biguint(j));
  }

  int left_shift(int i, int j) {
    return to_int(
      left_shift(to_biguint(i),
        to_biguint(j))
    );
  }

  int right_shift(int i, int j) {
    return to_int(
      right_shift(to_biguint(i),
        to_biguint(j))
    );
  }

  long left_shift(long i, long j) {
    return to_long(
      left_shift(to_biguint(i),
        to_biguint(j))
    );
  }

  long right_shift(long i, long j) {
    return to_long(
      right_shift(to_biguint(i),
        to_biguint(j))
    );
  }
}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_OPERATORS_H