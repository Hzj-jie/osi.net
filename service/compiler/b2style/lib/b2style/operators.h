﻿
#ifndef B2STYLE_LIB_B2STYLE_OPERATORS_H
#define B2STYLE_LIB_B2STYLE_OPERATORS_H

#include <b2style/types.h>
#include <bstyle/int.h>
#include <bstyle/str.h>

namespace b2style {

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

// TODO: Consider to avoid adding "_pre" for operator !.
bool not_pre(bool i) {
  return not(i);
}

bool equal(string i, string j) {
  return ::str_equal(i, j);
}

bool not_equal(string i, string j) {
  return not(equal(i, j));
}

bool equal(biguint i, biguint j) {
  return ::equal(i, j);
}

bool not_equal(biguint i, biguint j) {
  return not(equal(i, j));
}

bool equal(long i, long j) {
  return ::equal(i, j);
}

bool not_equal(long i, long j) {
  return not(equal(i, j));
}

bool equal(int i, int j) {
  return ::equal(i, j);
}

bool not_equal(int i, int j) {
  return not(equal(i, j));
}

bool equal(bool i, bool j) {
  if (i) return j;
  return not(j);
}

bool not_equal(bool i, bool j) {
  return not(equal(i, j));
}

#ifdef B2STYLE
bool greater_than(biguint i, biguint j) {
  bool result;
  logic "more b2style__result b2style__i b2style__j";
  return result;
}
#else
bool greater_than(biguint i, biguint j) {
  bool result;
  logic "more result i j";
  return result;
}
#endif

bool greater_than(long i, long j) {
  return greater_than(::to_biguint(i), ::to_biguint(j));
}

bool greater_than(int i, int j) {
  return greater_than(::to_biguint(i), ::to_biguint(j));
}

#ifdef B2STYLE
bool less_than(biguint i, biguint j) {
  bool result;
  logic "less b2style__result b2style__i b2style__j";
  return result;
}
#else
bool less_than(biguint i, biguint j) {
  bool result;
  logic "less result i j";
  return result;
}
#endif

bool less_than(long i, long j) {
  return less_than(::to_biguint(i), ::to_biguint(j));
}

bool less_than(int i, int j) {
  return less_than(::to_biguint(i), ::to_biguint(j));
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

#ifdef B2STYLE
biguint add(biguint i, biguint j) {
  logic "add b2style__i b2style__i b2style__j";
  return i;
}

long add(long i, long j) {
  logic "add b2style__i b2style__i b2style__j";
  return ::fit_in_long(i);
}

int add(int i, int j) {
  logic "add b2style__i b2style__i b2style__j";
  return ::fit_in_int(i);
}

byte add(byte i, byte j) {
  logic "add b2style__i b2style__i b2style__j";
  return ::fit_in_byte(i);
}
#else
biguint add(biguint i, biguint j) {
  logic "add i i j";
  return i;
}

long add(long i, long j) {
  logic "add i i j";
  return ::fit_in_long(i);
}

int add(int i, int j) {
  logic "add i i j";
  return ::fit_in_int(i);
}

byte add(byte i, byte j) {
  logic "add i i j";
  return ::fit_in_byte(i);
}
#endif

string add(string i, string j) {
  return ::str_concat(i, j);
}

#ifdef B2STYLE
biguint minus(biguint i, biguint j) {
  logic "subtract b2style__i b2style__i b2style__j";
  return i;
}

long minus(long i, long j) {
  logic "subtract b2style__i b2style__i b2style__j";
  return ::fit_in_long(i);
}

int minus(int i, int j) {
  logic "subtract b2style__i b2style__i b2style__j";
  return ::fit_in_int(i);
}

biguint multiply(biguint i, biguint j) {
  logic "multiply b2style__i b2style__i b2style__j";
  return i;
}

long multiply(long i, long j) {
  logic "multiply b2style__i b2style__i b2style__j";
  return ::fit_in_long(i);
}

int multiply(int i, int j) {
  logic "multiply b2style__i b2style__i b2style__j";
  return ::fit_in_int(i);
}

biguint divide(biguint i, biguint j) {
  biguint result;
  logic "divide b2style__result @@prefixes@temps@string b2style__i b2style__j";
  return result;
}

long divide(long i, long j) {
  long result;
  logic "divide b2style__result @@prefixes@temps@string b2style__i b2style__j";
  return ::fit_in_long(result);
}

int divide(int i, int j) {
  int result;
  logic "divide b2style__result @@prefixes@temps@string b2style__i b2style__j";
  return ::fit_in_int(result);
}

biguint mod(biguint i, biguint j) {
  biguint result;
  logic "divide @@prefixes@temps@string b2style__result b2style__i b2style__j";
  return result;
}

long mod(long i, long j) {
  long result;
  logic "divide @@prefixes@temps@string b2style__result b2style__i b2style__j";
  return ::fit_in_long(result);
}

int mod(int i, int j) {
  int result;
  logic "divide @@prefixes@temps@string b2style__result b2style__i b2style__j";
  return ::fit_in_int(result);
}

biguint power(biguint i, biguint j) {
  logic "power b2style__i b2style__i b2style__j";
  return i;
}

long power(long i, long j) {
  logic "power b2style__i b2style__i b2style__j";
  return ::fit_in_long(i);
}

int power(int i, int j) {
  logic "power b2style__i b2style__i b2style__j";
  return ::fit_in_int(i);
}

biguint bit_and(biguint i, biguint j) {
  logic "and b2style__i b2style__i b2style__j";
  return i;
}

long bit_and(long i, long j) {
  logic "and b2style__i b2style__i b2style__j";
  return i;
}

int bit_and(int i, int j) {
  logic "and b2style__i b2style__i b2style__j";
  return i;
}

biguint bit_or(biguint i, biguint j) {
  logic "or b2style__i b2style__i b2style__j";
  return i;
}

long bit_or(long i, long j) {
  logic "or b2style__i b2style__i b2style__j";
  return i;
}

int bit_or(int i, int j) {
  logic "or b2style__i b2style__i b2style__j";
  return i;
}

biguint self_inc_post(biguint& x) {
  biguint r = x;
  logic "add b2style__x b2style__x @@prefixes@constants@int_1";
  return r;
}

long self_inc_post(long& x) {
  long r = x;
  logic "add b2style__x b2style__x @@prefixes@constants@int_1";
  return ::fit_in_long(r);
}

int self_inc_post(int& x) {
  int r = x;
  logic "add b2style__x b2style__x @@prefixes@constants@int_1";
  return ::fit_in_int(r);
}

biguint self_dec_post(biguint& x) {
  biguint r = x;
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return r;
}

long self_dec_post(long& x) {
  long r = x;
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return ::fit_in_long(r);
}

int self_dec_post(int& x) {
  int r = x;
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return ::fit_in_int(r);
}

biguint self_inc_pre(biguint& x) {
  logic "add b2style__x b2style__x @@prefixes@constants@int_1";
  return x;
}

long self_inc_pre(long& x) {
  logic "add b2style__x b2style__x @@prefixes@constants@int_1";
  return ::fit_in_long(x);
}

int self_inc_pre(int& x) {
  logic "add b2style__x b2style__x @@prefixes@constants@int_1";
  return ::fit_in_int(x);
}

biguint self_dec_pre(biguint& x) {
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return x;
}

long self_dec_pre(long& x) {
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return ::fit_in_long(x);
}

int self_dec_pre(int& x) {
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return ::fit_in_int(x);
}

biguint extract(biguint i, biguint j) {
  biguint r;
  logic "extract b2style__r @@prefixes@temps@biguint b2style__i b2style__j";
  return r;
}

biguint extract_remainder(biguint i, biguint j) {
  biguint r;
  logic "extract @@prefixes@temps@biguint b2style__r b2style__i b2style__j";
  return r;
}

biguint left_shift(biguint i, biguint j) {
  biguint r;
  logic "left_shift b2style__r b2style__i b2style__j";
  return r;
}

biguint right_shift(biguint i, biguint j) {
  biguint r;
  logic "right_shift b2style__r b2style__i b2style__j";
  return r;
}
#else
biguint minus(biguint i, biguint j) {
  logic "subtract i i j";
  return i;
}

long minus(long i, long j) {
  logic "subtract i i j";
  return ::fit_in_long(i);
}

int minus(int i, int j) {
  logic "subtract i i j";
  return ::fit_in_int(i);
}

biguint multiply(biguint i, biguint j) {
  logic "multiply i i j";
  return i;
}

long multiply(long i, long j) {
  logic "multiply i i j";
  return ::fit_in_long(i);
}

int multiply(int i, int j) {
  logic "multiply i i j";
  return ::fit_in_int(i);
}

biguint divide(biguint i, biguint j) {
  biguint result;
  logic "divide result @@prefixes@temps@string i j";
  return result;
}

long divide(long i, long j) {
  long result;
  logic "divide result @@prefixes@temps@string i j";
  return ::fit_in_long(result);
}

int divide(int i, int j) {
  int result;
  logic "divide result @@prefixes@temps@string i j";
  return ::fit_in_int(result);
}

biguint mod(biguint i, biguint j) {
  biguint result;
  logic "divide @@prefixes@temps@string result i j";
  return result;
}

long mod(long i, long j) {
  long result;
  logic "divide @@prefixes@temps@string result i j";
  return ::fit_in_long(result);
}

int mod(int i, int j) {
  int result;
  logic "divide @@prefixes@temps@string result i j";
  return ::fit_in_int(result);
}

biguint power(biguint i, biguint j) {
  logic "power i i j";
  return i;
}

long power(long i, long j) {
  logic "power i i j";
  return ::fit_in_long(i);
}

int power(int i, int j) {
  logic "power i i j";
  return ::fit_in_int(i);
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

biguint self_inc_post(biguint& x) {
  biguint r = x;
  logic "add x x @@prefixes@constants@int_1";
  return r;
}

long self_inc_post(long& x) {
  long r = x;
  logic "add x x @@prefixes@constants@int_1";
  return ::fit_in_long(r);
}

int self_inc_post(int& x) {
  int r = x;
  logic "add x x @@prefixes@constants@int_1";
  return ::fit_in_int(r);
}

biguint self_dec_post(biguint& x) {
  biguint r = x;
  logic "subtract x x @@prefixes@constants@int_1";
  return r;
}

long self_dec_post(long& x) {
  long r = x;
  logic "subtract x x @@prefixes@constants@int_1";
  return ::fit_in_long(r);
}

int self_dec_post(int& x) {
  int r = x;
  logic "subtract x x @@prefixes@constants@int_1";
  return ::fit_in_int(r);
}

biguint self_inc_pre(biguint& x) {
  logic "add x x @@prefixes@constants@int_1";
  return x;
}

long self_inc_pre(long& x) {
  logic "add x x @@prefixes@constants@int_1";
  return ::fit_in_long(x);
}

int self_inc_pre(int& x) {
  logic "add x x @@prefixes@constants@int_1";
  return ::fit_in_int(x);
}

biguint self_dec_pre(biguint& x) {
  logic "subtract x x @@prefixes@constants@int_1";
  return x;
}

long self_dec_pre(long& x) {
  logic "subtract x x @@prefixes@constants@int_1";
  return ::fit_in_long(x);
}

int self_dec_pre(int& x) {
  logic "subtract x x @@prefixes@constants@int_1";
  return ::fit_in_int(x);
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
#endif

biguint left_shift(biguint i, int j) {
  return left_shift(i, ::to_biguint(j));
}

biguint right_shift(biguint i, int j) {
  return right_shift(i, ::to_biguint(j));
}

biguint left_shift(biguint i, long j) {
  return left_shift(i, ::to_biguint(j));
}

biguint right_shift(biguint i, long j) {
  return right_shift(i, ::to_biguint(j));
}

int left_shift(int i, int j) {
  return ::to_int(
    left_shift(::to_biguint(i),
               ::to_biguint(j))
  );
}

int right_shift(int i, int j) {
  return ::to_int(
    right_shift(::to_biguint(i),
                ::to_biguint(j))
  );
}

long left_shift(long i, long j) {
  return ::to_long(
    left_shift(::to_biguint(i),
               ::to_biguint(j))
  );
}

long right_shift(long i, long j) {
  return ::to_long(
    right_shift(::to_biguint(i),
                ::to_biguint(j))
  );
}

void self_and(bool& i, bool j) {
  i = and(i, j);
}

void self_or(bool& i, bool j) {
  i = or(i, j);
}

void self_add(biguint& i, biguint j) {
  i = add(i, j);
}

void self_add(long& i, long j) {
  i = add(i, j);
}

void self_add(int& i, int j) {
  i = add(i, j);
}

void self_add(byte& i, byte j) {
  i = add(i, j);
}

void self_minus(biguint& i, biguint j) {
  i = minus(i, j);
}

void self_minus(long& i, long j) {
  i = minus(i, j);
}

void self_minus(int& i, int j) {
  i = minus(i, j);
}

void self_multiply(biguint& i, biguint j) {
  i = multiply(i, j);
}

void self_multiply(long& i, long j) {
  i = multiply(i, j);
}

void self_multiply(int& i, int j) {
  i = multiply(i, j);
}

void self_divide(biguint& i, biguint j) {
  i = divide(i, j);
}

void self_divide(long& i, long j) {
  i = divide(i, j);
}

void self_divide(int& i, int j) {
  i = divide(i, j);
}

void self_mod(biguint& i, biguint j) {
  i = mod(i, j);
}

void self_mod(long& i, long j) {
  i = mod(i, j);
}

void self_mod(int& i, int j) {
  i = mod(i, j);
}

void self_power(biguint& i, biguint j) {
  i = power(i, j);
}

void self_power(long& i, long j) {
  i = power(i, j);
}

void self_power(int& i, int j) {
  i = power(i, j);
}

void self_bit_and(biguint& i, biguint j) {
  i = bit_and(i, j);
}

void self_bit_and(long& i, long j) {
  i = bit_and(i, j);
}

void self_bit_and(int& i, int j) {
  i = bit_and(i, j);
}

void self_bit_or(biguint& i, biguint j) {
  i = bit_or(i, j);
}

void self_bit_or(long& i, long j) {
  i = bit_or(i, j);
}

void self_bit_or(int& i, int j) {
  i = bit_or(i, j);
}

void self_left_shift(biguint& i, biguint j) {
  i = left_shift(i, j);
}

void self_right_shift(biguint& i, biguint j) {
  i = right_shift(i, j);
}

void self_left_shift(long& i, long j) {
  i = left_shift(i, j);
}

void self_right_shift(long& i, long j) {
  i = right_shift(i, j);
}

void self_left_shift(int& i, int j) {
  i = left_shift(i, j);
}

void self_right_shift(int& i, int j) {
  i = right_shift(i, j);
}

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_OPERATORS_H