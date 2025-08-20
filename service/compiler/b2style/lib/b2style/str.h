
#ifndef B2STYLE_LIB_B2STYLE_STR_H
#define B2STYLE_LIB_B2STYLE_STR_H

#include <b2style/types.h>
#include <bstyle/str.h>
#include <assert.h>

namespace b2style {

#ifdef B3STYLE
string _str_mid(string s, int i, int l) {
  string r;
  logic "cut_len r s i l";
  return r;
}
#else
string _str_mid(string s, int i, int l) {
  string r;
  logic "cut_len b2style__r b2style__s b2style__i b2style__l";
  return r;
}
#endif

string str_mid(string s, int i, int l) {
  ::assert(__STATEMENT__ + "@" + __FILE__, (i >= 0) && (l >= 0));
  return _str_mid(s, i, l);
}

bool str_ends_with(string i, string j) {
  int il = ::str_len(i);
  int jl = ::str_len(j);
  if (il < jl) {
    return false;
  }
  if (il == jl) {
    return ::str_equal(i, j);
  }
  string si = str_mid(i, il - jl, jl);
  return ::str_equal(si, j);
}

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_STR_H
