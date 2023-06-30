
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class error_writer_ignore_types(Of T)
    Private Shared ReadOnly s(255) As Boolean

    Private Shared Function to_int32(ByVal e As Char) As Int32
        Dim x As Int32 = 0
        x = Convert.ToInt32(e)
        assert(x >= 0 AndAlso x < array_size(s))
        Return x
    End Function

    Private Shared Function to_int32(ByVal e As error_type) As Int32
        assert(e > error_type.first AndAlso e < error_type.last)
        Return to_int32(error_message.error_type_char(e))
    End Function

    Public Shared Sub ignore_all()
        For i As Int32 = 0 To s.array_size_i() - 1
            s(i) = True
        Next
    End Sub

    Public Shared Sub value_all()
        For i As Int32 = 0 To s.array_size_i() - 1
            s(i) = False
        Next
    End Sub

    Public Shared Sub ignore(ByVal ParamArray e() As error_type)
        For i As Int32 = 0 To e.array_size_i() - 1
            s(to_int32(e(i))) = True
        Next
    End Sub

    Public Shared Sub ignore(ByVal ParamArray e() As Char)
        For i As Int32 = 0 To e.array_size_i() - 1
            s(to_int32(e(i))) = True
        Next
    End Sub

    Public Shared Sub value(ByVal ParamArray e() As error_type)
        For i As Int32 = 0 To e.array_size_i() - 1
            s(to_int32(e(i))) = False
        Next
    End Sub

    Public Shared Sub value(ByVal ParamArray e() As Char)
        For i As Int32 = 0 To e.array_size_i() - 1
            s(to_int32(e(i))) = False
        Next
    End Sub

    Public Shared Function valued(ByVal e As error_type, ByVal c As Char) As Boolean
        Return Not ignored(e, c)
    End Function

    Public Shared Function valued(ByVal e As error_type) As Boolean
        Return Not ignored(e)
    End Function

    Public Shared Function valued(ByVal e As Char) As Boolean
        Return Not ignored(e)
    End Function

    Public Shared Function ignored(ByVal e As error_type, ByVal c As Char) As Boolean
        Return If(e = error_type.other, ignored(c), ignored(e))
    End Function

    Public Shared Function ignored(ByVal e As error_type) As Boolean
        Return s(to_int32(e))
    End Function

    Public Shared Function ignored(ByVal e As Char) As Boolean
        Return s(to_int32(e))
    End Function

    Private Sub New()
    End Sub
End Class
