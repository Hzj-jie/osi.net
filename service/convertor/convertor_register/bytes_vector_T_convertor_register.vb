
#If RETIRED
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

<global_init(global_init_level.services)>
Friend Class bytes_vector_T_convertor_binder_initializer
    Shared Sub New()
        bytes_vector_T_convertor_register(Of SByte).assert_bind()
        bytes_vector_T_convertor_register(Of Byte).assert_bind()
        bytes_vector_T_convertor_register(Of Int16).assert_bind()
        bytes_vector_T_convertor_register(Of UInt16).assert_bind()
        bytes_vector_T_convertor_register(Of Int32).assert_bind()
        bytes_vector_T_convertor_register(Of UInt32).assert_bind()
        bytes_vector_T_convertor_register(Of Int64).assert_bind()
        bytes_vector_T_convertor_register(Of UInt64).assert_bind()
        bytes_vector_T_convertor_register(Of String).assert_bind()
        bytes_vector_T_convertor_register(Of Single).assert_bind()
        bytes_vector_T_convertor_register(Of Double).assert_bind()
        bytes_vector_T_convertor_register(Of Boolean).assert_bind()
        bytes_vector_T_convertor_register(Of Char).assert_bind()

        bytes_convertor_register(Of vector(Of Byte())).assert_bind(
            Function(i() As Byte, ByRef offset As UInt32, ByRef o As vector(Of Byte())) As Boolean
                Return i.bytes_vector_bytes(o, offset)
            End Function,
            Function(i() As Byte, ii As UInt32, il As UInt32, ByRef o As vector(Of Byte())) As Boolean
                Return i.bytes_vector_bytes(ii, il, o)
            End Function,
            Function(i As vector(Of Byte()), o() As Byte, ByRef offset As UInt32) As Boolean
                Return i.vector_bytes_bytes_val(o, offset)
            End Function,
            Function(i As vector(Of Byte()), ByRef o() As Byte) As Boolean
                Return i.vector_bytes_bytes_ref(o)
            End Function)
    End Sub

    Private Shared Sub init()
    End Sub
End Class

Public Class bytes_vector_T_convertor_register(Of T)
    Public Shared Function bind() As Boolean
        Return bytes_convertor_register(Of vector(Of T)).bind(
                   Function(b() As Byte, ByRef offset As UInt32, ByRef o As vector(Of T)) As Boolean
                       Dim v As vector(Of Byte()) = Nothing
                       Return b.bytes_vector_bytes(v, offset) AndAlso
                              v.from_vector_bytes(o)
                   End Function,
                   Function(b() As Byte, ii As UInt32, il As UInt32, ByRef o As vector(Of T)) As Boolean
                       Dim v As vector(Of Byte()) = Nothing
                       Return b.bytes_vector_bytes(ii, il, v) AndAlso
                              v.from_vector_bytes(o)
                   End Function,
                   Function(i As vector(Of T), o() As Byte, ByRef offset As UInt32) As Boolean
                       Dim v As vector(Of Byte()) = Nothing
                       Return i.to_vector_bytes(v) AndAlso
                              v.vector_bytes_bytes_val(o, offset)
                   End Function,
                   Function(i As vector(Of T), ByRef o() As Byte) As Boolean
                       Dim v As vector(Of Byte()) = Nothing
                       Return i.to_vector_bytes(v) AndAlso
                              v.vector_bytes_bytes_ref(o)
                   End Function)
    End Function

    Public Shared Sub assert_bind()
        assert(bind())
    End Sub

    Private Sub New()
    End Sub
End Class
#End If
