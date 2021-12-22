
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'nlexer_rule syntaxer_rule
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module b2style_rules
    Public ReadOnly nlexer_rule() As Byte
    Public ReadOnly syntaxer_rule() As Byte

    Sub New()
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(444), _
        "H4sIAAAAAAAEAFWRsU7DMBRFd0v+h1chGKBO96rpUhiQEExMpCATvzYWrh1shxaJP2Pgk/gFbCdO6WLde/Tsd2/y+/1Dye396u7x+gaKYuYablHMtMID2hfbKSz8wVNCydueab5D1/IaYVSJ14o7B+mMk2ewMruWW+mMpkShc8xYhu8dV7AoKdla5B7tES4D7FUZlDZ+4JNyuO4brmFxvJn8st/10KLl3lhKuBBQXVGyk7pzwILolJet+oTqkhIhP6RAmAVsBJxT0po9Wnim5FV6xrWAi14aC9VXeC2SgJKNIOSCCSUO1YZJXYdVcVmyAmtgLIbdeOYaufGwCHGt3DbZL1PcNN3nLAc3pB3tMXNGOfk4EvNnM7TIduzyH6QKGfS9shvaZXtSIMPTGmXsEf8/PFX76ct0Pl9HUYmkp8U6fOs/n8S0S1YCAAA="))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(948), _
        "H4sIAAAAAAAEALVWO2/cMAzeBfQ/COhWWMnepUPToUDRDGknwyhkm7aF6CRXj1zu35eWJb8TXIB0OYvUR4n8+NCR7z+//vh9943e3Nzajhuob+1FOf4M5o/xEm7csyPEaO2Yu/RA88arygmtMvrEpQdWQyOUGDTsLFzHLJwEq7QcEB3wHgGV5IYfI9IZr0AUP4HteQUso0JV0teQ0XArfkWjcMnOhvc91BmVuhXV/ozB9YDbblhnfOUyivdbW5AevTiFG2leCse4qosvQSazG/TxzCZpBFvHjWODdYuedDSfCCs+UVD1vEUsKAcKj9n4QnPRKm2AGbBeOha4G91jiXNWcSkx6BVws7nNyj4LB6wnFSq8RVoNOG/UJGLApQH+GPlF2kA2bG0SKc5oj67pHmI6AwiV2rqtFpmdwyfhyBGhDc2DyOs63nUSytu0xqhFLy9RrMWTGEpi3NPJotdnMHEdU7mQdNqa1ZNKQuOY7UTjosKItoua4gPZxY7cGcFLCWyshVUgAUgabZjUuh+YxHWsl9Lw6hEczfc522VoRVa0ONJN+ThI0fsU2FvyO5R+inJuAcPPI4Lmrzixq+sFycMkcNAOCS5F61HIqG+k5vgttZahsYVqCzIyMPSa9jvGQwtOXmPZJ8eWdjOA5l5xc9lHX4ojfUEO4TS/nkFyAKW50qkycR6mLoCqiMX2kT5gsLKm1ve9Nu7waBrPOjiGHOIXSTuy2V16zEm6NnR2bOq5n1Mrhy6ODTz1bmrbIAwLCdYy13FMXIvTyYGJUtjAjoO/nst5c9bED/I4ahJzL/i8CP19/J7ny2K0/I+A1pNp1W1jGwXNuBx+C7KFzAhaazc+hVfOEfriDrnOZvG0bhpXCuvW44VgBf66v7v/TB9iAaKhE9U0Rkh44ocBPC6O3+0r/o+kA7cv+/Ix+wfKRwT2VgkAAA=="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
