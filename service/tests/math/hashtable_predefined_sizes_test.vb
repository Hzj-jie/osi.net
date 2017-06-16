
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.math

Public Class hashtable_predefined_sizes_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New delegate_case(Sub()
                                         Dim x As Int32 = 1
                                         Do
                                             x <<= 1
                                             While x < max_int32 - 1 AndAlso (Not x.is_prime() OrElse x.even())
                                                 x += 1
                                             End While
                                             Console.Write(x)
                                             Console.Write(", ")
                                         Loop While x < (max_int32 >> 1)
                                         Console.WriteLine()
                                     End Sub))
    End Sub
End Class
