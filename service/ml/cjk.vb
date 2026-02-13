
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.resource

Partial Public NotInheritable Class cjk
    Private Shared Sub one_str(ByVal s As String, ByVal f As Action(Of String, UInt32, UInt32))
        assert(Not f Is Nothing)
        If s.null_or_whitespace() Then
            Return
        End If
        s.strsep(AddressOf _character.not_cjk,
                 Sub(ByVal a As UInt32, ByVal b As UInt32)
                     f(s, a, b)
                 End Sub)
    End Sub

    Public Shared Sub per_str_from(ByVal ss As IEnumerable(Of String), ByVal f As Action(Of String, UInt32, UInt32))
        For Each s As String In ss
            one_str(s, f)
        Next
    End Sub

    Public Shared Sub per_str_from(ByVal reader As tar.reader, ByVal f As Action(Of String, UInt32, UInt32))
        assert(Not reader Is Nothing)
        reader.foreach(Sub(ByVal name As String, ByVal p As Double, ByVal r As StreamReader)
                           If p < 0.8 Then
                               raise_error(error_type.user,
                                           "ignroe ",
                                           name,
                                           ", the encoding possibility is ",
                                           p)
                               Return
                           End If
                           Dim line As String = r.ReadLine()
                           While Not line Is Nothing
                               one_str(line, f)
                               line = r.ReadLine()
                           End While
                       End Sub)
    End Sub

    Private Sub New()
    End Sub
End Class
