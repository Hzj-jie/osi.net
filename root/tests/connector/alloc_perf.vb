
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class alloc_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New alloc_test(), 1024 * 64))
    End Sub

    Public Overrides Function prepare() As Boolean
        suppress.alloc_error.inc()
        Return MyBase.prepare()
    End Function

    Public Overrides Function finish() As Boolean
        suppress.alloc_error.dec()
        Return MyBase.finish()
    End Function
End Class

' alloc<T>() costs ~30x more time than New operator in release build.
Public NotInheritable Class alloc_perf2
    Inherits performance_comparison_case_wrapper

    Private Const round As Int64 = 1024 * 1024 * 10

    Private NotInheritable Class test_class
        Public Sub New()
        End Sub
    End Class

    Private NotInheritable Class test_class2
    End Class

    Private Class test_class3
        Protected Sub New(ByVal a As Int32, ByVal b As Boolean, ByVal c As Object)
        End Sub
    End Class

    Private MustInherit Class test_class4
        Public Sub New(ByVal a As String, ByVal b As Int32)
        End Sub
    End Class

    Private Class test_class5
        Protected Sub New()
        End Sub
    End Class

    Private NotInheritable Class alloc_perf2_case
        Inherits [case]

        Private Shared Function alloc_vector_string() As Boolean
            alloc(Of vector(Of String))()
            Return True
        End Function

        Private Shared Function alloc_int() As Boolean
            alloc(Of Int32)()
            Return True
        End Function

        Private Shared Function alloc_object() As Boolean
            alloc(Of Object)()
            Return True
        End Function

        Private Shared Function alloc_test_class() As Boolean
            alloc(Of test_class)()
            Return True
        End Function

        Private Shared Function alloc_test_class2() As Boolean
            alloc(Of test_class2)()
            Return True
        End Function

        Private Shared Function alloc_test_class3() As Boolean
            alloc(Of test_class3)()
            Return True
        End Function

        Private Shared Function alloc_action() As Boolean
            alloc(Of Action)()
            Return True
        End Function

        Private Shared Function alloc_func() As Boolean
            alloc(Of Func(Of Boolean))()
            Return True
        End Function

        Private Shared Function alloc_test_class4() As Boolean
            alloc(Of test_class4)()
            Return True
        End Function

        Private Shared Function alloc_test_class5() As Boolean
            alloc(Of test_class5)()
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return alloc_vector_string() AndAlso
                   alloc_int() AndAlso
                   alloc_object() AndAlso
                   alloc_test_class() AndAlso
                   alloc_test_class2() AndAlso
                   alloc_test_class3() AndAlso
                   alloc_action() AndAlso
                   alloc_func() AndAlso
                   alloc_test_class4() AndAlso
                   alloc_test_class5()
        End Function
    End Class

    Private NotInheritable Class new_perf2_case
        Inherits [case]

        Private NotInheritable Class test_class3
            Inherits alloc_perf2.test_class3

            Public Sub New(ByVal a As Int32, ByVal b As Boolean, ByVal c As String)
                MyBase.New(a, b, c)
            End Sub
        End Class

        Private NotInheritable Class test_class4
            Inherits alloc_perf2.test_class4
            Public Sub New(ByVal a As String, ByVal b As Int32)
                MyBase.New(a, b)
            End Sub
        End Class

        Private NotInheritable Class test_class5
            Inherits alloc_perf2.test_class5
            Public Sub New()
                MyBase.New()
            End Sub
        End Class

        Public Overrides Function run() As Boolean
            Using code_block
                Dim v As New vector(Of String)()
            End Using
            Using code_block
                Dim v As New Int32()
            End Using
            Using code_block
                Dim v As New Object()
            End Using
            Using code_block
                Dim v As New test_class()
            End Using
            Using code_block
                Dim v As New test_class2()
            End Using
            Using code_block
                Dim v As New test_class3(0, False, Nothing)
            End Using
            Using code_block
                Dim v As Action = Sub()
                                  End Sub
            End Using
            Using code_block
                Dim v As Func(Of Boolean) = Function() As Boolean
                                                Return False
                                            End Function
            End Using
            Using code_block
                Dim v As New test_class4(Nothing, 0)
            End Using
            Using code_block
                Dim v As New test_class5()
            End Using
            Return True
        End Function
    End Class

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({11473, 390}, i, j)
    End Function

    Public Sub New()
        MyBase.New(repeat(New alloc_perf2_case(), round), repeat(New new_perf2_case(), round))
    End Sub
End Class

' For simple empty constructor, the performance penalty of alloc<T> is even more noticeable as alloc<T> uses around 50x
' more time than new.
Public NotInheritable Class alloc_perf3
    Inherits performance_comparison_case_wrapper

    Private Const round As Int64 = 1024 * 1024 * 10

    Private NotInheritable Class test_class
    End Class

    Private NotInheritable Class alloc_perf3_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim v As test_class = alloc(Of test_class)()
            Return True
        End Function
    End Class

    Private NotInheritable Class new_perf3_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim v As New test_class()
            Return True
        End Function
    End Class

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({9137, 197}, i, j)
    End Function

    Public Sub New()
        MyBase.New(repeat(New alloc_perf3_case(), round), repeat(New new_perf3_case(), round))
    End Sub
End Class

