
#ifndef B2STYLE_LIB_B2STYLE_STR_H
#define B2STYLE_LIB_B2STYLE_STR_H

#include <b2style/types.h>
#include <bstyle/str.h>
#include <assert.h>

namespace b2style {

string str_mid(string s, int i, int l) {
  ::assert(__STATEMENT__ + "@" + __FILE__, (i >= 0) && (l >= 0));
  string r;
  logic "cut_len b2style__r b2style__s b2style__i b2style__l";
  return r;
}

bool str_ends_with(string i, string j) {
  int il = ::bstyle::str_len(i);
  int jl = ::bstyle::str_len(j);
  if (il < jl) {
    return false;
  }
  if (il == jl) {
    return ::bstyle::str_equal(i, j);
  }
  string si = str_mid(i, il - jl, jl);
  return ::bstyle::str_equal(si, j);
}

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_STR_H