
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'lang_parser_test_case0 lang_parser_test_case1
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module lang_parser_test_cases
    Public ReadOnly lang_parser_test_case0() As Byte
    Public ReadOnly lang_parser_test_case1() As Byte

    Sub New()
        lang_parser_test_case0 = Convert.FromBase64String(strcat_hint(CUInt(196), _
        "H4sIAAAAAAAEAG2OQQoCMQxF94Hc4S9bmEXqtuBdBmyHglZoO9JBvLspOihoNknIf48w3a7phMucsqmtpLxgLku1uKOEtpYM8XgwMaXcXrEx9AmjbZrTS4TpOI6thLUp0z1TONewOzZ4fCzR/XMYDIfYnflxfP0RD8YyKQat912tTmSCE6ukRp+OMfvg2wAAAA=="))

        assert(lang_parser_test_case0.ungzip(lang_parser_test_case0))
        lang_parser_test_case1 = Convert.FromBase64String(strcat_hint(CUInt(572), _
        "H4sIAAAAAAAEAIVT7YqDMBD8L/gOS+HAcDlp4i9p7auUJI2HYG1Re2iPe/fbmKSNVu78wE12Ms5uJnHU9W3VfEJ5axRL3GCgAC6EkcB3HAFeVQkGoM7XBAEbIdWGQFHA9oEwV6v7W9vAsLNTP6DrTq8ARg+II3ziqGp6kLXAJzFh14u2p2BC3ZxIHHkROOG5TexpBiiAbbduNBYsZbuA+SyGiVZYShkQYlUCDjjllQlHMgl3c9KTrdPZj1pjteOg/qqcMoo8p4Js0CGv49nG/1aocMWjvb6WFSWQyEMoJOCSAVfYCbXYtz8bYj/B9jkSAzevoJIqQk9k3l7rA56srmMUOIUszSiwPGd4k3Cnlej0sWo63XRVX33po7qcr6LV3tuo0EXyhd64G5cbhyNMkrmDqiYQZM8LuozCB0s5inlnacbznJPd0wKYx61GAHv1wRVF9GWyeavrcoO1IGZ+ZhaAD4eYFJ0uN1lrp2I6E7bVIwWXus/NOMC+MP9gZOmAEfbAU87J0lQmeYdDYbLZ43TcX1zpMxzr54tj73PmzztrmV9J7VXscwQAAA=="))

        assert(lang_parser_test_case1.ungzip(lang_parser_test_case1))
    End Sub
End Module
