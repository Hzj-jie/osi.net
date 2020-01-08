
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'nlexer_rule syntaxer_rule
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module bstyle_rules
    Public ReadOnly nlexer_rule() As Byte
    Public ReadOnly syntaxer_rule() As Byte

    Sub New()
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(368), _
        "H4sIAAAAAAAEAF1QUW6EIBT8N+EOL361rvYAbZo9CJjmqU+XiOA+MH60PVk/eqReoYJrs+kHw8y8gQz8fH2LbFwr3YPuEyPjCSIk1TuGbSW+XrQhSJh056BziRnnZoiQFFNY2MK+JadhwhES3vKDbiGhyETWGLQjSNXUpySdMyADL1T2uBWpRaZtoIEY5Kms6rNUXUz2xmG4s4qn28AH1naAXKq8LD7yusjjta2bJgRVRmachWeRXRfyQTtbTcgjnONJ5FDNyDgwzhd4FxnZ7s74PDINYztSgIc9ccjHY+6vCzL9+Uruuf/29jZPk672Si+xJ3qvBzuRDfAatcWJts9Zy7c6ouo2UsTBLzgR6ae8AQAA"))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(676), _
        "H4sIAAAAAAAEAI1Uy27jIBTdI/EPV53dyP6GrqJRN82o7aayrIpg4iBjiAA3mr+fiwEHO4lmNhY+933ug5KXX6/7t93Xx+fv3TscFNMDJW/7/UdCjpPmXhpNCSX5DZqNIn6cZ9bXB8v4IDw0Z2bZqKTz7TMI3S2CcVJe1k5oLzQXdVDrLTufKFksknF9kf5UczOOrP0JM5SUCknEYX4n6TWnkOkSAJoctHqYREtJBq/qMZoTo8SQyugKVhIz+UJYeNjaQfPN1CTqThylloG9CjLCFaZQQghMDjO1wk9WL7+Z95ozpSoYLsirYEMFyvSSI9XXcJtE1mlCw43uUhKXk1TBt7G1MubcBtoeMZTafIdV7FHoc9FQSrYFF+PCnJO9HtE4VnxVXrgoG1my8sgBJUtRgRl53MzkrLYaxqIMoRz6z/btMyVrJHgMCKwKnKkLovj473jzDiW+gzm+twt0Q8equ0l+D0ssbfbuti9ZfR7TpJbHL09MgtvcgFL3X9WWNoUvaEL7bkZZ4hT1wiKuDMM8DsYg6ryVum/LkzPr3z05MdztzcmJxNtSFJJvy2oAi9sSS0q3hZLVLoauRWBpRdD5AbvxIDpgcSNDil7MQ9pJK7hXf3BX5SCS+AnLRp0O+QH2Fat9oiQKMUJ2EnBK/gJLppwhowUAAA=="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
