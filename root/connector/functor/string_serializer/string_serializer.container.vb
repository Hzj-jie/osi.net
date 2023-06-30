
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO

Partial Public Class string_serializer(Of T)
    Public NotInheritable Class container(Of E)
        Public Shared Sub register(ByVal sep As String)
            string_serializer.register(Function(ByVal i As T, ByVal o As StringWriter) As Boolean
                                           assert(Not o Is Nothing)
                                           Dim it As container_operator(Of E).enumerator =
                                               container_operator(Of T, E).r.enumerate(i)
                                           If it Is Nothing Then
                                               Return True
                                           End If

                                           While Not it.end()
                                               If Not string_serializer.to_str(it.current(), o) Then
                                                   Return False
                                               End If
                                               it.next()
                                               If Not it.end() Then
                                                   o.Write(sep)
                                               End If
                                           End While
                                           Return True
                                       End Function,
                                       Function(ByVal i As StringReader, ByRef o As T) As Boolean
                                           assert(Not i Is Nothing)
                                           ' TODO: If it's possible to use strspliter in utils.
                                           Dim ss() As String = i.ReadToEnd().
                                                                  Split({sep}, StringSplitOptions.RemoveEmptyEntries)
                                           o = alloc(Of T)()
                                           For Each s As String In ss
                                               Dim c As E = Nothing
                                               If Not string_serializer.from_str(s, c) Then
                                                   Return False
                                               End If
                                               If Not container_operator(Of T, E).r.emplace(o, c) Then
                                                   Return False
                                               End If
                                           Next
                                           Return True
                                       End Function)
        End Sub

        Public Shared Sub register()
            register(",")
        End Sub
    End Class
End Class
