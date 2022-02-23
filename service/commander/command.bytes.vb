
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

<global_init(global_init_level.server_services)>
Partial Public Class command
    Shared Sub New()
        assert(global_init_level.services < global_init_level.server_services)
        bytes_serializer(Of constants.action).forward_registration.from(Of SByte)()
        bytes_serializer(Of constants.response).forward_registration.from(Of SByte)()
        bytes_serializer(Of constants.parameter).forward_registration.from(Of SByte)()

        bytes_serializer.fixed.register(Function(ByVal i As command, ByVal o As MemoryStream) As Boolean
                                            assert(i IsNot Nothing)
                                            Return bytes_serializer.append_to(i.a, o) AndAlso
                                                   bytes_serializer.append_to(i.ps, o)
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As command) As Boolean
                                            Dim a As array_ref(Of Byte) = Nothing
                                            Dim ps As map(Of array_ref(Of Byte), Byte()) = Nothing
                                            If bytes_serializer.consume_from(i, a) AndAlso
                                               bytes_serializer.consume_from(i, ps) Then
                                                o = New command(a, ps)
                                                Return True
                                            End If
                                            Return False
                                        End Function)
    End Sub

    Private Shared Sub init()
    End Sub
End Class
