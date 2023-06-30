
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'rlexer_rule rlexer_rule2 syntaxer_rule cycle_dependency_syntaxer_rule
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module syntaxer_test_rules
    Public ReadOnly rlexer_rule() As Byte
    Public ReadOnly rlexer_rule2() As Byte
    Public ReadOnly syntaxer_rule() As Byte
    Public ReadOnly cycle_dependency_syntaxer_rule() As Byte

    Sub New()
        rlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(428), _
        "H4sIAAAAAAAEAG2QTU7DMBBG95Z8h6EroDEHgP5sKBJCYsvCidCkmaRWHbu1HXWBuDt2QqCULDKZ+d6T7DFnj5un59cNGGxJdB5LTZCfsrzK3jnj7OVNqBpULfMzoRA9IO0JUpmCtXUQvyl02ql4SF+ncGWhslNAW3uAVKago9A5A8Pvn8BZqdHsQeZlMY+DtRpkcB1lNcYFimRo8l5YJ+jYoYbFkrPGEYbfaBWjoVsuv/WwQwOL0eynFWfovWpMSyZAFLe2bRGy1Ghr4J6zY0c+KGtEi24Pa858QBfEAR02Dg87+IgHmeos+Byd0uF2TwGuB2Mcb0bu4/0c/eS5HLyLOO7rqVViuNFD2t4Hp0wDMzkrrm5nnNXaYgA5z0SxlnlVzO/6ypkygRpy54iz9NyQevH36SP6AqRExrxiAgAA"))

        assert(rlexer_rule.ungzip(rlexer_rule))
        rlexer_rule2 = Convert.FromBase64String(strcat_hint(CUInt(412), _
        "H4sIAAAAAAAEAF1PS07DMBDdW/Idhq6AxhwA+tnACiGWLJKomiSTxKpjp7ajqkLcHdshVcXCz++n8Zizj8/XN/CXkQ51b2RN0Fmi5sIZZ+9fQrYg28RIOYIISbXGQjiJn3upCBIm3RhoTGLKmBEiJGXJT1bDfMX5lUJ9hLyoynUQxijIvZ0oazE8U8aGIueE71HDhrOwGPpZ7f4iYwWdJlSw2S751doFa2bbwNA52emBtIegajMMCFkkymh45uw0kfPSaDGgPcKeM+fRejGixc7i2MN3mKabG+Nn6VQW6yN5uJ8bi3xYcheWsHT1i3zu/bPDfx0NUswbvcTfO2+l7mCVr8q7xxVnrTLoIV9notznRVOunxJyJrWnjuxtxJnGgSBykRfnrGiyQ3R/ASVwGljxAQAA"))

        assert(rlexer_rule2.ungzip(rlexer_rule2))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(552), _
        "H4sIAAAAAAAEAJ1Ty2rDMBC8C/wPey7WN+QUSik0JQ2UYkxRHMUR0SOV5Pb3q6ctO0kPvdjSrDQ7O6ut0NPjy2a7/tx9vK7fYM+JPFdou9nsEnIcZGeZkhWqUF6DJILGj7FEW7zXpDtTC82FaCI4M7ZdAZWHMSAGbhk2VFoqO4r9sV6Ty6lC4410Gf8we8KdEoK0DxCgdKiIRBzCOkUnTREI9NDklPVdCW2FMjgdj7kMFcwl5ErWMIuowS6CeV2wLTmg+SZ8oPhAj0wy72MNEek4GYyTqKkdtBy32W7cEc7bIskix1wNNJ2Sh8DvxNyrOjXuhlPOdd+5okVL2aXVZQGxGiDGsF4KRxeBCo2C4Pkds+Pi0cRb5WspVFFuHP1Y0KpCc8QzegSWer0NwoMmGD3zsg7ia2Cu4p5qF+WK2Br2SrmYsZrJvvWyR4LUqOx2GWk4NQbbE3FZek2JTesAK43p10B4Dk378GuzQ3/Q/0f57M7NQY0Jryc1S4kTOckaJ/JKcJrIaHqayNlD9i2KQCL0XfwFjNeLGXgEAAA="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
        cycle_dependency_syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(140), _
        "H4sIAAAAAAAEAHu/ez9XkL9/SHxIZIBrsEJZYk5pKhcXmFKILihK1c0vSC1KLMnMz9MFC+ooFOQXl2CKliUWZSYm5aTq5iXmpsZyYdGpUJyak6abmZcMtQSbOQqoKgEME9PbnQAAAA=="))

        assert(cycle_dependency_syntaxer_rule.ungzip(cycle_dependency_syntaxer_rule))
    End Sub
End Module
