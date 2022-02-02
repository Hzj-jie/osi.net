
#ifndef TESTING_ASSERT_H
#define TESTING_ASSERT_H

#include <bstyle.h>
#include <testing/types.h>

namespace b2style {
namespace testing {

int _assertion_count = 0;

void assert_true(bool v, string msg) {
  _assertion_count++;
  string prefix;
  if (v) {
    prefix = "Success: ";
  } else {
    prefix = "Failure: ";
  }
  ::b2style::std_out(prefix);
  ::b2style::std_out(msg);
  ::b2style::std_out("\n");
}

void assert_true(bool v) {
  assert_true(v, "no extra information.");
}

/*
TODO: Make this work. Currently assert_equal__2 conflicts with the following one.
template <T, T2>
void assert_equal(T t, T2 t2, string msg) {
  assert_true(t == t2, msg);
}
*/

template <T, T2>
void assert_equal(T t, T2 t2) {
  assert_true(t == t2);
}

template <T>
void assert_equal(T t, T t2) {
  assert_equal<T, T>(t, t2);
}

}  // namespace testing
}  // namespace b2style

#endif  // TESTING_ASSERT_H
