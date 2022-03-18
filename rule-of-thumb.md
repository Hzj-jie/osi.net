
Rule of Thumb
==========

1. C-Style naming conventions

1. Use Module *ONLY* when \<Extension\> functions are included or serving only
   private methods for \<global\_init\>, e.g. no global functions.

1. Only use \<Extension\> functions as member functions.

1. Unless explicitly claimed in the function name, \<Extension\> functions
   should throw NullReferenceException or assert(this != null).

1. Static / Shared methods within a meaningful named class for "global
   functions".

1. Length of line: 120

1. Always include
  - Option Explicit On
  - Option Infer Off
  - Option Strict On
   at the top of files.

1. Always sort and remove imports.

1. Leave a vertical space at the top of the file to avoid introducing BOM issue.

1. Avoid using optional argument, consider to use overload instead.

1. Do not use cctor / shared new, see shared\_ctor\_perf, unless the class is
   implemented for global initialization only without public constructor, static
   members, etc.

1. Do not use empty init() function to trigger shared ctor, it won't cover
   shared variable, see shared\_ctor\_behavior\_test and BeforeFieldInit.

1. Do not use Convert.ToString against enum types, it returns integers rather
   than the meaningful tag names.

1. Never rely on the format of ToString(), it should be used for debugging
   purpose only. If a certain format is required, use a different function
   with more meaningful name.

1. Do not use namespace within a project, use class. Namespaces live in project
   level only. (?)
