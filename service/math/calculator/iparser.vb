
Public Interface iparser(Of T)
    Function parse(ByVal this As String, ByVal base As Byte, ByRef that As T) As Boolean
End Interface

Public Class int_parser
    Implements iparser(Of Int32)

    Public Shared ReadOnly instance As int_parser

    Shared Sub New()
        instance = New int_parser()
    End Sub

    Private Sub New()
    End Sub

    Public Function parse(ByVal this As String,
                          ByVal base As Byte,
                          ByRef that As Int32) As Boolean Implements iparser(Of Int32).parse
        Dim r As big_int = Nothing
        If big_int.parse(this, r, base) Then
            Dim o As Boolean = False
            that = r.as_int32(o)
            Return Not o
        Else
            Return False
        End If
    End Function
End Class

Public Class uint_parser
    Implements iparser(Of UInt32)

    Public Shared ReadOnly instance As uint_parser

    Shared Sub New()
        instance = New uint_parser()
    End Sub

    Private Sub New()
    End Sub

    Public Function parse(ByVal this As String,
                          ByVal base As Byte,
                          ByRef that As UInt32) As Boolean Implements iparser(Of UInt32).parse
        Dim r As big_uint = Nothing
        If big_uint.parse(this, r, base) Then
            Dim o As Boolean = False
            that = r.as_uint32(o)
            Return Not o
        Else
            Return False
        End If
    End Function
End Class

Public Class big_uint_parser
    Implements iparser(Of big_uint)

    Public Shared ReadOnly instance As big_uint_parser

    Shared Sub New()
        instance = New big_uint_parser()
    End Sub

    Private Sub New()
    End Sub

    Public Function parse(ByVal this As String,
                          ByVal base As Byte,
                          ByRef that As big_uint) As Boolean Implements iparser(Of big_uint).parse
        Return big_uint.parse(this, that, base)
    End Function
End Class

Public Class big_int_parser
    Implements iparser(Of big_int)

    Public Shared ReadOnly instance As big_int_parser

    Shared Sub New()
        instance = New big_int_parser()
    End Sub

    Private Sub New()
    End Sub

    Public Function parse(ByVal this As String,
                          ByVal base As Byte,
                          ByRef that As big_int) As Boolean Implements iparser(Of big_int).parse
        Return big_int.parse(this, that, base)
    End Function
End Class