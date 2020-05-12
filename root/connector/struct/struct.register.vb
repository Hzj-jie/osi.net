
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

    Private Shared Function compare(Of RT)(ByVal op As struct(Of T),
                                           ByVal l As T,
                                           ByVal r As T,
                                           ByVal typed_cmp As Func(Of Type, Object, Object, RT),
                                           ByVal early_return As Func(Of RT, Boolean),
                                           ByVal obj_compare_result As Func(Of Int32, RT),
                                           ByVal default_result As RT) As RT
        assert(Not typed_cmp Is Nothing)
        assert(Not early_return Is Nothing)
        assert(Not obj_compare_result Is Nothing)

        Dim cmp As Int32 = 0
        cmp = object_compare(l, r)
        If cmp <> object_compare_undetermined Then
            Return obj_compare_result(cmp)
        End If
        assert(Not l Is Nothing)
        assert(Not r Is Nothing)
        Dim vsl() As struct.variable = Nothing
        Dim vsr() As struct.variable = Nothing
        assert((+op).disassemble(l, vsl))
        assert((+op).disassemble(r, vsr))
        assert(array_size(vsl) = array_size(vsr))
        assert(array_size(vsl) = array_size((+op).definitions()))
        For i As Int32 = 0 To array_size_i(vsl) - 1
            Dim result As RT = Nothing
            result = typed_cmp((+op).definitions()(i).type, vsl(i).value, vsr(i).value)
            If early_return(result) Then
                Return result
            End If
        Next
        Return default_result
    End Function

    Private Shared Function compare(ByVal op As struct(Of T), ByVal l As T, ByVal r As T) As Int32
        Return compare(op,
                       l,
                       r,
                       Function(ByVal t As Type, ByVal x As Object, ByVal y As Object) As Int32
                           Return type_comparer.compare(t, t, x, y)
                       End Function,
                       Function(ByVal result As Int32) As Boolean
                           Return result <> 0
                       End Function,
                       Function(ByVal result As Int32) As Int32
                           Return result
                       End Function,
                       0)
    End Function

    Private Shared Function equal(ByVal op As struct(Of T), ByVal l As T, ByVal r As T) As Boolean
        Return compare(op,
                       l,
                       r,
                       Function(ByVal t As Type, ByVal x As Object, ByVal y As Object) As Boolean
                           Return type_equaler.equal(t, t, x, y)
                       End Function,
                       Function(ByVal result As Boolean) As Boolean
                           Return Not result
                       End Function,
                       Function(ByVal result As Int32) As Boolean
                           Return result = 0
                       End Function,
                       True)
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
        comparer.register(Function(ByVal i As T, ByVal j As T) As Int32
                              Return compare(op, i, j)
                          End Function)
        equaler.register(Function(ByVal i As T, ByVal j As T) As Boolean
                             Return equal(op, i, j)
                         End Function)
#If TODO Then
        ' This implementation does not work, concating multiple strings into one is not right.
        string_serializer.register(Function(ByVal i As T, ByVal o As StringWriter) As Boolean
                                       Return serialize(op, i, o, AddressOf type_string_serializer.default.to_str)
                                   End Function,
                                   Function(ByVal i As StringReader, ByRef o As T) As Boolean
                                       Return deserialize(op, i, o, AddressOf type_string_serializer.default.from_str)
                                   End Function)
#End If
        json_serializer.register(Function(ByVal i As T, ByVal o As StringWriter) As Boolean
                                     o.Write("{")
                                     Dim not_first As Boolean = False
                                     If Not serialize(op,
                                                      i,
                                                      o,
                                                      Function(ByVal type As Type,
                                                               ByVal x As Object,
                                                               ByVal s As StringWriter) As Boolean
                                                          If not_first Then
                                                              o.Write(",")
                                                          Else
                                                              not_first = True
                                                          End If
                                                          Return type_json_serializer.r.to_str(type, x, s)
                                                      End Function) Then
                                         Return False
                                     End If
                                     o.Write("}")
                                     Return True
                                 End Function)
    End Sub

    Public Shared Sub register()
        register(Nothing)
    End Sub
End Class
