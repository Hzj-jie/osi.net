
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
        bytes_serializer.fixed.register(Function(ByVal i As Decimal, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Dim b() As Int32 = Nothing
                                            b = Decimal.GetBits(i)
                                            assert(array_size(b) = 4)
                                            ' According to
                                            ' https://github.com/Microsoft/referencesource/blob/master/mscorlib/system/decimal.cs#L567
                                            ' The Decimal.GetBits() always uses little endian.
                                            For j As Int32 = 0 To array_size_i(b) - 1
                                                If Not o.write(BitConverter.GetBytes(
                                                               endian.to_little_endian(b(j)))) Then
                                                    Return False
                                                End If
                                            Next
                                            Return True
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As Decimal) As Boolean
                                            assert(Not i Is Nothing)
                                            Dim b() As Int32 = Nothing
                                            ReDim b(4 - 1)
                                            For j As Int32 = 0 To array_size_i(b) - 1
                                                If Not bytes_serializer.consume_from(i, b(j)) Then
                                                    Return False
                                                End If
                                            Next
                                            Try
                                                o = New Decimal(b)
                                                Return True
                                            Catch
                                                Return False
                                            End Try
                                        End Function)
        bytes_serializer.byte_size.register(Function(ByVal i As MemoryStream) As UInt32
                                                assert(Not i Is Nothing)
                                                assert(i.Length() <= max_uint32)
                                                Return CUInt(i.Length())
                                            End Function,
                                            Function(ByVal i As MemoryStream, ByVal o As MemoryStream) As Boolean
                                                assert(Not i Is Nothing)
                                                i.CopyTo(o)
                                                Return True
                                            End Function,
                                            Function(ByVal l As UInt32,
                                                     ByVal i As MemoryStream,
                                                     ByRef o As MemoryStream) As Boolean
                                                If l = 0 Then
                                                    o = New MemoryStream()
                                                    Return True
                                                End If
                                                Dim b() As Byte = Nothing
                                                ReDim b(CInt(l - uint32_1))
                                                If Not i.read(b) Then
                                                    Return False
                                                End If
                                                o = memory_stream.of(b)
                                                Return True
                                            End Function)
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
