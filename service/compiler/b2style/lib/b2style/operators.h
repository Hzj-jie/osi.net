
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

// TODO: Use a better way to compare strings, treating them as big_uints is not accurate or efficient.
bool equal(string i, string j) {
  bool result;
  logic "equal b2style__result b2style__i b2style__j";
  return result;
}

bool not_equal(string i, string j) {
  bool result;
  logic "equal b2style__result b2style__i b2style__j";
  return not(result);
}

bool equal(biguint i, biguint j) {
  bool result;
  logic "equal b2style__result b2style__i b2style__j";
  return result;
}

bool not_equal(biguint i, biguint j) {
  bool result;
  logic "equal b2style__result b2style__i b2style__j";
  return not(result);
}

bool equal(long i, long j) {
  return equal(::bstyle::to_biguint(i), ::bstyle::to_biguint(j));
}

bool not_equal(long i, long j) {
  return not_equal(::bstyle::to_biguint(i), ::bstyle::to_biguint(j));
}

bool equal(int i, int j) {
  return equal(::bstyle::to_biguint(i), ::bstyle::to_biguint(j));
}

bool not_equal(int i, int j) {
  return not_equal(::bstyle::to_biguint(i), ::bstyle::to_biguint(j));
}

bool equal(bool i, bool j) {
  if (i) return j;
  return !j;
}

bool not_equal(bool i, bool j) {
  if (i) return !j;
  return j;
}

bool greater_than(biguint i, biguint j) {
  bool result;
  logic "more b2style__result b2style__i b2style__j";
  return result;
}

bool greater_than(long i, long j) {
  return greater_than(::bstyle::to_biguint(i), ::bstyle::to_biguint(j));
}

bool greater_than(int i, int j) {
  return greater_than(::bstyle::to_biguint(i), ::bstyle::to_biguint(j));
}

bool less_than(biguint i, biguint j) {
  bool result;
  logic "less b2style__result b2style__i b2style__j";
  return result;
}

bool less_than(long i, long j) {
  return less_than(::bstyle::to_biguint(i), ::bstyle::to_biguint(j));
}

bool less_than(int i, int j) {
  return less_than(::bstyle::to_biguint(i), ::bstyle::to_biguint(j));
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
  logic "add b2style__i b2style__i b2style__j";
  return i;
}

long add(long i, long j) {
  logic "add b2style__i b2style__i b2style__j";
  return ::bstyle::fit_in_long(i);
}

int add(int i, int j) {
  logic "add b2style__i b2style__i b2style__j";
  return ::bstyle::fit_in_int(i);
}

byte add(byte i, byte j) {
  logic "add b2style__i b2style__i b2style__j";
  return ::bstyle::fit_in_byte(i);
}

string add(string i, string j) {
  return ::bstyle::str_concat(i, j);
}

biguint minus(biguint i, biguint j) {
  logic "subtract b2style__i b2style__i b2style__j";
  return i;
}

long minus(long i, long j) {
  logic "subtract b2style__i b2style__i b2style__j";
  return ::bstyle::fit_in_long(i);
}

int minus(int i, int j) {
  logic "subtract b2style__i b2style__i b2style__j";
  return ::bstyle::fit_in_int(i);
}

biguint multiply(biguint i, biguint j) {
  logic "multiply b2style__i b2style__i b2style__j";
  return i;
}

long multiply(long i, long j) {
  logic "multiply b2style__i b2style__i b2style__j";
  return ::bstyle::fit_in_long(i);
}

int multiply(int i, int j) {
  logic "multiply b2style__i b2style__i b2style__j";
  return ::bstyle::fit_in_int(i);
}

biguint divide(biguint i, biguint j) {
  biguint result;
  logic "divide b2style__result @@prefixes@temps@string b2style__i b2style__j";
  return result;
}

long divide(long i, long j) {
  long result;
  logic "divide b2style__result @@prefixes@temps@string b2style__i b2style__j";
  return ::bstyle::fit_in_long(result);
}

int divide(int i, int j) {
  int result;
  logic "divide b2style__result @@prefixes@temps@string b2style__i b2style__j";
  return ::bstyle::fit_in_int(result);
}

biguint mod(biguint i, biguint j) {
  biguint result;
  logic "divide @@prefixes@temps@string b2style__result b2style__i b2style__j";
  return result;
}

long mod(long i, long j) {
  long result;
  logic "divide @@prefixes@temps@string b2style__result b2style__i b2style__j";
  return ::bstyle::fit_in_long(result);
}

int mod(int i, int j) {
  int result;
  logic "divide @@prefixes@temps@string b2style__result b2style__i b2style__j";
  return ::bstyle::fit_in_int(result);
}

biguint power(biguint i, biguint j) {
  logic "power b2style__i b2style__i b2style__j";
  return i;
}

long power(long i, long j) {
  logic "power b2style__i b2style__i b2style__j";
  return ::bstyle::fit_in_long(i);
}

int power(int i, int j) {
  logic "power b2style__i b2style__i b2style__j";
  return ::bstyle::fit_in_int(i);
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
  return ::bstyle::fit_in_long(r);
}

int self_inc_post(int& x) {
  int r = x;
  logic "add b2style__x b2style__x @@prefixes@constants@int_1";
  return ::bstyle::fit_in_int(r);
}

biguint self_dec_post(biguint& x) {
  biguint r = x;
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return r;
}

long self_dec_post(long& x) {
  long r = x;
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return ::bstyle::fit_in_long(r);
}

int self_dec_post(int& x) {
  int r = x;
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return ::bstyle::fit_in_int(r);
}

biguint self_inc_pre(biguint& x) {
  logic "add b2style__x b2style__x @@prefixes@constants@int_1";
  return x;
}

long self_inc_pre(long& x) {
  logic "add b2style__x b2style__x @@prefixes@constants@int_1";
  return ::bstyle::fit_in_long(x);
}

int self_inc_pre(int& x) {
  logic "add b2style__x b2style__x @@prefixes@constants@int_1";
  return ::bstyle::fit_in_int(x);
}

biguint self_dec_pre(biguint& x) {
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return x;
}

long self_dec_pre(long& x) {
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return ::bstyle::fit_in_long(x);
}

int self_dec_pre(int& x) {
  logic "subtract b2style__x b2style__x @@prefixes@constants@int_1";
  return ::bstyle::fit_in_int(x);
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

biguint left_shift(biguint i, int j) {
  return left_shift(i, ::bstyle::to_biguint(j));
}

biguint right_shift(biguint i, int j) {
  return right_shift(i, ::bstyle::to_biguint(j));
}

biguint left_shift(biguint i, long j) {
  return left_shift(i, ::bstyle::to_biguint(j));
}

biguint right_shift(biguint i, long j) {
  return right_shift(i, ::bstyle::to_biguint(j));
}

int left_shift(int i, int j) {
  return ::bstyle::to_int(
    left_shift(::bstyle::to_biguint(i),
               ::bstyle::to_biguint(j))
  );
}

int right_shift(int i, int j) {
  return ::bstyle::to_int(
    right_shift(::bstyle::to_biguint(i),
                ::bstyle::to_biguint(j))
  );
}

long left_shift(long i, long j) {
  return ::bstyle::to_long(
    left_shift(::bstyle::to_biguint(i),
               ::bstyle::to_biguint(j))
  );
}

long right_shift(long i, long j) {
  return ::bstyle::to_long(
    right_shift(::bstyle::to_biguint(i),
                ::bstyle::to_biguint(j))
  );
}

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_OPERATORS_H