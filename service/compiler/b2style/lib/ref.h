
#ifndef B2STYLE_LIB_REF_H
#define B2STYLE_LIB_REF_H

#include <b2style.h>

namespace b2style {

template <T>
class ref {
  int _a;

  void construct() {
    this._a = -1;
  }

  void construct(T v) {
    
  }

  void set(T v) {
  }
};

}  // namespace b2style

#endif  // B2STYLE_LIB_REF_H
