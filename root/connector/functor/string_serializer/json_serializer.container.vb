
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO

Partial Public NotInheritable Class json_serializer(Of T)
    Public NotInheritable Class container(Of ELEMENT)
        Private Shared Sub register(ByVal prefix As String,
                                    ByVal trailing As String)
            json_serializer.register(Function(ByVal i As T, ByVal o As StringWriter) As Boolean
                                         assert(o IsNot Nothing)
                                         o.Write(prefix)

                                         Dim it As container_operator(Of ELEMENT).enumerator = Nothing
                                         it = container_operator(Of T, ELEMENT).r.enumerate(i)
                                         If it Is Nothing Then
                                             Return True
                                         End If

                                         While Not it.end()
                                             If Not json_serializer.to_str(it.current(), o) Then
                                                 Return False
                                             End If
                                             it.next()
                                             If Not it.end() Then
                                                 o.Write(",")
                                             End If
                                         End While

                                         o.Write(trailing)

                                         Return True
                                     End Function)
        End Sub

        Public Shared Sub register_as_object()
            register("{", "}")
        End Sub

        Public Shared Sub register_as_array()
            register("[", "]")
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
