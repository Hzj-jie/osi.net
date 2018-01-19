
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Partial Public NotInheritable Class bytes_serializer
    Public NotInheritable Class fixed
        Public Shared Sub register(Of T)(ByVal to_bytes As Func(Of T, MemoryStream, Boolean),
                                         ByVal from_bytes As _do_val_ref(Of MemoryStream, T, Boolean))
            bytes_serializer(Of T).fixed.register(to_bytes, from_bytes)
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class [variant]
        Public Shared Sub register(Of T)(ByVal append_to As Func(Of T, MemoryStream, Boolean),
                                         ByVal write_to As Func(Of T, MemoryStream, Boolean),
                                         ByVal consume_from As _do_val_ref(Of MemoryStream, T, Boolean),
                                         ByVal read_from As _do_val_ref(Of MemoryStream, T, Boolean))
            bytes_serializer(Of T).variant.register(append_to, write_to, consume_from, read_from)
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class byte_size
        Public Shared Sub register(Of T)(ByVal sizeof As Func(Of T, UInt32),
                                         ByVal write_to As Func(Of T, MemoryStream, Boolean),
                                         ByVal read_from As _do_val_val_ref(Of UInt32, MemoryStream, T, Boolean))
            bytes_serializer(Of T).byte_size.register(sizeof, write_to, read_from)
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared Function append_to(Of T)(ByVal i As T, ByVal o As MemoryStream) As Boolean
        Return bytes_serializer(Of T).default.append_to(i, o)
    End Function

    Public Shared Function write_to(Of T)(ByVal i As T, ByVal o As MemoryStream) As Boolean
        Return bytes_serializer(Of T).default.write_to(i, o)
    End Function

    Public Shared Function append_to(Of T)(ByVal i As T, ByVal o() As Byte, ByRef offset As UInt32) As Boolean
        Return bytes_serializer(Of T).default.append_to(i, o, offset)
    End Function

    Public Shared Function to_bytes(Of T)(ByVal i As T) As Byte()
        Return bytes_serializer(Of T).default.to_bytes(i)
    End Function

    Public Shared Function consume_from(Of T)(ByVal i As MemoryStream, ByRef o As T) As Boolean
        Return bytes_serializer(Of T).default.consume_from(i, o)
    End Function

    Public Shared Function read_from(Of T)(ByVal i As MemoryStream, ByRef o As T) As Boolean
        Return bytes_serializer(Of T).default.read_from(i, o)
    End Function

    Public Shared Function from_bytes(Of T)(ByVal i() As Byte, ByRef o As T) As Boolean
        Return bytes_serializer(Of T).default.from_bytes(i, o)
    End Function

    Public Shared Function consume_from(Of T)(ByVal i() As Byte, ByRef offset As UInt32, ByRef o As T) As Boolean
        Return bytes_serializer(Of T).default.consume_from(i, offset, o)
    End Function

    Private Sub New()
    End Sub
End Class
