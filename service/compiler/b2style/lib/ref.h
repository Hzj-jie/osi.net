
#ifndef B2STYLE_LIB_REF_H
#define B2STYLE_LIB_REF_H

#include <b2style.h>
#include <assert.h>
#include <bstyle/heap.h>

namespace b2style {

template <T>
class ref {
  ::bstyle::heap_ptr _a;

  void destruct() {
    ::bstyle::dealloc(this._a);
  }

  void set(T v) {
    this.destruct();
    ::bstyle::alloc(this._a, 1);
    this._a[0] = v;
  }

  T get() {
    ::assert(this._a != ::bstyle::npos());
    return this._a[0];
  }

  ::bstyle::heap_ptr release() {
    ::bstyle::heap_ptr r = this._a;
    this._a = ::bstyle::npos();
    return r;
  }

  void construct() {
    this._a = ::bstyle::npos();
  }

  void construct(T v) {
    set(v);
  }

  void construct(::bstyle::heap_ptr p) {
    this._a = p;
  }

  void construct(ref& other) {
    this.construct(other.release());
  }
};

}  // namespace b2style

#endif  // B2STYLE_LIB_REF_H
