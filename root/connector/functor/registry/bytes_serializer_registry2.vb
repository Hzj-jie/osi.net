
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Text
Imports osi.root.constants

<global_init(global_init_level.functor)>
Friend NotInheritable Class bytes_serializer_registry2
    Shared Sub New()
        bytes_serializer.byte_size.register(Function(ByVal i As String) As UInt32
                                                Return str_byte_count(i)
                                            End Function,
                                            Function(ByVal i As String, ByVal o As MemoryStream) As Boolean
                                                If String.IsNullOrEmpty(i) Then
                                                    Return True
                                                End If
                                                Return o.write(str_bytes(i))
                                            End Function,
                                            Function(ByVal l As UInt32,
                                                     ByVal i As MemoryStream,
                                                     ByRef o As String) As Boolean
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                If bytes_str(b, offset, l, o) Then
                                                    i.Seek(l, SeekOrigin.Current)
                                                    Return True
                                                End If
                                                Return False
                                            End Function)
        bytes_serializer(Of StringBuilder).forward_registration.from(Of String)()
        bytes_serializer.fixed.register(Function(ByVal i As Boolean, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write_byte(If(i, max_uint8, min_uint8))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As Boolean) As Boolean
                                            Dim b As Int32 = 0
                                            b = i.ReadByte()
                                            If b = npos Then
                                                Return False
                                            Else
                                                o = (b = max_uint8)
                                                Return True
                                            End If
                                        End Function)
        bytes_serializer.byte_size.register(Function(ByVal i() As Byte) As UInt32
                                                Return array_size(i)
                                            End Function,
                                            Function(ByVal i() As Byte, ByVal o As MemoryStream) As Boolean
                                                assert(Not o Is Nothing)
                                                If isemptyarray(i) Then
                                                    Return True
                                                End If
                                                Return o.write(i)
                                            End Function,
                                            Function(ByVal l As UInt32,
                                                     ByVal i As MemoryStream,
                                                     ByRef o() As Byte) As Boolean
                                                If l = 0 Then
                                                    Return True
                                                End If
                                                ReDim o(CInt(l - uint32_1))
                                                Return i.read(o)
                                            End Function)
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
