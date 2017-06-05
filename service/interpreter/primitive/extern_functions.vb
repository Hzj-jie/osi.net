
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Namespace primitive
    Public Class extern_functions
        Public Shared ReadOnly [default] As extern_functions
        Private ReadOnly io As console_io
        Private ReadOnly v As vector(Of Func(Of Byte(), Byte()))
        Private ReadOnly m As map(Of String, UInt32)

        Shared Sub New()
            [default] = New extern_functions()
        End Sub

        Public Sub New(ByVal io As console_io)
            assert(Not io Is Nothing)
            Me.io = io
            v = New vector(Of Func(Of Byte(), Byte()))()
            v.emplace_back(AddressOf stdout)
            v.emplace_back(AddressOf stderr)
            v.emplace_back(AddressOf stdin)
            v.emplace_back(AddressOf current_ms)

            m = New map(Of String, UInt32)()
            For i As UInt32 = 0 To v.size() - uint32_1
                assert(Not v(i) Is Nothing)
                m(v(i).Method().Name()) = i
            Next
        End Sub

        Public Sub New()
            Me.New(New console_io())
        End Sub

        Public Function stdout(ByVal i() As Byte) As Byte()
            io.output().Write(bytes_str(i))
            Return Nothing
        End Function

        Public Function stderr(ByVal i() As Byte) As Byte()
            io.error().Write(bytes_str(i))
            Return Nothing
        End Function

        Public Function stdin(ByVal i() As Byte) As Byte()
            Return str_bytes(io.input().ReadToEnd())
        End Function

        Public Function current_ms(ByVal i() As Byte) As Byte()
            Return int64_bytes(nowadays.milliseconds())
        End Function

        Public Function invoke(ByVal i As UInt32, ByVal j() As Byte) As Byte()
            If i >= v.size() Then
                executor_stop_error.throw(executor.error_type.undefined_extern_function)
                Return Nothing
            Else
                assert(Not v(i) Is Nothing)
                Return v(i)(j)
            End If
        End Function

        Public Function find_extern_function(ByVal name As String, ByRef o As UInt32) As Boolean
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
