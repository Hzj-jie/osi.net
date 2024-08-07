
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Namespace primitive
    Partial Public NotInheritable Class interrupts
        Public Shared ReadOnly [default] As New interrupts()
        Private ReadOnly io As console_io
        Private ReadOnly v As New vector(Of Func(Of Byte(), Byte()))()
        Private ReadOnly m As New unordered_map(Of String, UInt32)()
        Private ReadOnly lm As New loaded_method()

        Public Sub New(ByVal io As console_io)
            assert(Not io Is Nothing)
            Me.io = io
            v.emplace_back(AddressOf stdout)
            v.emplace_back(AddressOf stderr)
            v.emplace_back(AddressOf stdin)
            v.emplace_back(AddressOf current_ms)
            v.emplace_back(AddressOf load_method)
            v.emplace_back(AddressOf execute_loaded_method)
            v.emplace_back(AddressOf getchar)
            v.emplace_back(AddressOf putchar)

            For i As UInt32 = 0 To v.size() - uint32_1
                assert(Not v(i) Is Nothing)
                m(v(i).Method().Name()) = i
            Next
        End Sub

        Public Sub New()
            Me.New(New console_io())
        End Sub

        Public Function invoke(ByVal i As UInt32, ByVal j() As Byte) As Byte()
            If i >= v.size() Then
                executor_stop_error.throw(executor.error_type.undefined_interrupt)
                Return Nothing
            End If
            assert(Not v(i) Is Nothing)
            Return v(i)(j)
        End Function

        Public Function [of](ByVal name As String, ByRef o As UInt32) As Boolean
            Return m.find(name, o)
        End Function
    End Class
End Namespace
