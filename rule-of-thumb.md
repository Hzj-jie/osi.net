
Rule of Thumb
==========

1. C-Style naming conventions

1. Use Module *ONLY* when \<Extension\> functions are included or serving only
   private methods for \<global\_init\>, e.g. no global functions.

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
