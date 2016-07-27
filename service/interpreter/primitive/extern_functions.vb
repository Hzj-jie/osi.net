
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Namespace primitive
    Public Class extern_functions
        Private Shared ReadOnly v As vector(Of Func(Of Byte(), Byte()))
        Private Shared ReadOnly m As map(Of String, UInt32)

        Shared Sub New()
            v = New vector(Of Func(Of Byte(), Byte()))()
            v.emplace_back(AddressOf stdout)
            v.emplace_back(AddressOf stderr)
            v.emplace_back(AddressOf stdin)

            m = New map(Of String, UInt32)()
            For i As UInt32 = 0 To v.size() - uint32_1
                assert(Not v(i) Is Nothing)
                m(v(i).Method().Name()) = i
            Next
        End Sub

        Public Shared Function stdout(ByVal i() As Byte) As Byte()
            console_io.output().Write(bytes_str(i))
            Return Nothing
        End Function

        Public Shared Function stderr(ByVal i() As Byte) As Byte()
            console_io.error().Write(bytes_str(i))
            Return Nothing
        End Function

        Public Shared Function stdin(ByVal i() As Byte) As Byte()
            Return str_bytes(console_io.input().ReadLine())
        End Function

        Public Shared Function involve(ByVal i As UInt32, ByVal j() As Byte) As Byte()
            If i >= v.size() Then
                executor_stop_error.throw(executor.error_type.undefined_extern_function)
                Return Nothing
            Else
                assert(Not v(i) Is Nothing)
                Return v(i)(j)
            End If
        End Function

        Public Shared Function find_extern_function(ByVal name As String, ByRef o As UInt32) As Boolean
            Dim it As map(Of String, UInt32).iterator = Nothing
            it = m.find(name)
            If it = m.end() Then
                Return False
            Else
                o = (+it).second
                Return True
            End If
        End Function
    End Class
End Namespace
