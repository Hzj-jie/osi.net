
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Partial Public Class struct(Of T)
    Private Shared Function serialize(Of S)(ByVal op As struct(Of T),
                                            ByVal i As T,
                                            ByVal o As S,
                                            ByVal f As Func(Of Type, Object, S, Boolean)) As Boolean
        assert(Not f Is Nothing)
        Dim vs() As struct.variable = Nothing
        If Not (+op).disassemble(i, vs) Then
            Return False
        End If
        For j As Int32 = 0 To array_size_i(vs) - 1
            If Not f(vs(j).type, vs(j).value, o) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Shared Function deserialize(Of S)(ByVal op As struct(Of T),
                                              ByVal i As S,
                                              ByRef o As T,
                                              ByVal f As _do_val_val_ref(Of Type, S, Object, Boolean)) As Boolean
        assert(Not f Is Nothing)
        Dim vs() As Object = Nothing
        ReDim vs(array_size_i((+op).definitions()) - 1)
        For j As Int32 = 0 To array_size_i((+op).definitions()) - 1
            If Not f((+op).definitions()(j).type, i, vs(j)) Then
                Return False
            End If
        Next
        Return (+op).assemble(vs, o)
    End Function

    ' Register functors for the type T as struct.
    Public Shared Sub register(ByVal op As struct(Of T))
        bytes_serializer(Of T).fixed.register(Function(ByVal i As T, ByVal o As MemoryStream) As Boolean
                                                  Return serialize(op,
                                                                   i,
                                                                   o,
                                                                   AddressOf type_bytes_serializer.append_to)
                                              End Function,
                                              Function(ByVal i As MemoryStream, ByRef o As T) As Boolean
                                                  Return deserialize(op,
                                                                     i,
                                                                     o,
                                                                     AddressOf type_bytes_serializer.consume_from)
                                              End Function)
#If TODO Then
        ' This implementation does not work, concating multiple strings into one is not right.
        string_serializer.register(Function(ByVal i As T, ByVal o As StringWriter) As Boolean
                                       Return serialize(op, i, o, AddressOf type_string_serializer.to_str)
                                   End Function,
                                   Function(ByVal i As StringReader, ByRef o As T) As Boolean
                                       Return deserialize(op, i, o, AddressOf type_string_serializer.from_str)
                                   End Function)
#End If
    End Sub

    Public Shared Sub register()
        register(Nothing)
    End Sub
End Class
