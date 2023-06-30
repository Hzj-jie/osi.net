
Public Interface ioutputter(Of T)
    Function output(ByVal i As T, ByVal base As Byte) As String
End Interface

Public Class int_outputter
    Implements ioutputter(Of Int32)

    Public Shared ReadOnly instance As int_outputter

    Shared Sub New()
        instance = New int_outputter()
    End Sub

    Private Sub New()
    End Sub

    Public Function output(ByVal i As Int32,
                           ByVal base As Byte) As String Implements ioutputter(Of Int32).output
        Dim r As big_int = Nothing
        r = New big_int(i)
        Return r.str(base)
    End Function
End Class

Public Class uint_outputter
    Implements ioutputter(Of UInt32)

    Public Shared ReadOnly instance As uint_outputter

    Shared Sub New()
        instance = New uint_outputter()
    End Sub

    Private Sub New()
    End Sub

    Public Function output(ByVal i As UInt32,
                           ByVal base As Byte) As String Implements ioutputter(Of UInt32).output
        Dim r As big_uint = Nothing
        r = New big_uint(i)
        Return r.str(base)
    End Function
End Class

Public Class big_uint_outputter
    Implements ioutputter(Of big_uint)

    Public Shared ReadOnly instance As big_uint_outputter

    Shared Sub New()
        instance = New big_uint_outputter()
    End Sub

    Private Sub New()
    End Sub

    Public Function output(ByVal i As big_uint,
                           ByVal base As Byte) As String Implements ioutputter(Of big_uint).output
        If i Is Nothing Then
            Return Nothing
        Else
            Return i.str(base)
        End If
    End Function
End Class

Public Class big_int_outputter
    Implements ioutputter(Of big_int)

    Public Shared ReadOnly instance As big_int_outputter

    Shared Sub New()
        instance = New big_int_outputter()
    End Sub

    Private Sub New()
    End Sub

    Public Function output(ByVal i As big_int,
                           ByVal base As Byte) As String Implements ioutputter(Of big_int).output
        If i Is Nothing Then
            Return Nothing
        Else
            Return i.str(base)
        End If
    End Function
End Class
