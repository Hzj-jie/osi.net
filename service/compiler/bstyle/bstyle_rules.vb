
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
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(692), _
        "H4sIAAAAAAAEAI1Uy46bMBTdW/I/XE13LXzDrKKqm6aazqZCaOSAQyyMHdmmUf++1y8wJFG7Scy573MflHz7+v34dvh4//Xj8BNOkqmRkrfj8T0hZyE5JZT4f2jOs+qc0Kr9ErD0BYpNPP5Yx4yrT4Z1I3fQXJlhkxTWta/AVb8Iplk6UVuuHFcdr73aYNj1QslikYzrm3CXutPTxNrPEKCkVEgiDuGdpGtOPtMlADQ5aPU0iZaSDK7qMZrlk8CQUqsKNhI9u0JYeNjbQfObyZnXPT8LJTx7FWSkk5hCCSEwW8zUcDcbtXxm3uuOSVnBeENeORsrkHoQHVK9htslsk0Tmk6rPiVxu2CH0bc2tdT62nranjGU2vyAVeyR73PRUEr2BRfjwqwVg5rQOFa8Ki9clI0sWXnmgJKlKM+MOO9mMqhthrEog0uL/rN9+0rJFvEePQKbAgN1XhQf/x0v7FDi25vje79Ad3Rsupvkj7DE0m7v7vuS1cOYJrU8fnliEtzmBpS6/6q2tCl8rackDbHvZgUCp2jgBsdQaoZ5nLRGmXVGqKEtT06wenhyYrj7m5MTibelKCTfls0AFrcllpRuCyWbXfRdi8DSCq/zCQ7TiffA4kb6FB0PQ9oLwzsn/+CuipEn8QuWjTo98gPsI1b7QkkUYoTsxOOU/AWyuRVEswUAAA=="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
