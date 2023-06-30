
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

Public NotInheritable Class bit_array_thread_safe_test2
    Public NotInheritable Class bit_array_thread_safe_test2_size_1000
        Inherits multithreading_case_wrapper

        Public Sub New()
            MyBase.New(_wrapper.repeat(New test_case(1000), 1000000), 10)
        End Sub
    End Class

    Public NotInheritable Class bit_array_thread_safe_test2_size_1023
        Inherits multithreading_case_wrapper

        Public Sub New()
            MyBase.New(_wrapper.repeat(New test_case(1023), 1000000), 10)
        End Sub
    End Class

    <test>
    Public NotInheritable Class bit_array_thread_safe_test2_size_1024
        Inherits multithreading_case_wrapper

        Public Sub New()
            MyBase.New(_wrapper.repeat(New test_case(1024), 1000000), 10)
        End Sub
    End Class

    Private Class test_case
        Inherits [case]

        Private ReadOnly size As UInt32
        Private ReadOnly b As bit_array_thread_safe

        Public Sub New(ByVal size As UInt32)
            Me.size = size
            b = New bit_array_thread_safe()
        End Sub

        Public Overrides Function prepare() As Boolean
            If Not MyBase.prepare() Then
                Return False
            End If
            b.resize(size)
            assertion.equal(b.size(), size)
            For i As UInt32 = 0 To b.size() - uint32_1
                assertion.is_false(b(i))
            Next
            Return True
        End Function

        Public Overrides Function run() As Boolean
            b(rnd_uint(0, b.size())) = True
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            For i As UInt32 = 0 To b.size() - uint32_1
                assertion.is_true(b(i))
            Next
            b.resize(0)
            Return MyBase.finish()
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
