
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.constants
Imports osi.root.delegates

Partial Public Class bytes_serializer(Of T)
    Public NotInheritable Class [variant]
        Public Shared Sub register(ByVal append_to As Func(Of T, MemoryStream, Boolean),
                                   ByVal write_to As Func(Of T, MemoryStream, Boolean),
                                   ByVal consume_from As _do_val_ref(Of MemoryStream, T, Boolean),
                                   ByVal read_from As _do_val_ref(Of MemoryStream, T, Boolean))
            default_functor.register.append_to(append_to)
            default_functor.register.write_to(write_to)
            default_functor.register.consume_from(consume_from)
            default_functor.register.read_from(read_from)
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class byte_size
        Public Shared Sub register(ByVal sizeof As Func(Of T, UInt32),
                                   ByVal write_to As Func(Of T, MemoryStream, Boolean),
                                   ByVal read_from As _do_val_val_ref(Of UInt32, MemoryStream, T, Boolean))
            assert(sizeof IsNot Nothing)
            assert(write_to IsNot Nothing)
            assert(read_from IsNot Nothing)
            [variant].register(Function(ByVal i As T, ByVal o As MemoryStream) As Boolean
                                   If Not bytes_serializer.append_to(sizeof(i), o) Then
                                       Return False
                                   End If
                                   Return write_to(i, o)
                               End Function,
                               write_to,
                               Function(ByVal i As MemoryStream, ByRef o As T) As Boolean
                                   Dim l As UInt32 = 0
                                   If Not bytes_serializer.consume_from(i, l) Then
                                       Return False
                                   End If
                                   Return read_from(l, i, o)
                               End Function,
                               Function(ByVal i As MemoryStream, ByRef o As T) As Boolean
                                   Return read_from(i.unread_length(), i, o)
                               End Function)
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class container(Of ELEMENT)
        Private Shared Function write_to(ByVal i As T, ByVal o As MemoryStream) As Boolean
            assert(o IsNot Nothing)
            Dim it As container_operator(Of ELEMENT).enumerator = Nothing
            it = container_operator(Of T, ELEMENT).r.enumerate(i)
            If it Is Nothing Then
                Return True
            End If

            While Not it.end()
                If Not bytes_serializer.append_to(it.current(), o) Then
                    Return False
                End If
                it.next()
            End While

            Return True
        End Function

        Public Shared Sub register(ByVal op As container_operator(Of T, ELEMENT))
            [variant].register(Function(ByVal i As T, ByVal o As MemoryStream) As Boolean
                                   assert(o IsNot Nothing)
                                   Dim l As UInt32 = 0
                                   l = container_operator(Of T, ELEMENT).r.size(i)
                                   If Not bytes_serializer.append_to(l, o) Then
                                       Return False
                                   End If

                                   Return write_to(i, o)
                               End Function,
                               Function(ByVal i As T, ByVal o As MemoryStream) As Boolean
                                   Return write_to(i, o)
                               End Function,
                               Function(ByVal i As MemoryStream, ByRef o As T) As Boolean
                                   assert(i IsNot Nothing)
                                   Dim l As UInt32 = 0
                                   If Not bytes_serializer.consume_from(i, l) Then
                                       Return False
                                   End If
                                   o = alloc(Of T)()
                                   While l > uint32_0
                                       Dim c As ELEMENT = Nothing
                                       If Not bytes_serializer.consume_from(i, c) Then
                                           Return False
                                       End If
                                       If Not container_operator.emplace(o, c) Then
                                           Return False
                                       End If
                                       l -= uint32_1
                                   End While
                                   Return True
                               End Function,
                               Function(ByVal i As MemoryStream, ByRef o As T) As Boolean
                                   o = container_operator(Of T, ELEMENT).r.renew(o)
                                   While True
                                       Dim c As ELEMENT = Nothing
                                       If Not bytes_serializer.consume_from(i, c) Then
                                           Return True
                                       End If
                                       If Not container_operator.emplace(o, c) Then
                                           Return False
                                       End If
                                   End While
                                   Return assert(False)
                               End Function)
        End Sub

        Public Shared Sub register()
            register(Nothing)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
