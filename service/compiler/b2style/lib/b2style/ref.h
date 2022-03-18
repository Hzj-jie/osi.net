
#ifndef B2STYLE_LIB_B2STYLE_REF_H
#define B2STYLE_LIB_B2STYLE_REF_H

#include <b2style.h>
#include <b2style/types.h>
#include <assert.h>
#include <bstyle/heap.h>

namespace b2style {

template <T>
class ref {
  ::bstyle::heap_ptr _a;

  void destruct() {
    dealloc(this._a);
  }

  bool empty() {
    return this._a == ::bstyle::npos();
  }

  void set(T v) {
    this.destruct();
    ::bstyle::alloc(this._a, 1);
    reinterpret_cast(this._a, T);
    this._a[0] = v;
  }

  void alloc() {
    T v;
    this.set(v);
  }

  T get() {
    ::assert(!this.empty());
    reinterpret_cast(this._a, T);
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
    this.construct();
    this.set(v);
  }

  void construct(::bstyle::heap_ptr p) {
    this._a = p;
  }

  void construct(ref<T>& other) {
    this.construct(other.release());
  }
};

}  // namespace b2style

#endif  // B2STYLE_LIB_B2STYLE_REF_H