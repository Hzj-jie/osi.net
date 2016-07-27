
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.formation

'for utt
Public MustInherit Class iqless_test
    Inherits multithreading_case_wrapper

    Private Shared Shadows Function threadcount() As Int32
        Return (8 << (If(isreleasebuild(), 2, 0)))
    End Function

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New(ByVal ctor As Func(Of Int64, [case]))
        MyBase.New(repeat(ctor(round() * threadcount()),
                          round()),
                   threadcount())
    End Sub
End Class

Public Class qless_test
    Inherits iqless_test

    Public Sub New()
        MyBase.New(Function(x As Int64) As [case]
                       Return New qless_case(x)
                   End Function)
    End Sub
End Class

Public Class qless2_test
    Inherits iqless_test

    Public Sub New()
        MyBase.New(Function(x As Int64) As [case]
                       Return New qless2_case(x)
                   End Function)
    End Sub
End Class

Public Class heapless_test
    Inherits iqless_test

    Public Sub New()
        MyBase.New(Function(x As Int64) As [case]
                       Return New heapless_case(x)
                   End Function)
    End Sub
End Class

#If 0 Then
Public Class cycle_test
    Inherits iqless_test

    Public Sub New()
        MyBase.New(Function(x As Int64) As [case]
                       Return New cycle_1024_128_case(x)
                   End Function)
    End Sub
End Class
#End If
