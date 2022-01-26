
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

}  // namespace testing
}  // namespace b2style

#endif  // TESTING_ASSERT_H
