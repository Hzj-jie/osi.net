
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
        Return bytes_serializer(Of T).r.append_to(i, o)
    End Function

    Public Shared Function write_to(Of T)(ByVal i As T, ByVal o As MemoryStream) As Boolean
        Return bytes_serializer(Of T).r.write_to(i, o)
    End Function

    Public Shared Function append_to(Of T)(ByVal i As T, ByVal o() As Byte, ByRef offset As UInt32) As Boolean
        Return bytes_serializer(Of T).r.append_to(i, o, offset)
    End Function

    Public Shared Function to_bytes(Of T)(ByVal i As T) As Byte()
        Return bytes_serializer(Of T).r.to_bytes(i)
    End Function

    Public Shared Function consume_from(Of T)(ByVal i As MemoryStream, ByRef o As T) As Boolean
        Return bytes_serializer(Of T).r.consume_from(i, o)
    End Function

    Public Shared Function read_from(Of T)(ByVal i As MemoryStream, ByRef o As T) As Boolean
        Return bytes_serializer(Of T).r.read_from(i, o)
    End Function

    Public Shared Function from_bytes(Of T)(ByVal i() As Byte, ByRef o As T) As Boolean
        Return bytes_serializer(Of T).r.from_bytes(i, o)
    End Function

    Public Shared Function consume_from(Of T)(ByVal i() As Byte, ByRef offset As UInt32, ByRef o As T) As Boolean
        Return bytes_serializer(Of T).r.consume_from(i, offset, o)
    End Function

    Public Shared Function from_container(Of CONTAINER, ELEMENT, T)(ByVal i As CONTAINER, ByRef o As T) As Boolean
        Return bytes_serializer(Of T).r.from_container(Of CONTAINER, ELEMENT)(i, o)
    End Function

    Public Shared Function to_container(Of CONTAINER, ELEMENT, T)(ByVal i As T, ByRef o As CONTAINER) As Boolean
        Return bytes_serializer(Of T).r.to_container(Of CONTAINER, ELEMENT)(i, o)
    End Function

    Public Shared Function from_container(Of ELEMENT)() As container_from(Of ELEMENT)
        Return container_from(Of ELEMENT).instance
    End Function

    Public Shared Function to_container(Of ELEMENT)() As container_to(Of ELEMENT)
        Return container_to(Of ELEMENT).instance
    End Function

    Public NotInheritable Class container_from(Of ELEMENT)
        Public Shared ReadOnly instance As container_from(Of ELEMENT) = New container_from(Of ELEMENT)()

        Public Function [of](Of CONTAINER)(ByVal i As CONTAINER) As container_from_to(Of CONTAINER, ELEMENT)
            Return New container_from_to(Of CONTAINER, ELEMENT)(i)
        End Function

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class container_from_to(Of CONTAINER, ELEMENT)
        Private ReadOnly i As CONTAINER

        Public Sub New(ByVal i As CONTAINER)
            Me.i = i
        End Sub

        Public Function [to](Of T)(ByRef o As T) As Boolean
            Return from_container(Of CONTAINER, ELEMENT, T)(i, o)
        End Function
    End Class

    Public NotInheritable Class container_to(Of ELEMENT)
        Public Shared ReadOnly instance As container_to(Of ELEMENT) = New container_to(Of ELEMENT)()

        Public Function from(Of T)(ByVal i As T) As container_to_to(Of T, ELEMENT)
            Return New container_to_to(Of T, ELEMENT)(i)
        End Function

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class container_to_to(Of T, ELEMENT)
        Private ReadOnly i As T

        Public Sub New(ByVal i As T)
            Me.i = i
        End Sub

        Public Function [to](Of CONTAINER)(ByRef o As CONTAINER) As Boolean
            Return to_container(Of CONTAINER, ELEMENT, T)(i, o)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
